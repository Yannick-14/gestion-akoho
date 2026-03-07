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