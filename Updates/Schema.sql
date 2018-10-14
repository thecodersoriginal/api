USE [TaskTop]

CREATE TABLE [dbo].[Usuario]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Login] NVARCHAR (16) NOT NULL,
	[Nome] NVARCHAR (120) NOT NULL,
	[Telefone] NVARCHAR (20) NOT NULL,
	[Email] NVARCHAR (45) NOT NULL,
	[Senha] VARBINARY (MAX) NOT NULL,
	[Chave] VARBINARY (MAX) NOT NULL,
	[Tipo] INT NOT NULL,
	CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_UsuarioLogin]
    ON [dbo].[Usuario]([Login] ASC);
GO

CREATE TABLE [dbo].[Grupo]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Descricao] NVARCHAR (45) NOT NULL,
	CONSTRAINT [PK_Grupo] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Equipamento]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Codigo] NVARCHAR (45) NOT NULL,
	[Descricao] NVARCHAR (45) NOT NULL,
	[EmUso] BIT NOT NULL,
	[Ativo] BIT NOT NULL,
	CONSTRAINT [PK_Equipamento] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Material]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Descricao] NVARCHAR (45) NOT NULL,
	[QuantidadeAtual] INT NOT NULL,
	CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE TABLE [dbo].[Tarefa]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Nome] NVARCHAR (45) NOT NULL,
	[AgendadaEm] DATETIME NOT NULL,
	[IniciadoEm] DATETIME,
	[FinalizadoEm] DATETIME,
	[InterrompidoEm] DATETIME,
	[Origem] INT NOT NULL,
	[Destino] INT NOT NULL,
	[RepetirEm] INT NULL,
	CONSTRAINT [PK_Tarefa] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Origem_Tarefa] FOREIGN KEY ([Origem]) REFERENCES [dbo].[Usuario] ([Id]),
	CONSTRAINT [FK_Destino_Tarefa] FOREIGN KEY ([Destino]) REFERENCES [dbo].[Usuario] ([Id])
);
GO

CREATE TABLE [dbo].[EstoqueHistorico]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Quantidade] INT NOT NULL,
	[Tipo] CHAR(1) NOT NULL,
	[UsuarioId] INT NOT NULL,
	[MaterialId] INT NOT NULL,
	[TarefaId] INT NULL,
	[Data] DATETIME NOT NULL,
	CONSTRAINT [PK_EstoqueHistorico] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_EstoqueHistorico_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id]),
	CONSTRAINT [FK_EstoqueHistorico_Tarefa] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Material] ([Id]),
	CONSTRAINT [FK_EstoqueHistorico_Material] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[Tarefa] ([Id])
);
GO

CREATE TABLE [dbo].[TarefaMateriais]
(
	[TarefaId] INT NOT NULL,
	[MaterialId] INT NOT NULL,
	[Quantidade] INT NOT NULL,
	CONSTRAINT [PK_TarefaMateriais] PRIMARY KEY CLUSTERED ([TarefaId] ASC, [MaterialId] ASC),
	CONSTRAINT [FK_TarefaMateriais_Material] FOREIGN KEY ([MaterialId]) REFERENCES [dbo].[Material] ([Id]),
	CONSTRAINT [FK_TarefaMateriais_Tarefa] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[Tarefa] ([Id])
);
GO

CREATE TABLE [dbo].[TarefaEquipamentos]
(
	[TarefaId] INT NOT NULL,
	[EquipamentoId] INT NOT NULL,
	CONSTRAINT [PK_TarefaEquipamentos] PRIMARY KEY CLUSTERED ([TarefaId] ASC, [EquipamentoId] ASC),
	CONSTRAINT [FK_TarefaEquipamentos_Equipamento] FOREIGN KEY ([EquipamentoId]) REFERENCES [dbo].[Equipamento] ([Id]),
	CONSTRAINT [FK_TarefaEquipamentos_Tarefa] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[Tarefa] ([Id])
);
GO

CREATE TABLE [dbo].[TarefaAvaliacao]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[TarefaId] INT NOT NULL,
	[Nota] INT NOT NULL,
	[NotaMaxima] INT NOT NULL,
	CONSTRAINT [PK_TarefaAvaliacao] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Tarefa_TarefaAvaliacao] FOREIGN KEY ([TarefaId]) REFERENCES [dbo].[Tarefa] ([Id])
);
GO

CREATE TABLE [dbo].[UsuarioGrupos]
(
	[UsuarioId] INT NOT NULL,
	[GrupoId] INT NOT NULL,
	CONSTRAINT [PK_UsuarioGrupos] PRIMARY KEY CLUSTERED ([UsuarioId] ASC, [GrupoId] ASC),
	CONSTRAINT [FK_UsuarioGrupos_Grupo] FOREIGN KEY ([GrupoId]) REFERENCES [dbo].[Grupo] ([Id]),
	CONSTRAINT [FK_UsuarioGrupos_Usuario] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
);
GO

CREATE TABLE [dbo].[Alerta]
(
	[Id] INT IDENTITY (1, 1) NOT NULL,
	[Mensagem] NVARCHAR (300) NOT NULL,
	[Origem] INT NOT NULL,
	[Destino] INT NOT NULL,
	[VisualizadaEm] DATETIME NULL,
	CONSTRAINT [PK_Alerta] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Origem_Usuario] FOREIGN KEY ([Origem]) REFERENCES [dbo].[Usuario] ([Id]),
	CONSTRAINT [FK_Destino_Usuario] FOREIGN KEY ([Destino]) REFERENCES [dbo].[Usuario] ([Id])
);
GO

/*
** DATA **
*/
SET IDENTITY_INSERT [Usuario] ON
INSERT [Usuario] ([Id], [Login], [Nome], [Telefone], [Email], [Tipo], [Senha], [Chave])
VALUES (1, N'admin', N'Super Admin', N'18997528931', N'thecodersoriginal@gmail.com', 1, 0x57D16446A9A64D71C41FC5129DA45555E8BA34D3DDFC9972BFF32D2FDE905EA5, 0x19F1481FC59F5CF1EF83BF41337DD90B9D981B6E6477ED26F3397E558F450585)
SET IDENTITY_INSERT [Usuario] OFF