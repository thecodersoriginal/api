using APICore.Controllers;
using APICore.Model;
using APICore.Model.Selection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskTop.Authorization;
using TaskTop.Authorization.Model;
using TaskTop.DTO;
using TaskTop.Model;

namespace TaskTop.Controllers
{
    public class StockChange
    {
        public int quantity { get; set; }
        public int materialId { get; set; }
        public int? taskId { get; set; }
    }

    [Authorize]
    public class StockController : Controller
    {
        private readonly TaskTopContext DbContext;
        private readonly IMapper EntMapper;
        private Operator Operator
        {
            get { return AuthExts.ToOperador(User.Identity); }
        }

        public StockController(TaskTopContext db, IMapper mapper)
        {
            EntMapper = mapper;
            DbContext = db;
        }

        [HttpPost, ActionName("all")]
        public async Task<IActionResult> GetAll([FromBody] APIQuery query)
        {
            var source = DbContext.EstoqueHistorico.AsQueryable();
            var pager = new Pager
            {
                page = query.page,
                perPage = query.perPage,
                usePager = query.usePager
            };

            var selector = new Selector<EstoqueHistorico, StockHistory>(EntMapper.ConfigurationProvider, source, query.fields);
            var items = await selector
                .Filter(query.filters)
                .Ordering(query.sort, eh => eh.Id)
                .Paginate(pager)
                .ToPagedRecordsAsync(pager);

            return Ok(items);
        }

        [HttpPost, ActionName("add")]
        public async Task<IActionResult> AddStock([FromBody] StockChange ent)
        {
            var material = await DbContext.Material
                .SingleOrDefaultAsync(m => m.Id == ent.materialId);

            if (material == null)
                return NotFound();

            material.EstoqueHistorico.Add(new EstoqueHistorico
            {
                Quantidade = ent.quantity,
                Tipo = "E",
                UsuarioId = Operator.Id,
                MaterialId = ent.materialId,
                Data = DateTime.UtcNow
            });

            material.QuantidadeAtual += ent.quantity;

            DbContext.Entry(material).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [AcceptVerbs("POST", "DELETE"), ActionName("remove")]
        public async Task<IActionResult> RemoveStock([FromBody] StockChange ent)
        {
            if (ent.taskId == null)
                throw new ValidationExn("Informe o código da tarefa.");

            var material = await DbContext.Material
                .SingleOrDefaultAsync(m => m.Id == ent.materialId);

            if (material == null)
                return NotFound();

            if (material.QuantidadeAtual - ent.quantity < 0)
                throw new ValidationExn("Estoque insuficiente.");

            material.EstoqueHistorico.Add(new EstoqueHistorico
            {
                Quantidade = ent.quantity,
                Tipo = "S",
                UsuarioId = Operator.Id,
                MaterialId = ent.materialId,
                TarefaId = ent.taskId,
                Data = DateTime.UtcNow
            });

            material.QuantidadeAtual -= ent.quantity;

            DbContext.Entry(material).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}