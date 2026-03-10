-- USE [AkohoDb]
-- GO

-- Insertion de deux races
INSERT INTO dbo.race (nom, dureEclosionOeuf, poidsDefaut)
VALUES
    (N'Borboneze', 30, 50);
GO

-- Croissance poids pour 5 semaines
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen)
VALUES
    (1, 0, 50),
    (1, 1, 20),
    (1, 2, 25),
    (1, 3, 30),
    (1, 4, 40),
    (1, 5, 80),
    (1, 6, 85),
    (1, 7, 100),
    (1, 8, 100),
    (1, 9, 90),
    (1, 10, 140),
    (1, 11, 200),
    (1, 12, 220),
    (1, 13, 265),
    (1, 14, 285);
GO

-- Croissance aliment (consommation hebdomadaire en grammes)
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen)
VALUES
    (1, 0, 0),
    (1, 1, 75),
    (1, 2, 80),
    (1, 3, 100),
    (1, 4, 150),
    (1, 5, 170),
    (1, 6, 190),
    (1, 7, 200),
    (1, 8, 250),
    (1, 9, 270),
    (1, 10, 290),
    (1, 11, 300),
    (1, 12, 370),
    (1, 13, 390),
    (1, 14, 350);
GO

-- Prix de vente (par gramme)
INSERT INTO dbo.prixVenteRace (raceId, prix, valeurGrame)
VALUES
    (1, 15, 1);
GO

-- Prix de la nourriture (par gramme)
INSERT INTO dbo.prixNourritureRace (raceId, prix, valeurGrame)
VALUES
    (1, 5, 1);
GO

-- Insertion de trois enregistrements de lotOeuf avec validation = true
-- On suppose que les lots insérés ci-dessus ont des IDs 1 et 2
INSERT INTO dbo.lot (creation, nomLot, raceId, nombreInitial, poidsInitiale, prixAchat, lotOeufId)
VALUES
    ('2026-01-01 00:00:00', N'Lot 1', 1, 500, 50, 250000.00, NULL);
GO

INSERT INTO dbo.mouvementLot (creation, lotId, nombre)
VALUES
    ('2026-01-02 00:00:00' ,1, 15);
GO
-- yyyy-jj-mmm

INSERT INTO dbo.lotOeuf (creation, dateEclosion, lotParentId, raceId, nbOeufs, pourcentage)
VALUES
    ('2026-02-02 00:00:00', DATEADD(DAY, 30, '2026-02-02 00:00:00'), 1, 1, 100, 100);
GO
