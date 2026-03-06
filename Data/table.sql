-- SQL Server schema (EF6 compatible) for Akoho ASPX.
-- NOTE: this script recreates the tables (drops old tables first).
-- Database name: AkohoDb

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'dbo.mouvementLot', N'U') IS NOT NULL DROP TABLE dbo.mouvementLot;
IF OBJECT_ID(N'dbo.lot', N'U') IS NOT NULL DROP TABLE dbo.lot;
IF OBJECT_ID(N'dbo.typeMouvement', N'U') IS NOT NULL DROP TABLE dbo.typeMouvement;
IF OBJECT_ID(N'dbo.prixVenteRaceParPoids', N'U') IS NOT NULL DROP TABLE dbo.prixVenteRaceParPoids;
IF OBJECT_ID(N'dbo.croissanceAlimentRace', N'U') IS NOT NULL DROP TABLE dbo.croissanceAlimentRace;
IF OBJECT_ID(N'dbo.croissancePoidsRace', N'U') IS NOT NULL DROP TABLE dbo.croissancePoidsRace;
IF OBJECT_ID(N'dbo.race', N'U') IS NOT NULL DROP TABLE dbo.race;
GO

CREATE TABLE dbo.race
(
    id INT IDENTITY(1,1) NOT NULL,
    nom NVARCHAR(80) NOT NULL,
    jourFoyAtody INT NOT NULL,
    CONSTRAINT PK_race PRIMARY KEY CLUSTERED (id),
    CONSTRAINT CK_race_jourFoyAtody_positive CHECK (jourFoyAtody > 0)
);
GO

CREATE TABLE dbo.croissancePoidsRace
(
    id INT IDENTITY(1,1) NOT NULL,
    raceId INT NOT NULL,
    semaine NVARCHAR(15) NOT NULL,
    poids INT NOT NULL,
    CONSTRAINT PK_croissancePoidsRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_croissancePoidsRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_croissancePoidsRace_poids_nonnegative CHECK (poids >= 0)
);
GO

CREATE TABLE dbo.croissanceAlimentRace
(
    id INT IDENTITY(1,1) NOT NULL,
    raceId INT NOT NULL,
    semaine NVARCHAR(15) NOT NULL,
    aliment INT NOT NULL,
    CONSTRAINT PK_croissanceAlimentRace PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_croissanceAlimentRace_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_croissanceAlimentRace_aliment_nonnegative CHECK (aliment >= 0)
);
GO

CREATE TABLE dbo.prixVenteRaceParPoids
(
    id INT IDENTITY(1,1) NOT NULL,
    raceId INT NOT NULL,
    poidsGrame INT NOT NULL CONSTRAINT DF_prixVenteRaceParPoids_poidsGrame DEFAULT (1),
    prix DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_prixVenteRaceParPoids PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_prixVenteRaceParPoids_race FOREIGN KEY (raceId) REFERENCES dbo.race(id),
    CONSTRAINT CK_prixVenteRaceParPoids_poidsGrame_positive CHECK (poidsGrame > 0),
    CONSTRAINT CK_prixVenteRaceParPoids_prix_nonnegative CHECK (prix >= 0)
);
GO

CREATE TABLE dbo.lot
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_lot_creation DEFAULT (GETDATE()),
    dateAfoyAkoho DATETIME NULL,
    nomLot NVARCHAR(80) NOT NULL,
    raceID INT NOT NULL,
    nombreInitial INT NOT NULL,
    poidsAchat INT NOT NULL,
    totalInvesti DECIMAL(10,2) NOT NULL CONSTRAINT DF_lot_totalInvesti DEFAULT (0),
    lotParent INT NULL,
    statu INT NOT NULL CONSTRAINT DF_lot_statu DEFAULT (0),
    CONSTRAINT PK_lot PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_lot_race FOREIGN KEY (raceID) REFERENCES dbo.race(id),
    CONSTRAINT FK_lot_parent FOREIGN KEY (lotParent) REFERENCES dbo.lot(id),
    CONSTRAINT CK_lot_nombreInitial_nonnegative CHECK (nombreInitial >= 0),
    CONSTRAINT CK_lot_poidsAchat_nonnegative CHECK (poidsAchat >= 0),
    CONSTRAINT CK_lot_totalInvesti_nonnegative CHECK (totalInvesti >= 0),
    CONSTRAINT CK_lot_statu_valid CHECK (statu IN (0, 1)),
    CONSTRAINT CK_lot_parent_not_self CHECK (lotParent IS NULL OR lotParent <> id)
);
GO

CREATE TABLE dbo.typeMouvement
(
    id INT IDENTITY(1,1) NOT NULL,
    nom NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_typeMouvement PRIMARY KEY CLUSTERED (id),
    CONSTRAINT UQ_typeMouvement_nom UNIQUE (nom)
);
GO

CREATE TABLE dbo.mouvementLot
(
    id INT IDENTITY(1,1) NOT NULL,
    creation DATETIME NOT NULL CONSTRAINT DF_mouvementLot_creation DEFAULT (GETDATE()),
    lotId INT NOT NULL,
    quantite INT NOT NULL,
    typeID INT NOT NULL,
    CONSTRAINT PK_mouvementLot PRIMARY KEY CLUSTERED (id),
    CONSTRAINT FK_mouvementLot_lot FOREIGN KEY (lotId) REFERENCES dbo.lot(id),
    CONSTRAINT FK_mouvementLot_typeMouvement FOREIGN KEY (typeID) REFERENCES dbo.typeMouvement(id),
    CONSTRAINT CK_mouvementLot_quantite_positive CHECK (quantite > 0)
);
GO

CREATE INDEX IX_croissancePoidsRace_raceId_semaine ON dbo.croissancePoidsRace(raceId, semaine);
CREATE INDEX IX_croissanceAlimentRace_raceId_semaine ON dbo.croissanceAlimentRace(raceId, semaine);
CREATE INDEX IX_prixVenteRaceParPoids_raceId_poidsGrame ON dbo.prixVenteRaceParPoids(raceId, poidsGrame);
CREATE INDEX IX_lot_raceID ON dbo.lot(raceID);
CREATE INDEX IX_lot_lotParent ON dbo.lot(lotParent);
CREATE INDEX IX_mouvementLot_lotId_creation ON dbo.mouvementLot(lotId, creation);
CREATE INDEX IX_mouvementLot_typeID ON dbo.mouvementLot(typeID);
GO

INSERT INTO dbo.typeMouvement(nom) VALUES (N'akoho'), (N'maty');
GO
