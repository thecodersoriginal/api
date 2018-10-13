using APICore.Helpers.AutoMapper;
using AutoMapper;
using System;
using TaskTop.Authorization.Model;
using TaskTop.DTO;
using TaskTop.Model;

namespace TaskTop.Utils
{
    public static class Mapping
    {
        public static Action<IMapperConfigurationExpression> Initialize = cnfg =>
        {
            cnfg.CreateMap<Usuario, User>()
                .MapMember(dto => dto.name, us => us.Nome)
                .MapMember(dto => dto.phone, us => us.Telefone)
                .MapMember(dto => dto.type, us => (UserType) us.Tipo)
                .MapMember(dto => dto.groups, us => us.UsuarioGrupos)
                .IgnoreMember(dto => dto.password)
                .ReverseMap();

            cnfg.CreateMap<UsuarioGrupos, UserGroup>()
                .MapMember(dto => dto.id, ug => ug.GrupoId)
                .MapMember(dto => dto.name, ug => ug.Grupo.Descricao)
                .ReverseMap();

            cnfg.CreateMap<Grupo, Group>()
                .MapMember(dto => dto.name, g => g.Descricao)
                .MapMember(dto => dto.users, g => g.UsuarioGrupos)
                .ReverseMap();

            cnfg.CreateMap<UsuarioGrupos, GroupUser>()
                .MapMember(dto => dto.id, gu => gu.UsuarioId)
                .MapMember(dto => dto.name, gu => gu.Usuario.Nome)
                .ReverseMap();
        };
    }
}
