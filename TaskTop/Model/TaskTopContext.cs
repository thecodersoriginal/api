using Microsoft.EntityFrameworkCore;

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
        public virtual DbSet<SubTarefa> SubTarefa { get; set; }
        public virtual DbSet<SubTarefaEquipamentos> SubTarefaEquipamentos { get; set; }
        public virtual DbSet<SubTarefaMateriais> SubTarefaMateriais { get; set; }
        public virtual DbSet<Tarefa> Tarefa { get; set; }
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<SubTarefa>(entity =>
            {
                entity.Property(e => e.FinalizadoEm).HasColumnType("datetime");

                entity.Property(e => e.IniciadoEm).HasColumnType("datetime");

                entity.HasOne(d => d.Tarefa)
                    .WithMany(p => p.SubTarefa)
                    .HasForeignKey(d => d.TarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefa_Tarefa");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.SubTarefa)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefa_Usuario");
            });

            modelBuilder.Entity<SubTarefaEquipamentos>(entity =>
            {
                entity.HasKey(e => new { e.SubTarefaId, e.EquipamentoId });

                entity.HasOne(d => d.Equipamento)
                    .WithMany(p => p.SubTarefaEquipamentos)
                    .HasForeignKey(d => d.EquipamentoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefaEquipamentos_Equipamento");

                entity.HasOne(d => d.SubTarefa)
                    .WithMany(p => p.SubTarefaEquipamentos)
                    .HasForeignKey(d => d.SubTarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefaEquipamentos_SubTarefa");
            });

            modelBuilder.Entity<SubTarefaMateriais>(entity =>
            {
                entity.HasKey(e => new { e.SubTarefaId, e.MaterialId });

                entity.HasOne(d => d.Material)
                    .WithMany(p => p.SubTarefaMateriais)
                    .HasForeignKey(d => d.MaterialId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefaMateriais_Material");

                entity.HasOne(d => d.SubTarefa)
                    .WithMany(p => p.SubTarefaMateriais)
                    .HasForeignKey(d => d.SubTarefaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubTarefaMateriais_SubTarefa");
            });

            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.Property(e => e.AgendadaEm).HasColumnType("datetime");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(45);

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Tarefa)
                    .HasForeignKey(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tarefa_Usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
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
