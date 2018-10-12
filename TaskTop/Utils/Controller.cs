using APICore.Controllers;
using AutoMapper;
using TaskTop.Authorization;
using TaskTop.Authorization.Model;
using TaskTop.Model;

namespace TaskTop.Utils
{
    public abstract class EntityController<DataModel, DTO, PK> : EntityController<TaskTopContext, Operator, DataModel, DTO, PK>
        where DataModel : class
        where DTO : class
        where PK : struct
    {
        public EntityController(TaskTopContext ctx, IMapper mapper) : base(ctx, mapper, AuthController.ToOperador) { }
    }
}
