
GO
SET IDENTITY_INSERT [dbo].[Modules] ON 
GO
INSERT [dbo].[Modules] ([Id], [Descripcion], [Enable]) VALUES (1, N'Menu Principal', 1)
GO
INSERT [dbo].[Modules] ([Id], [Descripcion], [Enable]) VALUES (2, N'ABM Maestros', 1)

INSERT [dbo].[Modules] ([Id], [Descripcion], [Enable]) VALUES (3, N'Tipificaciones', 1)
GO
INSERT [dbo].[Modules] ([Id], [Descripcion], [Enable]) VALUES (4, N'Configuración', 1)
GO
SET IDENTITY_INSERT [dbo].[Modules] OFF
GO
SET IDENTITY_INSERT [dbo].[Windows] ON 
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (2, 2, N'Centros', 1, N'Centers/Index', N'1')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (3, 2, N'Productos', 1, N'Products/Index', N'2')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (4, 4, N'Roles', 1, N'Rols/Index', N'1')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (5, 4, N'Usuarios', 1, N'Usuarios/Index', N'2')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (6, 4, N'Permisos', 1, N'Permissions/Index', N'3')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (7, 4, N'Auditoria', 1, N'Audits/Index', N'4')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (8, 4, N'Sistema', 1, N'Settings/Edit', N'5')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (9, 1, N'Pedidos', 1, N'Orders/Index', N'1')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (10, 1, N'Gestion Insumos', 1, N'Products/Index', N'2')
GO
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (11, 3, N'Tipo Insumo', 1, N'SupplieMedicals/Index', N'1')
INSERT [dbo].[Windows] ([Id], [ModuleId], [Descripcion], [Enable], [Url], [Orden]) VALUES (11, 3, N'Tipo Producto', 1, N'ProductTypes/Index', N'2')
GO
SET IDENTITY_INSERT [dbo].[Windows] OFF
GO
SET IDENTITY_INSERT [dbo].[Rols] ON 
GO
INSERT [dbo].[Rols] ([RolId], [Nombre], [Descripcion], [IsAdmin], [WindowId]) VALUES (1, N'Administrador', N'Administrador', 1, 11)
GO
INSERT [dbo].[Rols] ([RolId], [Nombre], [Descripcion], [IsAdmin], [WindowId]) VALUES (2, N'Operadores', N'Operadores', 0, 11)
GO
SET IDENTITY_INSERT [dbo].[Rols] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 
GO
INSERT [dbo].[Usuarios] ([UsuarioId], [Nombre], [Apellido], [Nombreusuario], [Contraseña], [Enable], [RolId]) VALUES (1, NULL, NULL, N'admin', N'1234', 1, 1)
GO
INSERT [dbo].[Usuarios] ([UsuarioId], [Nombre], [Apellido], [Nombreusuario], [Contraseña], [Enable], [RolId]) VALUES (2, N'Usuario', N'Operador', N'operador', N'1234', 1, 2)
GO
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET IDENTITY_INSERT [dbo].[Permissions] ON 
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (1, 2, 2, 1, 1, 1, CAST(N'2020-04-08T21:56:20.337' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (2, 3, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.343' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (3, 4, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.343' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (4, 5, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (5, 6, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (6, 7, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (7, 8, 2, 0, 0, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (8, 9, 2, 1, 1, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (9, 10, 2, 1, 1, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
INSERT [dbo].[Permissions] ([Id], [WindowId], [RolId], [Consulta], [AltaModificacion], [Baja], [Fecha]) VALUES (10, 11, 2, 1, 1, 0, CAST(N'2020-04-08T21:56:20.347' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Permissions] OFF
GO
