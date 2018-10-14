using APICore.Model;
using APICore.Model.Selection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskTop.DTO;
using TaskTop.Model;
using TaskTop.Utils;

namespace TaskTop.Controllers
{
    public class ChangeEquipment
    {
        public int userId { get; set; }
        public int equipmentId { get; set; }
    }

    public class ChangeMaterial
    {
        public int userId { get; set; }
        public int materialId { get; set; }
        public int quantity { get; set; }
    }

    public class TransferTask
    {
        public int taskId { get; set; }
        public int userId { get; set; }
    }

    public class RateTask
    {
        public int taskId { get; set; }
        public int rate { get; set; }
        public int rateMax { get; set; }
    }

    [Authorize]
    public class TaskController : EntityController<Tarefa, TaskDTO, int>
    {
        public TaskController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Tarefa, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<TaskDTO, int>> GetExternalId => ent => ent.id;

        public override async Task<Tarefa> BeforeAdd(TaskDTO sendedData)
        {
            if (sendedData.name == null)
                throw new BadRequestExn("Campo nome obrigatório");

            var tarefa = await base.BeforeAdd(sendedData);
            tarefa.IniciadoEm = null;
            tarefa.FinalizadoEm = null;
            tarefa.InterrompidoEm = null;
            tarefa.Avaliacao = null;
            tarefa.Origem = tarefa.Origem == 0 ? Operator.Id : tarefa.Origem;

            return tarefa;
        }

        [HttpGet, ActionName("start")]
        public async Task<IActionResult> Start(int id)
        {
            var tsk = await InitialQuery.SingleOrDefaultAsync(t => t.Id == id);

            if (tsk == null)
                return NotFound();

            tsk.IniciadoEm = DateTime.UtcNow;

            DbContext.Entry(tsk).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return Ok(new { ok = true });
        }

        [HttpGet, ActionName("finish")]
        public async Task<IActionResult> Finish(int id)
        {
            var tsk = await InitialQuery.SingleOrDefaultAsync(t => t.Id == id);

            if (tsk == null)
                return NotFound();

            tsk.FinalizadoEm = DateTime.UtcNow;

            DbContext.Entry(tsk).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return Ok(new { ok = true });
        }

        [HttpPost, ActionName("rate")]
        public async Task<IActionResult> Rate([FromBody] RateTask ent)
        {
            var tsk = await InitialQuery.SingleOrDefaultAsync(t => t.Id == ent.taskId);

            tsk.Avaliacao = new TarefaAvaliacao
            {
                Nota = ent.rate,
                NotaMaxima = ent.rateMax
            };

            DbContext.Entry(tsk).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost, ActionName("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferTask ent)
        {
            var tsk = await InitialQuery.SingleOrDefaultAsync(t => t.Id == ent.taskId);

            if (tsk == null)
                return NotFound();

            tsk.InterrompidoEm = DateTime.UtcNow;

            DbContext.Tarefa.Add(new Tarefa
            {
                AgendadaEm = tsk.AgendadaEm,
                IniciadoEm = tsk.IniciadoEm,
                FinalizadoEm = tsk.FinalizadoEm,
                Origem = tsk.Origem,
                Destino = ent.userId,
                TarefaEquipamentos = tsk.TarefaEquipamentos,
                TarefaMateriais = tsk.TarefaMateriais,
                Nome = tsk.Nome,
                RepetirEm = tsk.RepetirEm
            });

            DbContext.Entry(tsk).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost, ActionName("material")]
        public async Task<IActionResult> AddMaterial([FromBody] ChangeMaterial ent)
        {
            var material = await DbContext.Material
                .SingleOrDefaultAsync(e => e.Id == ent.materialId);

            if (material == null)
                return NotFound();

            if (material.QuantidadeAtual - ent.quantity < 0)
                throw new ValidationExn("Estoque insuficiente.");

            var task = await DbContext.Tarefa
                .Where(t => t.Destino == ent.userId && t.FinalizadoEm == null)
                .OrderByDescending(t => t.IniciadoEm)
                .SingleOrDefaultAsync();

            if (task == null)
                throw new ValidationExn("Usuário sem tarefas abertas.");

            DbContext.EstoqueHistorico.Add(new EstoqueHistorico
            {
                MaterialId = ent.materialId,
                Quantidade = ent.quantity,
                Data = DateTime.UtcNow,
                TarefaId = task.Id,
                Tarefa = task,
                Tipo = "S",
                UsuarioId = Operator.Id
            });

            DbContext.TarefaMateriais.Add(new TarefaMateriais
            {
                Tarefa = task,
                TarefaId = task.Id,
                MaterialId = ent.materialId,
                Quantidade = ent.quantity
            });

            material.QuantidadeAtual -= ent.quantity;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete, ActionName("material")]
        public async Task<IActionResult> RemoveMaterial([FromBody] ChangeMaterial ent)
        {
            var material = await DbContext.Material
                .SingleOrDefaultAsync(e => e.Id == ent.materialId);

            if (material == null)
                return NotFound();

            var task = await DbContext.Tarefa
                .Where(t => t.Destino == ent.userId && t.FinalizadoEm == null)
                .OrderByDescending(t => t.IniciadoEm)
                .SingleOrDefaultAsync();

            if (task == null)
                throw new ValidationExn("Usuário sem tarefas abertas.");

            DbContext.EstoqueHistorico.Add(new EstoqueHistorico
            {
                MaterialId = ent.materialId,
                Quantidade = ent.quantity,
                Data = DateTime.UtcNow,
                Tarefa = task,
                TarefaId = task.Id,
                Tipo = "E",
                UsuarioId = Operator.Id,
            });

            material.QuantidadeAtual += ent.quantity;

            DbContext.Entry(material).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost, ActionName("equipment")]
        public async Task<IActionResult> AddEquipment([FromBody] ChangeEquipment ent)
        {
            var equip = await DbContext.Equipamento
                .SingleOrDefaultAsync(e => e.Id == ent.equipmentId);

            if (equip == null)
                return NotFound();

            if (equip.EmUso == true)
                throw new ValidationExn("Equipamento já está em uso.");

            DbContext.TarefaEquipamentos.Add(new TarefaEquipamentos
            {
                EquipamentoId = ent.equipmentId
            });

            equip.EmUso = true;

            DbContext.Entry(equip).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost, ActionName("equipment")]
        public async Task<IActionResult> RemoveEquipment([FromBody] ChangeEquipment ent)
        {
            var equip = await DbContext.Equipamento
                .SingleOrDefaultAsync(e => e.Id == ent.equipmentId);

            if (equip == null)
                return NotFound();

            equip.EmUso = false;

            DbContext.Entry(equip).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Add([FromBody] TaskDTO ent) => _Add(ent);
    }
}
