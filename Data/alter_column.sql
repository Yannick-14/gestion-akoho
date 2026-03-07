USE [AkohoDb]
GO

-- Renommer la colonne 'semaine' en 'valueSemaine' dans la table 'croissanceAlimentRace'
EXEC sp_rename 'dbo.croissanceAlimentRace.semaine', 'valueSemaine', 'COLUMN';
GO
