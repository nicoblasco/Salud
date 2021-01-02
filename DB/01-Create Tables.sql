USE [AyudaSocial]
GO

/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Audits]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Audits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[WindowId] [int] NOT NULL,
	[Accion] [nvarchar](max) NULL,
	[Fecha] [datetime] NOT NULL,
	[Clave] [nvarchar](max) NULL,
	[Entidad] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Audits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Countries]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Predeterminado] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DeliveryStates]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeliveryStates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.DeliveryStates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Modules]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Modules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_dbo.Modules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Permissions]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WindowId] [int] NOT NULL,
	[RolId] [int] NOT NULL,
	[Consulta] [bit] NOT NULL,
	[AltaModificacion] [bit] NOT NULL,
	[Baja] [bit] NOT NULL,
	[Fecha] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Personas]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Personas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Dni] [nvarchar](max) NULL,
	[Cuil] [nvarchar](max) NULL,
	[Apellido] [nvarchar](max) NULL,
	[Telefono] [nvarchar](max) NULL,
	[FechaNacimiento] [datetime] NULL,
	[Manzana] [nvarchar](max) NULL,
	[Origen] [nvarchar](max) NULL,
	[GrupoFamiliar] [nvarchar](max) NULL,
	[Detalle] [nvarchar](max) NULL,
	[EstadoAnses] [nvarchar](max) NULL,
	[ComentarioAnses] [nvarchar](max) NULL,
	[FechaVerificacionAnses] [datetime] NULL,
	[FechaSolicitud] [datetime] NULL,
	[TipoIngreso] [nvarchar](max) NULL,
	[MotivoGestion] [nvarchar](max) NULL,
	[FechaUltimaEntrega] [datetime] NULL,
	[Calle] [nvarchar](max) NULL,
	[Altura] [nvarchar](max) NULL,
	[EntreCalle1] [nvarchar](max) NULL,
	[EntreCalle2] [nvarchar](max) NULL,
	[Piso] [nvarchar](max) NULL,
	[Depto] [nvarchar](max) NULL,
	[Barrio] [nvarchar](max) NULL,
	[Latitud] [nvarchar](max) NULL,
	[Longitud] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Personas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Rols]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rols](
	[RolId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[IsAdmin] [bit] NOT NULL,
	[WindowId] [int] NULL,
 CONSTRAINT [PK_dbo.Rols] PRIMARY KEY CLUSTERED 
(
	[RolId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Settings]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Clave] [nvarchar](max) NULL,
	[Texto1] [nvarchar](max) NULL,
	[Texto2] [nvarchar](max) NULL,
	[Numero1] [int] NULL,
	[Numero2] [int] NULL,
	[Logico1] [bit] NULL,
	[Logico2] [bit] NULL,
	[Fecha1] [datetime] NULL,
	[Fecha2] [datetime] NULL,
 CONSTRAINT [PK_dbo.Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Streets]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Streets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Baja] [nvarchar](max) NULL,
	[DescripcionOficial] [nvarchar](max) NULL,
	[Codigo] [nvarchar](max) NULL,
	[DescripcionGoogle] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Streets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Usuarios]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Usuarios](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NULL,
	[Apellido] [nvarchar](max) NULL,
	[Nombreusuario] [nvarchar](max) NULL,
	[Contraseña] [nvarchar](max) NULL,
	[Enable] [bit] NOT NULL,
	[RolId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Usuarios] PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Visits]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Visits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PersonaId] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[Detalle] [nvarchar](max) NULL,
	[DeliveryStateId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Visits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Windows]    Script Date: 08/04/2020 22:05:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Windows](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Enable] [bit] NOT NULL,
	[Url] [nvarchar](max) NULL,
	[Orden] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Windows] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Audits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Audits_dbo.Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([UsuarioId])
GO

ALTER TABLE [dbo].[Audits] CHECK CONSTRAINT [FK_dbo.Audits_dbo.Usuarios_UsuarioId]
GO

ALTER TABLE [dbo].[Audits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Audits_dbo.Windows_WindowId] FOREIGN KEY([WindowId])
REFERENCES [dbo].[Windows] ([Id])
GO

ALTER TABLE [dbo].[Audits] CHECK CONSTRAINT [FK_dbo.Audits_dbo.Windows_WindowId]
GO

ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Permissions_dbo.Rols_RolId] FOREIGN KEY([RolId])
REFERENCES [dbo].[Rols] ([RolId])
GO

ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_dbo.Permissions_dbo.Rols_RolId]
GO

ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Permissions_dbo.Windows_WindowId] FOREIGN KEY([WindowId])
REFERENCES [dbo].[Windows] ([Id])
GO

ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_dbo.Permissions_dbo.Windows_WindowId]
GO

ALTER TABLE [dbo].[Rols]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Rols_dbo.Windows_WindowId] FOREIGN KEY([WindowId])
REFERENCES [dbo].[Windows] ([Id])
GO

ALTER TABLE [dbo].[Rols] CHECK CONSTRAINT [FK_dbo.Rols_dbo.Windows_WindowId]
GO

ALTER TABLE [dbo].[Usuarios]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Usuarios_dbo.Rols_RolId] FOREIGN KEY([RolId])
REFERENCES [dbo].[Rols] ([RolId])
GO

ALTER TABLE [dbo].[Usuarios] CHECK CONSTRAINT [FK_dbo.Usuarios_dbo.Rols_RolId]
GO

ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Visits_dbo.DeliveryStates_DeliveryStateId] FOREIGN KEY([DeliveryStateId])
REFERENCES [dbo].[DeliveryStates] ([Id])
GO

ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_dbo.Visits_dbo.DeliveryStates_DeliveryStateId]
GO

ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Visits_dbo.Personas_PersonaId] FOREIGN KEY([PersonaId])
REFERENCES [dbo].[Personas] ([Id])
GO

ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_dbo.Visits_dbo.Personas_PersonaId]
GO

ALTER TABLE [dbo].[Windows]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Windows_dbo.Modules_ModuleId] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Modules] ([Id])
GO

ALTER TABLE [dbo].[Windows] CHECK CONSTRAINT [FK_dbo.Windows_dbo.Modules_ModuleId]
GO

USE [AyudaSocial]
GO

/****** Object:  Table [dbo].[Nighborhoods]    Script Date: 08/04/2020 22:11:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Nighborhoods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Direccion] [nvarchar](max) NULL,
	[Altura] [nvarchar](max) NULL,
	[Partida] [nvarchar](max) NULL,
	[Barrio] [nvarchar](max) NULL,
	[Nombre] [nvarchar](max) NULL,
	[Zona] [nvarchar](max) NULL,
	[NombreCorto] [nvarchar](max) NULL,
	[Codigo] [int] NOT NULL,
	[Habitantes2010] [nvarchar](max) NULL,
	[ProyecionHabitantes2016] [nvarchar](max) NULL,
	[ProyecionHabitantes2020] [nvarchar](max) NULL,
	[Detalle] [nvarchar](max) NULL,
	[Latitud] [nvarchar](max) NULL,
	[Longitud] [nvarchar](max) NULL,
	[Perimetro] [nvarchar](max) NULL,
	[Area] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Nighborhoods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

