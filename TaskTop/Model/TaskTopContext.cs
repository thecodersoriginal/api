using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TaskTop.Model
{
    public partial class TaskTopContext : DbContext
    {
        public TaskTopContext()
        {
        }

        public TaskTopContext(DbContextOptions<TaskTopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alerta> Alerta { get; set; }
        public virtual DbSet<Equipamento> Equipamento { get; set; }
        public virtual DbSet<EstoqueHistorico> EstoqueHistorico { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Tarefa> Tarefa { get; set; }
        public virtual DbSet<TarefaAvaliacao> TarefaAvaliacao { get; set; }
        public virtual DbSet<TarefaEquipamentos> TarefaEquipamentos { get; set; }
        public virtual DbSet<TarefaMateriais> TarefaMateriais { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioGrupos> UsuarioGrupos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alerta>(entity =>
            {
                entity.Property(e => e.Mensagem)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.VisualizadaEm).HasColumnType("datetime");

                entity.HasOne(d => d.DestinoNavigation)
                    .WithMany(p => p.AlertaDestinoNavigation)
                    .HasForeignKey(d => d.Destino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Destino_Usuario");

                entity.HasOne(d => d.OrigemNavigation)
                    .WithMany(p => p.AlertaOrigemNavigation)
                    .HasForeignKey(d => d.Origem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Origem_Usuario");
            });

            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.Property(e => e.Codigo)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<EstoqueHistorico>(entity =>
            {
                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.EstoqueHistorico)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstoqueHistorico_Tarefa");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.EstoqueHistorico)
                    .HasForeignKey(d => d.TarefaId)
                    .HasConstraintName("FK_EstoqueHistorico_Material");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.EstoqueHistorico)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EstoqueHistorico_Usuario");
            });

            modelBuilder.Entity<Grupo>(entity =>
            {
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.Property(e => e.Descricao)
                    .IsRequired()
                    .HasMaxLength(45);
            });

            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.Property(e => e.AgendadaEm).HasColumnType("datetime");

                entity.Property(e => e.FinalizadoEm).HasColumnType("datetime");

                entity.Property(e => e.IniciadoEm).HasColumnType("datetime");

                entity.Property(e => e.InterrompidoEm).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.HasOne(d => d.DestinoNavigation)
                    .WithMany(p => p.TarefaDestinoNavigation)
                    .HasForeignKey(d => d.Destino)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Destino_Tarefa");

                entity.HasOne(d => d.OrigemNavigation)
                    .WithMany(p => p.TarefaOrigemNavigation)
                    .HasForeignKey(d => d.Origem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Origem_Tarefa");
            });

            modelBuilder.Entity<TarefaAvaliacao>(entity =>
            {
                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TarefaAvaliacao)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tarefa_TarefaAvaliacao");
            });

            modelBuilder.Entity<TarefaEquipamentos>(entity =>
            {
                entity.HasKey(e => new { e.TarefaId, e.EquipamentoId });

                entity.HasOne(d => d.Equipamento)
                    .WithMany(p => p.TarefaEquipamentos)
                    .HasForeignKey(d => d.EquipamentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TarefaEquipamentos_Equipamento");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TarefaEquipamentos)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TarefaEquipamentos_Tarefa");
            });

            modelBuilder.Entity<TarefaMateriais>(entity =>
            {
                entity.HasKey(e => new { e.TarefaId, e.MaterialId });

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.TarefaMateriais)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TarefaMateriais_Material");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.TarefaMateriais)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TarefaMateriais_Tarefa");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("IX_UsuarioLogin")
                    .IsUnique();

                entity.Property(e => e.Chave).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(e => e.Tipo)
                    .IsRequired();
                    
                entity.Property(e => e.Senha).IsRequired();

                entity.Property(e => e.Telefone)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<UsuarioGrupos>(entity =>
            {
                entity.HasKey(e => new { e.UsuarioId, e.GrupoId });

                entity.HasOne(d => d.Grupo)
                    .WithMany(p => p.UsuarioGrupos)
                    .HasForeignKey(d => d.GrupoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioGrupos_Grupo");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioGrupos)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioGrupos_Usuario");
            });
        }
    }
}
