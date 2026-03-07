USE [AkohoDb]
GO

-- Insertion de deux races
INSERT INTO dbo.race (nom, dureEclosionOeuf, poidsDefaut)
VALUES
    (N'Poulet Standard', 21, 40),
    (N'Poulet Amélioré', 24, 45);
GO

-- Croissance poids pour 5 semaines
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen)
VALUES
    (1, 1, 150), (1, 2, 350), (1, 3, 700), (1, 4, 1200), (1, 5, 2000),
    (2, 1, 130), (2, 2, 300), (2, 3, 650), (2, 4, 1100), (2, 5, 1800);
GO

-- Croissance aliment (consommation hebdomadaire en grammes)
INSERT INTO dbo.croissanceAlimentRace (raceId, semaine, poidsMoyen)
VALUES
    (1, 1, 200), (1, 2, 400), (1, 3, 800), (1, 4, 1400), (1, 5, 2200),
    (2, 1, 180), (2, 2, 380), (2, 3, 750), (2, 4, 1300), (2, 5, 2100);
GO

-- Prix de vente (par gramme)
INSERT INTO dbo.prixVenteRace (raceId, prix, valeurGrame)
VALUES
    (1, 0.50, 1),
    (2, 0.55, 1);
GO

-- Prix de la nourriture (par gramme)
INSERT INTO dbo.prixNourritureRace (raceId, prix, valeurGrame)
VALUES
    (1, 0.020, 1),
    (2, 0.022, 1);
GO

-- REGION INSERT LOT
INSERT INTO dbo.lot (creation, nomLot, raceId, nombreInitial, poidsInitiale, prixAchat, lotOeufId)
VALUES
    (GETDATE(), N'Lot Achat 1', 1, 100, 200, 5000.00, NULL),
    (GETDATE(), N'Lot Éclosion 1', 2, 50, 40, 0.00, NULL);
GO

-- Insertion de trois enregistrements de lotOeuf avec validation = true
-- On suppose que les lots insérés ci-dessus ont des IDs 1 et 2
INSERT INTO dbo.lotOeuf (creation, dateEclosion, lotParentId, raceId, nbOeufs, pourcentage, validation)
VALUES
    (GETDATE(), DATEADD(day, 21, GETDATE()), 1, 1, 200, 75.5, 1),
    (GETDATE(), DATEADD(day, 22, GETDATE()), 1, 1, 180, 80.0, 1),
    (GETDATE(), DATEADD(day, 24, GETDATE()), 2, 2, 150, 90.0, 1);
GO