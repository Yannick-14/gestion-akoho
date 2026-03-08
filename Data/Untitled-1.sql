-- ============================================================
-- 1. RACE
-- ============================================================
INSERT INTO dbo.race (nom, dureEclosionOeuf, poidsDefaut)
VALUES ('Pondeuse', 45, 1250);

INSERT INTO dbo.race (nom, dureEclosionOeuf, poidsDefaut)
VALUES ('Chair', 45, 2500);
GO

-- ============================================================
-- 2. PRIX DE VENTE PAR RACE
-- ============================================================
INSERT INTO dbo.prixVenteRace (raceId, prix, valeurGrame)
VALUES (1, 500, 1);   -- Pondeuse

INSERT INTO dbo.prixVenteRace (raceId, prix, valeurGrame)
VALUES (2, 700, 1);   -- Chair
GO

-- ============================================================
-- 3. CROISSANCE POIDS PAR RACE
-- ============================================================
-- Pondeuse semaines 1 à 15
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  1,  40);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  2,  50);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  3,  60);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  4,  70);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  5,  80);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  6,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  7,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  8,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  9,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 10,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 11,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 12,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 13,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 14,  90);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 15,  90);

-- Chair semaines 1 à 15
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  1,  80);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  2, 100);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  3, 120);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  4, 150);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  5, 180);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  6, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  7, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  8, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  9, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 10, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 11, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 12, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 13, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 14, 200);
INSERT INTO dbo.croissancePoidsRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 15, 200);
GO

-- ============================================================
-- 4. CROISSANCE ALIMENT PAR RACE
-- ============================================================
-- Pondeuse semaines 1 à 15
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  1, 100);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  2, 120);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  3, 140);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  4, 160);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  5, 180);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  6, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  7, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  8, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1,  9, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 10, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 11, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 12, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 13, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 14, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (1, 15, 200);

-- Chair semaines 1 à 15
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  1, 150);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  2, 180);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  3, 200);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  4, 220);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  5, 250);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  6, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  7, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  8, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2,  9, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 10, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 11, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 12, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 13, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 14, 280);
INSERT INTO dbo.croissanceAlimentRace (raceId, valueSemaine, poidsMoyen) VALUES (2, 15, 280);
GO

-- ============================================================
-- 5. PRIX NOURRITURE PAR RACE
-- ============================================================
INSERT INTO dbo.prixNourritureRace (raceId, prix, valeurGrame)
VALUES (1, 200, 1);   -- Pondeuse

INSERT INTO dbo.prixNourritureRace (raceId, prix, valeurGrame)
VALUES (2, 100, 1);   -- Chair
GO

-- ============================================================
-- 6. LOT
-- ============================================================
INSERT INTO dbo.lot (creation, nomLot, raceId, nombreInitial, poidsInitiale, prixAchat)
VALUES (CONVERT(DATETIME, '20260227', 112), 'Maison A', 1, 500, 150, 500000);

INSERT INTO dbo.lot (creation, nomLot, raceId, nombreInitial, poidsInitiale, prixAchat)
VALUES (CONVERT(DATETIME, '20260227', 112), 'Maison B', 2, 300, 150, 400000);

INSERT INTO dbo.lot (creation, nomLot, raceId, nombreInitial, poidsInitiale, prixAchat)
VALUES (CONVERT(DATETIME, '20260220', 112), 'Maison C', 1, 200, 200, 200000);
GO

-- ============================================================
-- 7. MOUVEMENT LOT
-- lotId 1 = Maison A | lotId 3 = Maison C
-- ============================================================
INSERT INTO dbo.mouvementLot (creation, lotId, nombre)
VALUES (CONVERT(DATETIME, '20260301', 112), 1, 40);

INSERT INTO dbo.mouvementLot (creation, lotId, nombre)
VALUES (CONVERT(DATETIME, '20260225', 112), 3, 5);
GO

-- ============================================================
-- 8. LOT OEUF
-- lotParentId 1 = Maison A | raceId 1 = Pondeuse
-- ============================================================
INSERT INTO dbo.lotOeuf (creation, dateEclosion, lotParentId, raceId, nbOeufs, pourcentage, validation)
VALUES (CONVERT(DATETIME, '20260301', 112), CONVERT(DATETIME, '20260415', 112), 1, 1, 300, 0, 0);

INSERT INTO dbo.lotOeuf (creation, dateEclosion, lotParentId, raceId, nbOeufs, pourcentage, validation)
VALUES (CONVERT(DATETIME, '20260302', 112), CONVERT(DATETIME, '20260416', 112), 1, 1, 250, 0, 0);
GO