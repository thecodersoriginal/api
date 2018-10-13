using APICore.Helpers.WebApi;
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
    [Authorize]
    public class TaskController : EntityController<Tarefa, TaskDTO, int>
    {

        public class User
        {
            int id;
        }

        public TaskController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Tarefa, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<TaskDTO, int>> GetExternalId => ent => ent.id;

        public override async Task<Tarefa> BeforeAdd(TaskDTO sendedData)
        {
            var equip = await base.BeforeAdd(sendedData);

            return equip;
        }

        public override Task<Tarefa> BeforeUpdate(Tarefa oldData, TaskDTO changedData)
        {
            return oldData.AsTask();
        }

        [HttpPost]
        public async Task<IActionResult> Start([FromBody] TaskDTO task)
        {

            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);

            if (tar == null)
                return NotFound();

            DbContext.Entry(tar).State = EntityState.Modified;

            tar.IniciadoEm = DateTime.UtcNow;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> AddMaterial([FromBody] TaskDTO task, TaskMaterial material)
        {

            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);
            var mat = await DbContext.Material.SingleOrDefaultAsync(e => e.Id == material.id);

            if (tar == null || mat == null)
                return NotFound();

            if(mat.QuantidadeAtual - material.quantity < 0)
            {
                throw new ValidationExn("Estoque insuficiente.");
            }

            var usu = await DbContext.Usuario.SingleOrDefaultAsync(u => u.Id == tar.Destino);

            if (usu == null)
                return NotFound();

            //criar historico de estoque - remoção
            EstoqueHistorico movimentacao = new EstoqueHistorico
            {
                Material = mat,
                MaterialId = mat.Id,
                Quantidade = material.quantity,
                Data = DateTime.UtcNow,
                Tarefa = tar,
                TarefaId = tar.Id,
                Tipo = "S", //saída
                Usuario = usu,
                UsuarioId = usu.Id,
            };

            TarefaMateriais materialTarefa = new TarefaMateriais
            {
               MaterialId = material.id,
               Quantidade = material.quantity
            };

            DbContext.Entry(tar).State = EntityState.Modified;
            DbContext.Entry(mat).State = EntityState.Modified;

            DbContext.EstoqueHistorico.Add(movimentacao);

            tar.TarefaMateriais.Add(materialTarefa);
            mat.QuantidadeAtual -= material.quantity;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMaterial([FromBody] TaskDTO task, TaskMaterial material) //devolução de materiais
        {

            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);
            var mat = await DbContext.Material.SingleOrDefaultAsync(e => e.Id == material.id);

            if (tar == null || mat == null)
                return NotFound();

            var usu = await DbContext.Usuario.SingleOrDefaultAsync(u => u.Id == tar.Destino);

            if (usu == null)
                return NotFound();

            //criar historico de estoque - adição
            EstoqueHistorico movimentacao = new EstoqueHistorico
            {
                Material = mat,
                MaterialId = mat.Id,
                Quantidade = material.quantity,
                Data = DateTime.UtcNow,
                Tarefa = tar,
                TarefaId = tar.Id,
                Tipo = "E", //entrada
                Usuario = usu,
                UsuarioId = usu.Id,
            };

            DbContext.Entry(mat).State = EntityState.Modified;

            DbContext.EstoqueHistorico.Add(movimentacao);

            mat.QuantidadeAtual += material.quantity;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> AddEquipment([FromBody] TaskDTO task, TaskEquipment equipment)
        {
            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);
            var equi = await DbContext.Equipamento.SingleOrDefaultAsync(e => e.Id == equipment.id);

            if (tar == null || equi == null)
                return NotFound();
            
            TarefaEquipamentos materialEquipamentos = new TarefaEquipamentos
            {
                EquipamentoId = equipment.id
            };

            if(equi.EmUso == true)
            {
                throw new ValidationExn("Equipamento já está em uso.");
            }

            DbContext.Entry(tar).State = EntityState.Modified;
            DbContext.Entry(equi).State = EntityState.Modified;

            tar.TarefaEquipamentos.Add(materialEquipamentos);
            equi.EmUso = true;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEquipment([FromBody] TaskDTO task, TaskEquipment equipment)//devolução de equipamento
        {
            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);
            var equi = await DbContext.Equipamento.SingleOrDefaultAsync(e => e.Id == equipment.id);

            if (tar == null || equi == null)
                return NotFound();
            
            DbContext.Entry(equi).State = EntityState.Modified;

            equi.EmUso = false;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> Finish([FromBody] TaskDTO task)
        {

            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);

            if (tar == null)
                return NotFound();

            DbContext.Entry(tar).State = EntityState.Modified;

            tar.FinalizadoEm = DateTime.UtcNow;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TaskDTO task, User user)
        {
            var tar = await InitialQuery.SingleOrDefaultAsync(t => t.Id == task.id);

            if (tar == null)
                return NotFound();

            DbContext.Entry(tar).State = EntityState.Modified;

            tar.InterrompidoEm = DateTime.UtcNow;

            DbContext.Tarefa.Add(new Tarefa
            {
                AgendadaEm = tar.AgendadaEm,
                IniciadoEm = tar.IniciadoEm,
                FinalizadoEm = tar.FinalizadoEm,
                Origem = tar.Origem,
                Destino = tar.Destino,
                TarefaEquipamentos = tar.TarefaEquipamentos,
                TarefaMateriais = tar.TarefaMateriais,
                Nome = tar.Nome,
                RepetirEm = tar.RepetirEm
            });

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Add([FromBody] TaskDTO ent) => _Add(ent);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Update([FromBody] TaskDTO ent) => _Update(ent);
    }
}
