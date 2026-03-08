using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using AkohoAspx.Models;

namespace AkohoAspx.Data
{
    public class AppDbContext : DbContext
    {
        static AppDbContext()
        {
            Database.SetInitializer<AppDbContext>(null);
        }

        public AppDbContext() : base("name=AkohoDbContext")
        {
        }

        // ── DbSets ───────────────────────────────────────────────────
        public virtual DbSet<Race>                 Races                  { get; set; }
        public virtual DbSet<CroissancePoidsRace>  CroissancesPoidsRace   { get; set; }
        public virtual DbSet<CroissanceAlimentRace> CroissancesAlimentRace { get; set; }
        public virtual DbSet<LotOeuf>              LotsOeuf               { get; set; }
        public virtual DbSet<Lot>                  Lots                   { get; set; }
        public virtual DbSet<PrixVenteRace>        PrixVentesRace         { get; set; }
        public virtual DbSet<PrixNourritureRace>   PrixNourrituresRace    { get; set; }
        public virtual DbSet<MouvementLot>         MouvementsLot          { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ── race ────────────────────────────────────────────────
            modelBuilder.Entity<Race>()
                .ToTable("race")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Race>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Race>().Property(e => e.Nom)
                .HasColumnName("nom")
                .HasMaxLength(80);
            modelBuilder.Entity<Race>().Property(e => e.DureEclosionOeuf)
                .HasColumnName("dureEclosionOeuf");
            modelBuilder.Entity<Race>().Property(e => e.PoidsDefaut)
                .HasColumnName("poidsDefaut");

            // ── croissancePoidsRace ─────────────────────────────────
            modelBuilder.Entity<CroissancePoidsRace>()
                .ToTable("croissancePoidsRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.ValueSemaine)
                .HasColumnName("valueSemaine");
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.PoidsMoyen)
                .HasColumnName("poidsMoyen");

            modelBuilder.Entity<CroissancePoidsRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.CroissancesPoids)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            // ── croissanceAlimentRace ───────────────────────────────
            modelBuilder.Entity<CroissanceAlimentRace>()
                .ToTable("croissanceAlimentRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.ValueSemaine)
                .HasColumnName("valueSemaine");
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.PoidsMoyen)
                .HasColumnName("poidsMoyen");

            modelBuilder.Entity<CroissanceAlimentRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.CroissancesAliment)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            // ── lotOeuf ─────────────────────────────────────────────
            modelBuilder.Entity<LotOeuf>()
                .ToTable("lotOeuf")
                .HasKey(e => e.Id);

            modelBuilder.Entity<LotOeuf>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LotOeuf>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<LotOeuf>().Property(e => e.DateEclosion)
                .HasColumnName("dateEclosion")
                .HasColumnType("datetime");
            modelBuilder.Entity<LotOeuf>().Property(e => e.LotParentId)
                .HasColumnName("lotParentId");
            modelBuilder.Entity<LotOeuf>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<LotOeuf>().Property(e => e.NbOeufs)
                .HasColumnName("nbOeufs");
            modelBuilder.Entity<LotOeuf>().Property(e => e.Pourcentage)
                .HasColumnName("pourcentage")
                .HasPrecision(10, 2)
                .IsOptional();
            modelBuilder.Entity<LotOeuf>().Property(e => e.Validation)
                .HasColumnName("validation");

            modelBuilder.Entity<LotOeuf>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.LotsOeuf)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LotOeuf>()
                .Ignore(e => e.ParentLot)
                .HasRequired(e => e.LotParent)
                .WithMany(l => l.LotsOeuf)
                .HasForeignKey(e => e.LotParentId)
                .WillCascadeOnDelete(false);

            // ── lot ─────────────────────────────────────────────────
            modelBuilder.Entity<Lot>()
                .ToTable("lot")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Lot>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Lot>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<Lot>().Property(e => e.NomLot)
                .HasColumnName("nomLot")
                .HasMaxLength(80);
            modelBuilder.Entity<Lot>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<Lot>().Property(e => e.NombreInitial)
                .HasColumnName("nombreInitial");
            modelBuilder.Entity<Lot>().Property(e => e.PoidsInitiale)
                .HasColumnName("poidsInitiale");
            modelBuilder.Entity<Lot>().Property(e => e.PrixAchat)
                .HasColumnName("prixAchat")
                .HasPrecision(10, 2);
            modelBuilder.Entity<Lot>().Property(e => e.LotOeufId)
                .HasColumnName("lotOeufId");

            modelBuilder.Entity<Lot>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.Lots)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lot>()
                .Ignore(e => e.ParentLot)
                .HasOptional(e => e.LotOeuf)
                .WithMany(o => o.Lots)
                .HasForeignKey(e => e.LotOeufId)
                .WillCascadeOnDelete(false);

            // ── prixVenteRace ────────────────────────────────────────
            modelBuilder.Entity<PrixVenteRace>()
                .ToTable("prixVenteRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<PrixVenteRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PrixVenteRace>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<PrixVenteRace>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<PrixVenteRace>().Property(e => e.Prix)
                .HasColumnName("prix")
                .HasPrecision(10, 2);
            modelBuilder.Entity<PrixVenteRace>().Property(e => e.ValeurGrame)
                .HasColumnName("valeurGrame");

            modelBuilder.Entity<PrixVenteRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.PrixVenteRace)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            // ── prixNourritureRace ───────────────────────────────────
            modelBuilder.Entity<PrixNourritureRace>()
                .ToTable("prixNourritureRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<PrixNourritureRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PrixNourritureRace>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<PrixNourritureRace>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<PrixNourritureRace>().Property(e => e.Prix)
                .HasColumnName("prix")
                .HasPrecision(10, 2);
            modelBuilder.Entity<PrixNourritureRace>().Property(e => e.ValeurGrame)
                .HasColumnName("valeurGrame");

            modelBuilder.Entity<PrixNourritureRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.PrixNourritures)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            // ── mouvementLot ─────────────────────────────────────────
            modelBuilder.Entity<MouvementLot>()
                .ToTable("mouvementLot")
                .HasKey(e => e.Id);

            modelBuilder.Entity<MouvementLot>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MouvementLot>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<MouvementLot>().Property(e => e.LotId)
                .HasColumnName("lotId");
            modelBuilder.Entity<MouvementLot>().Property(e => e.Nombre)
                .HasColumnName("nombre");

            modelBuilder.Entity<MouvementLot>()
                .HasRequired(e => e.Lot)
                .WithMany(l => l.Mouvements)
                .HasForeignKey(e => e.LotId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
