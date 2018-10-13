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
    public class AlertController : EntityController<Alerta, Alert, int>
    {
        public AlertController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Alerta, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<Alert, int>> GetExternalId => ent => ent.id;

        public override IQueryable<Alerta> FilterQuery(IQueryable<Alerta> query)
        {
            switch(Operator.Type)
            {
                case UserType.Funcionario:
                case UserType.Supervisor:
                case UserType.Estoque:
                    return query.Where(a => (a.Origem == Operator.Id) || (a.Destino == Operator.Id));
                default:
                    return query;
            }
        }

        public override async Task<Alerta> BeforeAdd(Alert sendedData)
        {
            var alerta = await base.BeforeAdd(sendedData);
            alerta.VisualizadaEm = null;

            return alerta;
        }

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);
        public override Task<IActionResult> Add([FromBody] Alert ent) => _Add(ent);

        [HttpPost]
        public async Task<IActionResult> Read(int? id)
        {
            var alert = await InitialQuery.SingleOrDefaultAsync(a => a.Id == id);

            if (alert == null)
                return NotFound();

            alert.VisualizadaEm = DateTime.UtcNow;

            DbContext.Entry(alert).State = EntityState.Modified;

            await DbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}

