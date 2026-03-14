-- SQL Server schema aligned with current Models
-- (excluding DashboardLotData and ErrorViewModel).
-- NOTE: this script recreates the tables (drops old tables first).

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.lot', N'U') IS NOT NULL AND OBJECT_ID(N'dbo.FK_lot_lotOeuf', N'F') IS NOT NULL
    ALTER TABLE dbo.lot DROP CONSTRAINT FK_lot_lotOeuf;
IF OBJECT_ID(N'dbo.lotOeuf', N'U') IS NOT NULL AND OBJECT_ID(N'dbo.FK_lotOeuf_lotParent', N'F') IS NOT NULL
    ALTER TABLE dbo.lotOeuf DROP CONSTRAINT FK_lotOeuf_lotParent;
GO

IF OBJECT_ID(N'dbo.mouvementLot', N'U') IS NOT NULL DROP TABLE dbo.mouvementLot;
IF OBJECT_ID(N'dbo.prixNourritureRace', N'U') IS NOT NULL DROP TABLE dbo.prixNourritureRace;
IF OBJECT_ID(N'dbo.prixVenteRace', N'U') IS NOT NULL DROP TABLE dbo.prixVenteRace;
IF OBJECT_ID(N'dbo.lotOeuf', N'U') IS NOT NULL DROP TABLE dbo.lotOeuf;
IF OBJECT_ID(N'dbo.lot', N'U') IS NOT NULL DROP TABLE dbo.lot;
IF OBJECT_ID(N'dbo.croissanceAlimentRace', N'U') IS NOT NULL DROP TABLE dbo.croissanceAlimentRace;
IF OBJECT_ID(N'dbo.croissancePoidsRace', N'U') IS NOT NULL DROP TABLE dbo.croissancePoidsRace;
IF OBJECT_ID(N'dbo.typeLot', N'U') IS NOT NULL DROP TABLE dbo.typeLot;
IF OBJECT_ID(N'dbo.race', N'U') IS NOT NULL DROP TABLE dbo.race;
GO

CREATE TABLE dbo.race
(
    id INT IDENTITY(1,1) NOT NULL,
    nom NVARCHAR(80) NOT NULL,
    dureEclosionOeuf INT NOT NULL,
    poidsDefaut INT NOT NULL,
    capacitePondesion INT NOT NULL CONSTRAINT DF_race_pondesion DEFAULT (0),
    CONSTRAINT PK_race PRIMARY KEY CLUSTERED (id),
    CONSTRAINT CK_race_dureEclosionOeuf_nonnegative CHECK (dureEclosionOeuf >= 0),
    CONSTRAINT CK_race_poidsDefaut_nonnegative CHECK (poidsDefaut >= 0)
);
GO

CREATE TABLE dbo.croissancePoidsRace
(
    id INT IDENTITY(1,1) NOT NULL,
    raceId INT NOT NULL,
    valueSemaine INT NOT NULL,
    poidsMoyen INT NOT NULL,
    CONSTRAINT PK_croissancePoidsRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_croissancePoidsRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_croissancePoidsRace_valueSemaine_positive CHECK (valueSemaine > 0),
    CONSTRAINT CK_croissancePoidsRace_poidsMoyen_nonnegative CHECK (poidsMoyen >= 0)
);
GO

CREATE TABLE dbo.croissanceAlimentRace
(
    id INT IDENTITY(1,1) NOT NULL,
    raceId INT NOT NULL,
    valueSemaine INT NOT NULL,
    poidsMoyen INT NOT NULL,
    CONSTRAINT PK_croissanceAlimentRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_croissanceAlimentRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_croissanceAlimentRace_valueSemaine_positive CHECK (valueSemaine > 0),
    CONSTRAINT CK_croissanceAlimentRace_poidsMoyen_nonnegative CHECK (poidsMoyen >= 0)
);
GO

CREATE TABLE dbo.lotOeuf
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_lotOeuf_creation DEFAULT (GETDATE()),
    dateEclosion DATETIME NULL,
    lotParentId INT NOT NULL,
    raceId INT NOT NULL,
    nbOeufs INT NOT NULL,
    pourcentage DECIMAL(10,2) NULL,
    validation BIT NOT NULL CONSTRAINT DF_lotOeuf_validation DEFAULT 0,  -- FALSE par défaut
    CONSTRAINT PK_lotOeuf PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_lotOeuf_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_lotOeuf_nbOeufs_nonnegative CHECK (nbOeufs >= 0),
    CONSTRAINT CK_lotOeuf_pourcentage_nonnegative CHECK (pourcentage >= 0)
);
GO

CREATE TABLE dbo.parametrePondetionOeuf
(
    id INT IDENTITY(1,1) NOT NULL,
    lotOeufId INT NOT NULL,
    pourcentageMal INT NOT NULL,
    pourcentageFemelle INT NOT NULL, 
    CONSTRAINT FK_parametreLot_lotOeuf FOREIGN KEY (lotOeufId) REFERENCES dbo.lotOeuf(id)
);
GO

CREATE TABLE dbo.lot
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_lot_creation DEFAULT (GETDATE()),
    nomLot NVARCHAR(80) NOT NULL,
    raceId INT NOT NULL,
    nombreInitial INT NOT NULL,
    poidsInitiale INT NOT NULL,
    prixAchat DECIMAL(10,2) NOT NULL CONSTRAINT DF_lot_prixAchat DEFAULT (0),
    lotOeufId INT NULL,
    maxCapacitePondesion INT NOT NULL,
    CONSTRAINT PK_lot PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_lot_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_lot_nombreInitial_nonnegative CHECK (nombreInitial >= 0),
    CONSTRAINT CK_lot_poidsInitiale_nonnegative CHECK (poidsInitiale >= 0),
    CONSTRAINT CK_lot_prixAchat_nonnegative CHECK (prixAchat >= 0)
);
GO

ALTER TABLE dbo.lotOeuf
ADD CONSTRAINT FK_lotOeuf_lotParent FOREIGN KEY (lotParentId) REFERENCES dbo.lot(id);
GO

CREATE TABLE dbo.prixVenteRace
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_prixVenteRace_creation DEFAULT (GETDATE()),
    raceId INT NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    valeurGrame INT NOT NULL CONSTRAINT DF_prixVenteRace_valeurGrame DEFAULT (1),
    CONSTRAINT PK_prixVenteRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_prixVenteRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_prixVenteRace_prix_nonnegative CHECK (prix >= 0),
    CONSTRAINT CK_prixVenteRace_valeurGrame_positive CHECK (valeurGrame > 0)
);
GO

CREATE TABLE dbo.prixNourritureRace
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_prixNourritureRace_creation DEFAULT (GETDATE()),
    raceId INT NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    valeurGrame INT NOT NULL CONSTRAINT DF_prixNourritureRace_valeurGrame DEFAULT (1),
    CONSTRAINT PK_prixNourritureRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_prixNourritureRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_prixNourritureRace_prix_nonnegative CHECK (prix >= 0),
    CONSTRAINT CK_prixNourritureRace_valeurGrame_positive CHECK (valeurGrame >= 0)
);
GO

CREATE TABLE dbo.prixVenteAtody
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_prixVenteAtody_creation DEFAULT (GETDATE()),
    raceId INT NOT NULL,
    prix DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_prixVenteAtody PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_prixVenteAtody_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_prixVenteAtody_prix_nonnegative CHECK (prix >= 0)
);
GO

CREATE TABLE dbo.mouvementLot
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_mouvementLot_creation DEFAULT (GETDATE()),
    lotId INT NOT NULL,
    nombre INT NOT NULL,
    CONSTRAINT PK_mouvementLot PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_mouvementLot_lot FOREIGN KEY (lotId) REFERENCES dbo.lot(id),
    CONSTRAINT CK_mouvementLot_nombre_nonnegative CHECK (nombre >= 0)
);
GO

CREATE INDEX IX_croissancePoidsRace_raceId_valueSemaine ON dbo.croissancePoidsRace(raceId, valueSemaine);
CREATE INDEX IX_croissanceAlimentRace_raceId_valueSemaine ON dbo.croissanceAlimentRace(raceId, valueSemaine);
CREATE INDEX IX_lotOeuf_lotParentId ON dbo.lotOeuf(lotParentId);
CREATE INDEX IX_lotOeuf_raceId ON dbo.lotOeuf(raceId);
CREATE INDEX IX_lot_raceId ON dbo.lot(raceId);
CREATE INDEX IX_lot_lotOeufId ON dbo.lot(lotOeufId);
CREATE INDEX IX_prixVenteRace_raceId_valeurGrame ON dbo.prixVenteRace(raceId, valeurGrame);
CREATE INDEX IX_prixNourritureRace_raceId_valeurGrame ON dbo.prixNourritureRace(raceId, valeurGrame);
CREATE INDEX IX_mouvementLot_lotId_creation ON dbo.mouvementLot(lotId, creation);
GO


ALTER TABLE dbo.lotOeuf ALTER COLUMN pourcentage DECIMAL(10,2) NULL;


ALTER TABLE dbo.croissancePoidsRace NOCHECK CONSTRAINT CK_croissancePoidsRace_valueSemaine_positive;
ALTER TABLE dbo.croissanceAlimentRace NOCHECK CONSTRAINT CK_croissanceAlimentRace_valueSemaine_positive;
GO