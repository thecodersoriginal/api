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
    public class EquipmentController : EntityController<Equipamento, Equipment, int>
    {

        public EquipmentController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Equipamento, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<Equipment, int>> GetExternalId => ent => ent.id;

        public override async Task<Equipamento> BeforeAdd(Equipment sendedData)
        {
            var equip = await base.BeforeAdd(sendedData);
            equip.Ativo = true;

            return equip;
        }

        public override Task<Equipamento> BeforeUpdate(Equipamento oldData, Equipment changedData)
        {
            oldData.Ativo = changedData.active;
            oldData.EmUso = changedData.inUse;
            return oldData.AsTask();
        }

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);

        [Authorize(Roles = "Admin,Estoque")]
        public override Task<IActionResult> Add([FromBody] Equipment ent) => _Add(ent);

        [Authorize(Roles = "Admin,Estoque")]
        public override Task<IActionResult> Update([FromBody] Equipment ent) => _Update(ent);
    }
}
