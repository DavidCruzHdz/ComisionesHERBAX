
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/24/2019 18:53:03
-- Generated from EDMX file: C:\Users\hsolano\documents\visual studio 2015\Projects\Comisiones\Comisiones\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Evo_Comisiones];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__CO_tb_Mov__id_Co__48CFD27E]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CO_tb_MovimientosSocio] DROP CONSTRAINT [FK__CO_tb_Mov__id_Co__48CFD27E];
GO
IF OBJECT_ID(N'[dbo].[FK__CO_tb_Pre__id_Pr__5165187F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CO_tb_PrestamoDetalle] DROP CONSTRAINT [FK__CO_tb_Pre__id_Pr__5165187F];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CO_tb_Comisiones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_Comisiones];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_Comisiones_hist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_Comisiones_hist];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_CompaniaPago]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_CompaniaPago];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_Conceptos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_Conceptos];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_CuentaSocioHistorico]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_CuentaSocioHistorico];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_ImpuestoComision]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_ImpuestoComision];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_ImpuestoPais]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_ImpuestoPais];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_MovimientosSocio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_MovimientosSocio];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_PagoSocio]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_PagoSocio];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_PagoSocio_hist]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_PagoSocio_hist];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_PagosRespuesta]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_PagosRespuesta];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_Parametro]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_Parametro];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_Prestamo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_Prestamo];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_PrestamoDetalle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_PrestamoDetalle];
GO
IF OBJECT_ID(N'[dbo].[CO_tb_RecepcionDepositos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CO_tb_RecepcionDepositos];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CO_tb_Comisiones'
CREATE TABLE [dbo].[CO_tb_Comisiones] (
    [id_Comisiones] int IDENTITY(1,1) NOT NULL,
    [id_Periodo] int  NULL,
    [partner_id] varchar(10)  NOT NULL,
    [id_Concepto] int  NULL,
    [fechaAplicacion] datetime  NULL,
    [monto] decimal(18,2)  NOT NULL,
    [observaciones] varchar(150)  NULL,
    [estatus] int  NULL,
    [folioCalculo] int  NULL,
    [fechaRegistro] datetime  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_Comisiones_hist'
CREATE TABLE [dbo].[CO_tb_Comisiones_hist] (
    [id_Comisiones_hist] int IDENTITY(1,1) NOT NULL,
    [id_Comisiones] int  NULL,
    [id_Periodo] int  NULL,
    [partner_id] varchar(10)  NOT NULL,
    [id_Concepto] int  NULL,
    [fechaAplicacion] datetime  NULL,
    [monto] decimal(18,2)  NOT NULL,
    [observaciones] varchar(150)  NULL,
    [estatus] int  NULL,
    [folioCalculo] int  NULL,
    [fechaRegistro] datetime  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_CompaniaPago'
CREATE TABLE [dbo].[CO_tb_CompaniaPago] (
    [id_CompaniaPago] int IDENTITY(1,1) NOT NULL,
    [nombreCompania] varchar(100)  NOT NULL,
    [porcentaje] decimal(18,0)  NOT NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_Conceptos'
CREATE TABLE [dbo].[CO_tb_Conceptos] (
    [id_Concepto] int IDENTITY(1,1) NOT NULL,
    [descripcion] varchar(50)  NOT NULL,
    [tipo] char(1)  NOT NULL,
    [gravable] smallint  NOT NULL,
    [pagoEspecie] smallint  NOT NULL,
    [estatus] smallint  NOT NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_CuentaSocioHistorico'
CREATE TABLE [dbo].[CO_tb_CuentaSocioHistorico] (
    [id_CuentaSocioHistorico] int IDENTITY(1,1) NOT NULL,
    [partner_id] varchar(10)  NOT NULL,
    [id_Banco] int  NULL,
    [Clabe] varchar(20)  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_ImpuestoComision'
CREATE TABLE [dbo].[CO_tb_ImpuestoComision] (
    [id_Impuesto] int IDENTITY(1,1) NOT NULL,
    [limiteInferior] decimal(18,2)  NULL,
    [limiteSuperior] decimal(18,2)  NULL,
    [porcentaje] decimal(18,2)  NULL,
    [coutaFija] decimal(18,2)  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_ImpuestoPais'
CREATE TABLE [dbo].[CO_tb_ImpuestoPais] (
    [id_Impuesto] int IDENTITY(1,1) NOT NULL,
    [pais] varchar(20)  NOT NULL,
    [impuesto] decimal(18,2)  NOT NULL,
    [pagoMinimo] decimal(18,2)  NOT NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_MovimientosSocio'
CREATE TABLE [dbo].[CO_tb_MovimientosSocio] (
    [id_MovimientosSocio] int IDENTITY(1,1) NOT NULL,
    [partner_id] varchar(10)  NOT NULL,
    [id_Concepto] int  NOT NULL,
    [comentario] varchar(255)  NOT NULL,
    [monto] decimal(18,2)  NOT NULL,
    [fechaRegistro] datetime  NOT NULL,
    [estatus] smallint  NOT NULL,
    [folioCalculo] int  NULL,
    [Id_Periodo] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_PagoSocio'
CREATE TABLE [dbo].[CO_tb_PagoSocio] (
    [id_PagoSocio] int IDENTITY(1,1) NOT NULL,
    [partner_id] varchar(10)  NOT NULL,
    [monto] decimal(18,2)  NOT NULL,
    [id_Periodo] int  NULL,
    [id_CompaniaPago] int  NULL,
    [fechaRegistro] datetime  NOT NULL,
    [fechaPago] datetime  NULL,
    [estatus] smallint  NOT NULL,
    [folioCalculo] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_PagoSocio_hist'
CREATE TABLE [dbo].[CO_tb_PagoSocio_hist] (
    [id_PagoSocio_hist] int IDENTITY(1,1) NOT NULL,
    [id_PagoSocio] int  NULL,
    [partner_id] varchar(10)  NOT NULL,
    [monto] decimal(18,2)  NOT NULL,
    [id_Periodo] int  NOT NULL,
    [id_CompaniaPago] int  NULL,
    [fechaRegistro] datetime  NOT NULL,
    [fechaPago] datetime  NULL,
    [estatus] smallint  NOT NULL,
    [folioCalculo] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_PagosRespuesta'
CREATE TABLE [dbo].[CO_tb_PagosRespuesta] (
    [id_PagosRespuesta] int IDENTITY(1,1) NOT NULL,
    [tiporeRegistro] int  NULL,
    [cuentaDestino] varchar(20)  NULL,
    [partner_id] varchar(10)  NULL,
    [beneficiario] varchar(150)  NULL,
    [importe] decimal(18,2)  NULL,
    [numeroReferencia] decimal(18,0)  NULL,
    [fechaAplicacion] datetime  NULL,
    [claveRastreo] varchar(50)  NULL,
    [estatus] varchar(20)  NULL,
    [motivoDevolucion] varchar(100)  NULL,
    [id_Periodo] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_Parametro'
CREATE TABLE [dbo].[CO_tb_Parametro] (
    [id_Parametro] int IDENTITY(1,1) NOT NULL,
    [parametro] varchar(20)  NOT NULL,
    [descripcion] varchar(100)  NOT NULL,
    [valor] decimal(18,0)  NOT NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_Prestamo'
CREATE TABLE [dbo].[CO_tb_Prestamo] (
    [id_Prestamo] int IDENTITY(1,1) NOT NULL,
    [partner_id] varchar(10)  NOT NULL,
    [importe] decimal(18,2)  NOT NULL,
    [tipoAplicacion] int  NOT NULL,
    [numeroPagos] int  NOT NULL,
    [id_Concepto] int  NOT NULL,
    [id_Periodo] int  NOT NULL,
    [saldo] decimal(18,2)  NOT NULL,
    [fechaInicio] datetime  NULL,
    [fechaFin] datetime  NULL,
    [estatus] int  NULL,
    [folioCalculo] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_PrestamoDetalle'
CREATE TABLE [dbo].[CO_tb_PrestamoDetalle] (
    [id_PrestamoDetalle] int IDENTITY(1,1) NOT NULL,
    [id_Prestamo] int  NULL,
    [importe] decimal(18,2)  NOT NULL,
    [pagonumero] int  NOT NULL,
    [fechaPago] datetime  NOT NULL,
    [fechaAplicacion] datetime  NOT NULL,
    [estatus] int  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- Creating table 'CO_tb_RecepcionDepositos'
CREATE TABLE [dbo].[CO_tb_RecepcionDepositos] (
    [id_RecepcionDeposito] int IDENTITY(1,1) NOT NULL,
    [numeroReferencia] varchar(20)  NULL,
    [cuentaOrigen] varchar(20)  NULL,
    [cuentaDestino] varchar(20)  NULL,
    [beneficiario] varchar(150)  NULL,
    [importe] decimal(18,2)  NULL,
    [fecha] datetime  NULL,
    [estatus] varchar(80)  NULL,
    [conceptoPagoTransf] varchar(20)  NULL,
    [referenciaInterban] varchar(20)  NULL,
    [formaAplicacion] varchar(20)  NULL,
    [fechaAplicacion] datetime  NULL,
    [claveRastreo] varchar(50)  NULL,
    [motivoDevolucion] varchar(100)  NULL,
    [iva] decimal(18,2)  NULL,
    [rfc] varchar(15)  NULL,
    [comprobanteElectronico] varchar(20)  NULL,
    [divisa] varchar(20)  NULL,
    [usuario] varchar(150)  NULL,
    [fechaMovimiento] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id_Comisiones] in table 'CO_tb_Comisiones'
ALTER TABLE [dbo].[CO_tb_Comisiones]
ADD CONSTRAINT [PK_CO_tb_Comisiones]
    PRIMARY KEY CLUSTERED ([id_Comisiones] ASC);
GO

-- Creating primary key on [id_Comisiones_hist] in table 'CO_tb_Comisiones_hist'
ALTER TABLE [dbo].[CO_tb_Comisiones_hist]
ADD CONSTRAINT [PK_CO_tb_Comisiones_hist]
    PRIMARY KEY CLUSTERED ([id_Comisiones_hist] ASC);
GO

-- Creating primary key on [id_CompaniaPago] in table 'CO_tb_CompaniaPago'
ALTER TABLE [dbo].[CO_tb_CompaniaPago]
ADD CONSTRAINT [PK_CO_tb_CompaniaPago]
    PRIMARY KEY CLUSTERED ([id_CompaniaPago] ASC);
GO

-- Creating primary key on [id_Concepto] in table 'CO_tb_Conceptos'
ALTER TABLE [dbo].[CO_tb_Conceptos]
ADD CONSTRAINT [PK_CO_tb_Conceptos]
    PRIMARY KEY CLUSTERED ([id_Concepto] ASC);
GO

-- Creating primary key on [id_CuentaSocioHistorico] in table 'CO_tb_CuentaSocioHistorico'
ALTER TABLE [dbo].[CO_tb_CuentaSocioHistorico]
ADD CONSTRAINT [PK_CO_tb_CuentaSocioHistorico]
    PRIMARY KEY CLUSTERED ([id_CuentaSocioHistorico] ASC);
GO

-- Creating primary key on [id_Impuesto] in table 'CO_tb_ImpuestoComision'
ALTER TABLE [dbo].[CO_tb_ImpuestoComision]
ADD CONSTRAINT [PK_CO_tb_ImpuestoComision]
    PRIMARY KEY CLUSTERED ([id_Impuesto] ASC);
GO

-- Creating primary key on [id_Impuesto] in table 'CO_tb_ImpuestoPais'
ALTER TABLE [dbo].[CO_tb_ImpuestoPais]
ADD CONSTRAINT [PK_CO_tb_ImpuestoPais]
    PRIMARY KEY CLUSTERED ([id_Impuesto] ASC);
GO

-- Creating primary key on [id_MovimientosSocio] in table 'CO_tb_MovimientosSocio'
ALTER TABLE [dbo].[CO_tb_MovimientosSocio]
ADD CONSTRAINT [PK_CO_tb_MovimientosSocio]
    PRIMARY KEY CLUSTERED ([id_MovimientosSocio] ASC);
GO

-- Creating primary key on [id_PagoSocio] in table 'CO_tb_PagoSocio'
ALTER TABLE [dbo].[CO_tb_PagoSocio]
ADD CONSTRAINT [PK_CO_tb_PagoSocio]
    PRIMARY KEY CLUSTERED ([id_PagoSocio] ASC);
GO

-- Creating primary key on [id_PagoSocio_hist] in table 'CO_tb_PagoSocio_hist'
ALTER TABLE [dbo].[CO_tb_PagoSocio_hist]
ADD CONSTRAINT [PK_CO_tb_PagoSocio_hist]
    PRIMARY KEY CLUSTERED ([id_PagoSocio_hist] ASC);
GO

-- Creating primary key on [id_PagosRespuesta] in table 'CO_tb_PagosRespuesta'
ALTER TABLE [dbo].[CO_tb_PagosRespuesta]
ADD CONSTRAINT [PK_CO_tb_PagosRespuesta]
    PRIMARY KEY CLUSTERED ([id_PagosRespuesta] ASC);
GO

-- Creating primary key on [id_Parametro] in table 'CO_tb_Parametro'
ALTER TABLE [dbo].[CO_tb_Parametro]
ADD CONSTRAINT [PK_CO_tb_Parametro]
    PRIMARY KEY CLUSTERED ([id_Parametro] ASC);
GO

-- Creating primary key on [id_Prestamo] in table 'CO_tb_Prestamo'
ALTER TABLE [dbo].[CO_tb_Prestamo]
ADD CONSTRAINT [PK_CO_tb_Prestamo]
    PRIMARY KEY CLUSTERED ([id_Prestamo] ASC);
GO

-- Creating primary key on [id_PrestamoDetalle] in table 'CO_tb_PrestamoDetalle'
ALTER TABLE [dbo].[CO_tb_PrestamoDetalle]
ADD CONSTRAINT [PK_CO_tb_PrestamoDetalle]
    PRIMARY KEY CLUSTERED ([id_PrestamoDetalle] ASC);
GO

-- Creating primary key on [id_RecepcionDeposito] in table 'CO_tb_RecepcionDepositos'
ALTER TABLE [dbo].[CO_tb_RecepcionDepositos]
ADD CONSTRAINT [PK_CO_tb_RecepcionDepositos]
    PRIMARY KEY CLUSTERED ([id_RecepcionDeposito] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [id_Concepto] in table 'CO_tb_MovimientosSocio'
ALTER TABLE [dbo].[CO_tb_MovimientosSocio]
ADD CONSTRAINT [FK__CO_tb_Mov__id_Co__48CFD27E]
    FOREIGN KEY ([id_Concepto])
    REFERENCES [dbo].[CO_tb_Conceptos]
        ([id_Concepto])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__CO_tb_Mov__id_Co__48CFD27E'
CREATE INDEX [IX_FK__CO_tb_Mov__id_Co__48CFD27E]
ON [dbo].[CO_tb_MovimientosSocio]
    ([id_Concepto]);
GO

-- Creating foreign key on [id_Prestamo] in table 'CO_tb_PrestamoDetalle'
ALTER TABLE [dbo].[CO_tb_PrestamoDetalle]
ADD CONSTRAINT [FK__CO_tb_Pre__id_Pr__5165187F]
    FOREIGN KEY ([id_Prestamo])
    REFERENCES [dbo].[CO_tb_Prestamo]
        ([id_Prestamo])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__CO_tb_Pre__id_Pr__5165187F'
CREATE INDEX [IX_FK__CO_tb_Pre__id_Pr__5165187F]
ON [dbo].[CO_tb_PrestamoDetalle]
    ([id_Prestamo]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------