using APICore.Controllers;
using APICore.Helpers.Misc;
using APICore.Model;
using APICore.Model.Auth;
using APICore.Model.Selection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class UserController : EntityController<Usuario, User, int>
    {
        public UserController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper) { }

        public override Expression<Func<Usuario, int>> GetInternalId => ent => ent.Id;
        public override Expression<Func<User, int>> GetExternalId => ent => ent.id;

        public override Task Validate(User data, ActionOperation operation)
        {
            if(operation is ActionOperation.Add && data.password.IsEmpty())
            {
                throw new ValidationExn("Senha obrigatória.");
            }

            return Task.CompletedTask;
        }

        public override async Task<Usuario> BeforeAdd(User sendedData)
        {
            if (Operator.Type is UserType.Supervisor && (sendedData.type is UserType.Admin || sendedData.type is UserType.Supervisor))
                throw new UnauthorizedExn("Usuário não autorizado para esta operação.");

            var usuario = await base.BeforeAdd(sendedData);
            usuario.Tipo = (int) sendedData.type;

            var salt = Auth.GetSalt();
            var pass = Auth.GetPassword(sendedData.password, salt);

            usuario.Senha = pass;
            usuario.Chave = salt;

            return usuario;
        }

        public override async Task<Usuario> BeforeUpdate(Usuario oldData, User changedData)
        {
            var newData = await base.BeforeUpdate(oldData, changedData);
            newData.Tipo = (int)changedData.type;

            if (!changedData.password.IsEmpty())
            {
                var salt = Auth.GetSalt();
                var pass = Auth.GetPassword(changedData.password, salt);

                newData.Senha = pass;
                newData.Chave = salt;
            }

            return newData;
        }

        public override Task<IActionResult> GetByKey(int? id) => _GetByKey(id);
        public override Task<IActionResult> GetAll([FromBody] APIQuery query) => _GetAll(query);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Add([FromBody] User ent) => _Add(ent);

        [Authorize(Roles = "Admin,Supervisor")]
        public override Task<IActionResult> Update([FromBody] User ent) => _Update(ent);
    }
}