using APICore.Helpers.WebApi;
using APICore.Model;
using APICore.Model.Selection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class GroupController : EntityController<Grupo, Group, int>
    {
        public GroupController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Grupo, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<Group, int>> GetExternalId => ent => ent.id;

        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);
        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);

        public override IQueryable<Grupo> MapSource(IQueryable<Grupo> query) => query.Include(g => g.UsuarioGrupos);

        public override Task<Grupo> BeforeAdd(Group sendedData)
        {
            var grupo = new Grupo
            {
                Id = sendedData.id,
                Descricao = sendedData.name
            };

            return grupo.AsTask();
        }

        public override Task BeforeDelete(Grupo data)
        {
            if (data.UsuarioGrupos.Any())
                throw new ValidationExn("Este grupo possui usuários pertencentes a ele.");

            return Task.CompletedTask;
        }

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Add([FromBody] Group ent) => _Add(ent);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Delete(int? id) => _Delete(id);
    }
}
