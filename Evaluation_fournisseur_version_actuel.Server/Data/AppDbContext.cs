using Microsoft.EntityFrameworkCore;
using evaluation_fournisseur_version_actuel.Server.Models;

namespace evaluation_fournisseur_version_actuel.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Administrateur> Administrateurs { get; set; }
        public DbSet<Campagne> Campagnes { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Fabricant> Fabricants { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Nature> Natures { get; set; }
        public DbSet<NomFournisseur> NomFournisseurs { get; set; }
        public DbSet<Ponderation> Ponderations { get; set; }
        public DbSet<Resultat> Resultats { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<ResultatGlobalFournisseur> ResultatGlobalFournisseurs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureAdministrateur(modelBuilder);
            ConfigureCampagne(modelBuilder);
            ConfigureCategorie(modelBuilder);
            ConfigureEvaluation(modelBuilder);
            ConfigureFabricant(modelBuilder);
            ConfigureFournisseur(modelBuilder);
            ConfigureNature(modelBuilder);
            ConfigureNomFournisseur(modelBuilder);
            ConfigurePonderation(modelBuilder);
            ConfigureResultat(modelBuilder);
            ConfigureSite(modelBuilder);
            ConfigureResultatGlobalFournisseur(modelBuilder);
        }

        private void ConfigureAdministrateur(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrateur>(entity =>
            {
                entity.ToTable("eval_administrateur");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_administrateur");
                entity.Property(e => e.IdentiteUtilisateur).HasColumnName("identite_utilisateur").IsRequired().HasMaxLength(200);
            });
        }

        private void ConfigureCampagne(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campagne>(entity =>
            {
                entity.ToTable("eval_campagne");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_campagne");
                entity.Property(e => e.Annee).HasColumnName("annee");
                entity.Property(e => e.NomCampagne).HasColumnName("nom_campagne").IsRequired().HasMaxLength(200);
                entity.Property(e => e.Active).HasColumnName("active");

                entity.HasMany(c => c.NomFournisseurs)
                      .WithOne(nf => nf.Campagne)
                      .HasForeignKey(nf => nf.CampagneId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(c => c.Evaluations)
                      .WithOne(e => e.Campagne)
                      .HasForeignKey(e => e.CampagneId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureCategorie(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorie>(entity =>
            {
                entity.ToTable("eval_categorie");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_categorie");
                entity.Property(e => e.NomCategorie).HasColumnName("nom_categorie").IsRequired().HasMaxLength(500);
                entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500);

                entity.HasMany(c => c.Fournisseurs)
                      .WithOne(f => f.Categorie)
                      .HasForeignKey(f => f.CategorieId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Ponderation)
                      .WithOne(p => p.Categorie)
                      .HasForeignKey<Ponderation>(p => p.CategorieId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureEvaluation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.ToTable("eval_evaluation");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_evaluation");

                entity.Property(e => e.ITransparence).HasColumnName("I_transparence");
                entity.Property(e => e.IIFacture).HasColumnName("II_facture");
                entity.Property(e => e.IIINotoriete).HasColumnName("III_notoriete");
                entity.Property(e => e.IVPreventionCorruption).HasColumnName("IV_prevention_corruption");
                entity.Property(e => e.VPreventionDroitHomme).HasColumnName("V_prevention_droit_homme");
                entity.Property(e => e.VIConformiteVsExigence).HasColumnName("VI_conformite_vs_exigence");
                entity.Property(e => e.VIIDelaiLivraison).HasColumnName("VII_delai_livraison");
                entity.Property(e => e.VIIICommunication).HasColumnName("VIII_communication");
                entity.Property(e => e.IXDisponibiliteDocuments).HasColumnName("IX_disponibilite_documents");
                entity.Property(e => e.XConsigneQhse).HasColumnName("X_consigne_QHSE");
                entity.Property(e => e.XIQualitePrix).HasColumnName("XI_qualite_prix");
                entity.Property(e => e.XIIDureePaiement).HasColumnName("XII_duree_paiement");
                entity.Property(e => e.XIIIConformiteFiscale).HasColumnName("XIII_conformite_fiscale");

                entity.Property(e => e.MoyennePct).HasColumnName("moyenne_pct");
                entity.Property(e => e.Observation).HasColumnName("observation");
                entity.Property(e => e.PropositionAmelioration).HasColumnName("proposition_amelioration");
                entity.Property(e => e.ObservationsFournisseur).HasColumnName("observations_fournisseur");
                entity.Property(e => e.Responsable).HasColumnName("responsable").IsRequired().HasMaxLength(200);
                entity.Property(e => e.NumeroActionPac).HasColumnName("numero_action_pac").HasMaxLength(100);
                entity.Property(e => e.Deadline).HasColumnName("deadline");
                entity.Property(e => e.DateEvaluation).HasColumnName("date_evaluation");

                entity.Property(e => e.FournisseurId).HasColumnName("id_fournisseur");
                entity.Property(e => e.PonderationId).HasColumnName("id_ponderation");
                entity.Property(e => e.CampagneId).HasColumnName("id_campagne");
                entity.Property(e => e.ResultatId).HasColumnName("id_resultat");

                entity.HasOne(e => e.Fournisseur)
                      .WithMany(f => f.Evaluations)
                      .HasForeignKey(e => e.FournisseurId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Ponderation)
                      .WithMany(p => p.Evaluations)
                      .HasForeignKey(e => e.PonderationId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Campagne)
                      .WithMany(c => c.Evaluations)
                      .HasForeignKey(e => e.CampagneId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Resultat)
                      .WithMany(r => r.Evaluations)
                      .HasForeignKey(e => e.ResultatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureFabricant(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fabricant>(entity =>
            {
                entity.ToTable("eval_fabricant");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_fabricant");
                entity.Property(e => e.NomFabricant).HasColumnName("nom_fabricant").IsRequired().HasMaxLength(500);

                entity.HasMany(f => f.Fournisseurs)
                      .WithOne(f => f.Fabricant)
                      .HasForeignKey(f => f.FabricantId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureFournisseur(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fournisseur>(entity =>
            {
                entity.ToTable("eval_fournisseur");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_fournisseur");

                entity.Property(e => e.NomFournisseurId).HasColumnName("id_nom_fournisseur");
                entity.Property(e => e.FabricantId).HasColumnName("id_fabricant");
                entity.Property(e => e.CategorieId).HasColumnName("id_categorie");
                entity.Property(e => e.NatureId).HasColumnName("id_nature");
                entity.Property(e => e.SiteId).HasColumnName("id_site");

                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(254);
                entity.Property(e => e.EmailEnvoye).HasColumnName("emailEnvoye");
                entity.Property(e => e.DateEnvoiEmail).HasColumnName("dateEnvoiEmail");

                entity.HasOne(f => f.NomFournisseur)
                      .WithMany(nf => nf.Fournisseurs)
                      .HasForeignKey(f => f.NomFournisseurId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Fabricant)
                      .WithMany(fa => fa.Fournisseurs)
                      .HasForeignKey(f => f.FabricantId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Categorie)
                      .WithMany(c => c.Fournisseurs)
                      .HasForeignKey(f => f.CategorieId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Nature)
                      .WithMany(n => n.Fournisseurs)
                      .HasForeignKey(f => f.NatureId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Site)
                      .WithMany(s => s.Fournisseurs)
                      .HasForeignKey(f => f.SiteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureNature(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Nature>(entity =>
            {
                entity.ToTable("eval_nature");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_nature");
                entity.Property(e => e.LibelNature).HasColumnName("libel_nature").IsRequired().HasMaxLength(500);

                entity.HasMany(n => n.Fournisseurs)
                      .WithOne(f => f.Nature)
                      .HasForeignKey(f => f.NatureId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureNomFournisseur(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NomFournisseur>(entity =>
            {
                entity.ToTable("eval_nom_fournisseur");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_nom_fournisseur");

                entity.Property(e => e.VendorNumber).HasColumnName("vendor_number").IsRequired().HasMaxLength(50);
                entity.Property(e => e.VendorName).HasColumnName("vendor_name").IsRequired().HasMaxLength(300);
                entity.Property(e => e.CompanyCode).HasColumnName("company_code").HasMaxLength(50);
                entity.Property(e => e.ApDivision).HasColumnName("ap_division").HasMaxLength(50);
                entity.Property(e => e.StrClassStatus).HasColumnName("str_class_status").HasMaxLength(100);
                entity.Property(e => e.SysCodeDescription).HasColumnName("sys_code_description").HasMaxLength(200);
                entity.Property(e => e.SignificationCompanyCode).HasColumnName("signification_companycode").HasMaxLength(200);
                entity.Property(e => e.SignificationApDivision).HasColumnName("signification_apdivision").HasMaxLength(200);
                entity.Property(e => e.StrVendorClass).HasColumnName("str_vendor_class").HasMaxLength(100);
                entity.Property(e => e.CreditTermsCode).HasColumnName("credit_terms_code").HasMaxLength(50);

                entity.Property(e => e.CampagneId).HasColumnName("id_campagne");

                entity.HasOne(nf => nf.Campagne)
                      .WithMany(c => c.NomFournisseurs)
                      .HasForeignKey(nf => nf.CampagneId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(nf => nf.Fournisseurs)
                      .WithOne(f => f.NomFournisseur)
                      .HasForeignKey(f => f.NomFournisseurId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigurePonderation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ponderation>(entity =>
            {
                entity.ToTable("eval_ponderation");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_ponderation");

                entity.Property(e => e.CategorieId).HasColumnName("id_categorie");

                entity.Property(e => e.ITransparence).HasColumnName("I_transparence");
                entity.Property(e => e.IIFacture).HasColumnName("II_facture");
                entity.Property(e => e.IIINotoriete).HasColumnName("III_notoriete");
                entity.Property(e => e.IVPreventionCorruption).HasColumnName("IV_prevention_corruption");
                entity.Property(e => e.VPreventionDroitHomme).HasColumnName("V_prevention_droit_homme");
                entity.Property(e => e.VIConformiteVsExigence).HasColumnName("VI_conformite_vs_exigence");
                entity.Property(e => e.VIIDelaiLivraison).HasColumnName("VII_delai_livraison");
                entity.Property(e => e.VIIICommunication).HasColumnName("VIII_communication");
                entity.Property(e => e.IXDisponibiliteDocuments).HasColumnName("IX_disponibilite_documents");
                entity.Property(e => e.XConsigneQhse).HasColumnName("X_consigne_QHSE");
                entity.Property(e => e.XIQualitePrix).HasColumnName("XI_qualite_prix");
                entity.Property(e => e.XIIDureePaiement).HasColumnName("XII_duree_paiement");
                entity.Property(e => e.XIIIConformiteFiscale).HasColumnName("XIII_conformite_fiscale");

                entity.HasOne(p => p.Categorie)
                      .WithOne(c => c.Ponderation)
                      .HasForeignKey<Ponderation>(p => p.CategorieId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(p => p.Evaluations)
                      .WithOne(e => e.Ponderation)
                      .HasForeignKey(e => e.PonderationId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureResultat(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resultat>(entity =>
            {
                entity.ToTable("eval_resultat");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_resultat");
                entity.Property(e => e.NomResultat).HasColumnName("nom_resultat").IsRequired().HasMaxLength(100);
                entity.Property(e => e.Observation).HasColumnName("observation").HasMaxLength(500);

                entity.HasMany(r => r.Evaluations)
                      .WithOne(e => e.Resultat)
                      .HasForeignKey(e => e.ResultatId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureSite(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Site>(entity =>
            {
                entity.ToTable("eval_site");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id_site");
                entity.Property(e => e.NomSite).HasColumnName("nom_site").IsRequired().HasMaxLength(150);

                entity.HasMany(s => s.Fournisseurs)
                      .WithOne(f => f.Site)
                      .HasForeignKey(f => f.SiteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureResultatGlobalFournisseur(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ResultatGlobalFournisseur>(entity =>
            {
                entity.ToTable("v_ResultatGlobal_Fournisseur");
                entity.HasNoKey();
                entity.ToView("v_ResultatGlobal_Fournisseur");
            });
        }
    }
}