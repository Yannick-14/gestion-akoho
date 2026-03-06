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

        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<CroissancePoidsRace> CroissancesPoidsRace { get; set; }
        public virtual DbSet<CroissanceAlimentRace> CroissancesAlimentRace { get; set; }
        public virtual DbSet<PrixVenteRaceParPoids> PrixVentesRaceParPoids { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<TypeMouvement> TypesMouvement { get; set; }
        public virtual DbSet<MouvementLot> MouvementsLot { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Race>()
                .ToTable("race")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Race>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Race>().Property(e => e.Nom)
                .HasColumnName("nom")
                .HasMaxLength(80);
            modelBuilder.Entity<Race>().Property(e => e.JourFoyAtody)
                .HasColumnName("jourFoyAtody");

            modelBuilder.Entity<CroissancePoidsRace>()
                .ToTable("croissancePoidsRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.RaceId).HasColumnName("raceId");
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.Semaine)
                .HasColumnName("semaine")
                .HasMaxLength(15);
            modelBuilder.Entity<CroissancePoidsRace>().Property(e => e.Poids)
                .HasColumnName("poids");

            modelBuilder.Entity<CroissancePoidsRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.CroissancesPoids)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CroissanceAlimentRace>()
                .ToTable("croissanceAlimentRace")
                .HasKey(e => e.Id);

            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.RaceId).HasColumnName("raceId");
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.Semaine)
                .HasColumnName("semaine")
                .HasMaxLength(15);
            modelBuilder.Entity<CroissanceAlimentRace>().Property(e => e.Aliment)
                .HasColumnName("aliment");

            modelBuilder.Entity<CroissanceAlimentRace>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.CroissancesAliment)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PrixVenteRaceParPoids>()
                .ToTable("prixVenteRaceParPoids")
                .HasKey(e => e.Id);

            modelBuilder.Entity<PrixVenteRaceParPoids>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PrixVenteRaceParPoids>().Property(e => e.RaceId)
                .HasColumnName("raceId");
            modelBuilder.Entity<PrixVenteRaceParPoids>().Property(e => e.PoidsGrame)
                .HasColumnName("poidsGrame");
            modelBuilder.Entity<PrixVenteRaceParPoids>().Property(e => e.Prix)
                .HasColumnName("prix")
                .HasPrecision(10, 2);

            modelBuilder.Entity<PrixVenteRaceParPoids>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.PrixVentesParPoids)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lot>()
                .ToTable("lot")
                .HasKey(e => e.Id);

            modelBuilder.Entity<Lot>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Lot>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<Lot>().Property(e => e.DateAfoyAkoho)
                .HasColumnName("dateAfoyAkoho")
                .HasColumnType("datetime");
            modelBuilder.Entity<Lot>().Property(e => e.NomLot)
                .HasColumnName("nomLot")
                .HasMaxLength(80);
            modelBuilder.Entity<Lot>().Property(e => e.RaceId).HasColumnName("raceID");
            modelBuilder.Entity<Lot>().Property(e => e.NombreInitial).HasColumnName("nombreInitial");
            modelBuilder.Entity<Lot>().Property(e => e.PoidsAchat).HasColumnName("poidsAchat");
            modelBuilder.Entity<Lot>().Property(e => e.TotalInvesti)
                .HasColumnName("totalInvesti")
                .HasPrecision(10, 2);
            modelBuilder.Entity<Lot>().Property(e => e.LotParent).HasColumnName("lotParent");
            modelBuilder.Entity<Lot>().Property(e => e.Statu).HasColumnName("statu");

            modelBuilder.Entity<Lot>()
                .HasRequired(e => e.Race)
                .WithMany(r => r.Lots)
                .HasForeignKey(e => e.RaceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lot>()
                .HasOptional(e => e.ParentLot)
                .WithMany(l => l.ChildLots)
                .HasForeignKey(e => e.LotParent)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeMouvement>()
                .ToTable("typeMouvement")
                .HasKey(e => e.Id);

            modelBuilder.Entity<TypeMouvement>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TypeMouvement>().Property(e => e.Nom)
                .HasColumnName("nom")
                .HasMaxLength(100);

            modelBuilder.Entity<MouvementLot>()
                .ToTable("mouvementLot")
                .HasKey(e => e.Id);

            modelBuilder.Entity<MouvementLot>().Property(e => e.Id)
                .HasColumnName("id")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MouvementLot>().Property(e => e.Creation)
                .HasColumnName("creation")
                .HasColumnType("datetime");
            modelBuilder.Entity<MouvementLot>().Property(e => e.LotId).HasColumnName("lotId");
            modelBuilder.Entity<MouvementLot>().Property(e => e.Quantite).HasColumnName("quantite");
            modelBuilder.Entity<MouvementLot>().Property(e => e.TypeId).HasColumnName("typeID");

            modelBuilder.Entity<MouvementLot>()
                .HasRequired(e => e.Lot)
                .WithMany(l => l.Mouvements)
                .HasForeignKey(e => e.LotId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MouvementLot>()
                .HasRequired(e => e.TypeMouvement)
                .WithMany(t => t.Mouvements)
                .HasForeignKey(e => e.TypeId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
