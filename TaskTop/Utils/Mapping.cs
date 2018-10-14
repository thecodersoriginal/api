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
                .MapMember(dto => dto.type, us => us.Tipo)
                .MapMember(dto => dto.groups, us => us.UsuarioGrupos)
                .IgnoreMember(dto => dto.password)
                .ReverseMap()
                .IgnoreMember(us => us.UsuarioGrupos);

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

            cnfg.CreateMap<Alerta, Alert>()
                .MapMember(dto => dto.message, a => a.Mensagem)
                .MapMember(dto => dto.readAt, a => a.VisualizadaEm)
                .MapMember(dto => dto.receiverId, a => a.Destino)
                .MapMember(dto => dto.receiverName, a => a.DestinoNavigation.Nome)
                .MapMember(dto => dto.senderId, a => a.Origem)
                .MapMember(dto => dto.senderName, a => a.OrigemNavigation.Nome)
                .ReverseMap();

            cnfg.CreateMap<Equipamento, Equipment>()
                .MapMember(dto => dto.description, e => e.Descricao)
                .MapMember(dto => dto.inUse, e => e.EmUso)
                .MapMember(dto => dto.code, e => e.Codigo)
                .MapMember(dto => dto.active, e => e.Ativo)
                .ReverseMap();

            cnfg.CreateMap<Material, MaterialDTO>()
                .MapMember(dto => dto.description, m => m.Descricao)
                .MapMember(dto => dto.actualQuantity, m => m.QuantidadeAtual)
                .ReverseMap();

            cnfg.CreateMap<Tarefa, TaskDTO>()
                .MapMember(dto => dto.name, t => t.Nome)
                .MapMember(dto => dto.senderId, t => t.Origem)
                .MapMember(dto => dto.senderName, t => t.OrigemNavigation.Nome)
                .MapMember(dto => dto.receiverId, t => t.Destino)
                .MapMember(dto => dto.receiverName, t => t.DestinoNavigation.Nome)
                .MapMember(dto => dto.scheduledAt, t => t.AgendadaEm)
                .MapMember(dto => dto.startedAt, t => t.IniciadoEm)
                .MapMember(dto => dto.finishedAt, t => t.FinalizadoEm)
                .MapMember(dto => dto.interruptedAt, t => t.InterrompidoEm)
                .MapMember(dto => dto.repeatInDays, t => t.RepetirEm)
                .MapMember(dto => dto.equipments, t=> t.TarefaEquipamentos)
                .MapMember(dto => dto.materials, t => t.TarefaMateriais)
                .MapMember(dto => dto.rateId, t => t.AvaliacaoId)
                .ReverseMap()
                .IgnoreMember(t => t.OrigemNavigation)
                .IgnoreMember(t => t.DestinoNavigation);

            cnfg.CreateMap<TarefaEquipamentos, TaskEquipment>()
                .MapMember(dto=> dto.id, e=> e.EquipamentoId)
                .MapMember(dto => dto.description, e => e.Equipamento.Descricao)
                .ReverseMap();

            cnfg.CreateMap<TarefaMateriais, TaskMaterial>()
                .MapMember(dto => dto.id, m => m.MaterialId)
                .MapMember(dto => dto.quantity, m => m.Quantidade)
                .MapMember(dto => dto.description, m=> m.Material.Descricao)
                .ReverseMap();
        };
    }
}
