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
using TaskTop.Authorization.Model;
using TaskTop.DTO;
using TaskTop.Model;
using TaskTop.Utils;

namespace TaskTop.Controllers
{
    [Authorize]
    public class MaterialController : EntityController<Material, MaterialDTO, int>
    {
        public class Quantity
        {
            public int quantity;
        }

        public MaterialController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Material, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<MaterialDTO, int>> GetExternalId => ent => ent.id;

        public override Task<Material> BeforeUpdate(Material oldData, MaterialDTO changedData)
        {
            oldData.QuantidadeAtual = changedData.actualQuantity;
            return oldData.AsTask();
        }

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);

        [Authorize(Roles = "Admin,Estoque")]
        public override Task<IActionResult> Add([FromBody] MaterialDTO ent) => _Add(ent);

        [HttpPost]
        public async Task<IActionResult> AddStock([FromBody] Quantity quantity, int id)
        {
            var material = await InitialQuery.SingleOrDefaultAsync(m => m.Id == id);

            if (material == null)
                return NotFound();
            
            DbContext.Entry(material).State = EntityState.Modified;

            material.QuantidadeAtual += quantity.quantity;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveStock([FromBody] Quantity quantity, int id)
        {
            var material = await InitialQuery.SingleOrDefaultAsync(m => m.Id == id);

            if (material == null)
                return NotFound();
            
            var matTarefa = await DbContext.TarefaMateriais.SingleOrDefaultAsync(t => t.MaterialId == id);

            if (material.QuantidadeAtual - quantity.quantity < 0)
            {
                throw new ValidationExn("Estoque insuficiente.");
            }
            
            DbContext.Entry(material).State = EntityState.Modified;

            material.QuantidadeAtual -= quantity.quantity;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [Authorize(Roles = "Admin,Estoque")]
        public override Task<IActionResult> Update([FromBody] MaterialDTO ent) => _Update(ent);
    }
}
