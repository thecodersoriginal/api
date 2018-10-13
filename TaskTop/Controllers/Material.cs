using APICore.Helpers.WebApi;
using APICore.Model;
using APICore.Model.Selection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class MaterialController : EntityController<Material, MaterialDTO, int>
    {
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

        [Authorize(Roles = "Admin,Estoque")]
        public override Task<IActionResult> Update([FromBody] MaterialDTO ent) => _Update(ent);
    }
}
