using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation_fournisseur_version_actuel.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreationBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eval_administrateur",
                columns: table => new
                {
                    id_administrateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    identite_utilisateur = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_administrateur", x => x.id_administrateur);
                });

            migrationBuilder.CreateTable(
                name: "eval_campagne",
                columns: table => new
                {
                    id_campagne = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    annee = table.Column<short>(type: "smallint", nullable: false),
                    nom_campagne = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_campagne", x => x.id_campagne);
                });

            migrationBuilder.CreateTable(
                name: "eval_categorie",
                columns: table => new
                {
                    id_categorie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_categorie = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_categorie", x => x.id_categorie);
                });

            migrationBuilder.CreateTable(
                name: "eval_fabricant",
                columns: table => new
                {
                    id_fabricant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_fabricant = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_fabricant", x => x.id_fabricant);
                });

            migrationBuilder.CreateTable(
                name: "eval_nature",
                columns: table => new
                {
                    id_nature = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    libel_nature = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_nature", x => x.id_nature);
                });

            migrationBuilder.CreateTable(
                name: "eval_resultat",
                columns: table => new
                {
                    id_resultat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_resultat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    seuil_min = table.Column<double>(type: "float", nullable: false),
                    seuil_max = table.Column<double>(type: "float", nullable: false),
                    observation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_resultat", x => x.id_resultat);
                });

            migrationBuilder.CreateTable(
                name: "eval_site",
                columns: table => new
                {
                    id_site = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_site = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_site", x => x.id_site);
                });

            migrationBuilder.CreateTable(
                name: "v_ResultatGlobal_Fournisseur",
                columns: table => new
                {
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    VendorNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Annee = table.Column<short>(type: "smallint", nullable: false),
                    NomCampagne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NbreEvaluations = table.Column<int>(type: "int", nullable: false),
                    MoyenneGlobalePct = table.Column<double>(type: "float", nullable: true),
                    ResultatGlobal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResultatGlobalCalcule = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "eval_nom_fournisseur",
                columns: table => new
                {
                    id_nom_fournisseur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vendor_number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    vendor_name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    company_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ap_division = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    str_class_status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sys_code_description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    signification_companycode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    signification_apdivision = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    str_vendor_class = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    credit_terms_code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    id_campagne = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_nom_fournisseur", x => x.id_nom_fournisseur);
                    table.ForeignKey(
                        name: "FK_eval_nom_fournisseur_eval_campagne_id_campagne",
                        column: x => x.id_campagne,
                        principalTable: "eval_campagne",
                        principalColumn: "id_campagne",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "eval_ponderation",
                columns: table => new
                {
                    id_ponderation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_categorie = table.Column<int>(type: "int", nullable: false),
                    I_transparence = table.Column<double>(type: "float", nullable: false),
                    II_facture = table.Column<double>(type: "float", nullable: false),
                    III_notoriete = table.Column<double>(type: "float", nullable: false),
                    IV_prevention_corruption = table.Column<double>(type: "float", nullable: false),
                    V_prevention_droit_homme = table.Column<double>(type: "float", nullable: false),
                    VI_conformite_vs_exigence = table.Column<double>(type: "float", nullable: false),
                    VII_delai_livraison = table.Column<double>(type: "float", nullable: false),
                    VIII_communication = table.Column<double>(type: "float", nullable: false),
                    IX_disponibilite_documents = table.Column<double>(type: "float", nullable: false),
                    X_consigne_QHSE = table.Column<double>(type: "float", nullable: false),
                    XI_qualite_prix = table.Column<double>(type: "float", nullable: false),
                    XII_duree_paiement = table.Column<double>(type: "float", nullable: false),
                    XIII_conformite_fiscale = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_ponderation", x => x.id_ponderation);
                    table.ForeignKey(
                        name: "FK_eval_ponderation_eval_categorie_id_categorie",
                        column: x => x.id_categorie,
                        principalTable: "eval_categorie",
                        principalColumn: "id_categorie",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "eval_fournisseur",
                columns: table => new
                {
                    id_fournisseur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_nom_fournisseur = table.Column<int>(type: "int", nullable: false),
                    id_fabricant = table.Column<int>(type: "int", nullable: true),
                    id_categorie = table.Column<int>(type: "int", nullable: false),
                    id_nature = table.Column<int>(type: "int", nullable: false),
                    id_site = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    emailEnvoye = table.Column<bool>(type: "bit", nullable: false),
                    dateEnvoiEmail = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_fournisseur", x => x.id_fournisseur);
                    table.ForeignKey(
                        name: "FK_eval_fournisseur_eval_categorie_id_categorie",
                        column: x => x.id_categorie,
                        principalTable: "eval_categorie",
                        principalColumn: "id_categorie",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_fournisseur_eval_fabricant_id_fabricant",
                        column: x => x.id_fabricant,
                        principalTable: "eval_fabricant",
                        principalColumn: "id_fabricant",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_fournisseur_eval_nature_id_nature",
                        column: x => x.id_nature,
                        principalTable: "eval_nature",
                        principalColumn: "id_nature",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_fournisseur_eval_nom_fournisseur_id_nom_fournisseur",
                        column: x => x.id_nom_fournisseur,
                        principalTable: "eval_nom_fournisseur",
                        principalColumn: "id_nom_fournisseur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_fournisseur_eval_site_id_site",
                        column: x => x.id_site,
                        principalTable: "eval_site",
                        principalColumn: "id_site",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "eval_evaluation",
                columns: table => new
                {
                    id_evaluation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_fournisseur = table.Column<int>(type: "int", nullable: false),
                    id_ponderation = table.Column<int>(type: "int", nullable: false),
                    id_campagne = table.Column<int>(type: "int", nullable: false),
                    id_resultat = table.Column<int>(type: "int", nullable: true),
                    I_transparence = table.Column<double>(type: "float", nullable: true),
                    II_facture = table.Column<double>(type: "float", nullable: true),
                    III_notoriete = table.Column<double>(type: "float", nullable: true),
                    IV_prevention_corruption = table.Column<double>(type: "float", nullable: true),
                    V_prevention_droit_homme = table.Column<double>(type: "float", nullable: true),
                    VI_conformite_vs_exigence = table.Column<double>(type: "float", nullable: true),
                    VII_delai_livraison = table.Column<double>(type: "float", nullable: true),
                    VIII_communication = table.Column<double>(type: "float", nullable: true),
                    IX_disponibilite_documents = table.Column<double>(type: "float", nullable: true),
                    X_consigne_QHSE = table.Column<double>(type: "float", nullable: true),
                    XI_qualite_prix = table.Column<double>(type: "float", nullable: true),
                    XII_duree_paiement = table.Column<double>(type: "float", nullable: true),
                    XIII_conformite_fiscale = table.Column<double>(type: "float", nullable: true),
                    moyenne_pct = table.Column<double>(type: "float", nullable: true),
                    observation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    proposition_amelioration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    observations_fournisseur = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    responsable = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    numero_action_pac = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    deadline = table.Column<DateOnly>(type: "date", nullable: true),
                    date_evaluation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eval_evaluation", x => x.id_evaluation);
                    table.ForeignKey(
                        name: "FK_eval_evaluation_eval_campagne_id_campagne",
                        column: x => x.id_campagne,
                        principalTable: "eval_campagne",
                        principalColumn: "id_campagne",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_evaluation_eval_fournisseur_id_fournisseur",
                        column: x => x.id_fournisseur,
                        principalTable: "eval_fournisseur",
                        principalColumn: "id_fournisseur",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_evaluation_eval_ponderation_id_ponderation",
                        column: x => x.id_ponderation,
                        principalTable: "eval_ponderation",
                        principalColumn: "id_ponderation",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_eval_evaluation_eval_resultat_id_resultat",
                        column: x => x.id_resultat,
                        principalTable: "eval_resultat",
                        principalColumn: "id_resultat",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eval_evaluation_id_campagne",
                table: "eval_evaluation",
                column: "id_campagne");

            migrationBuilder.CreateIndex(
                name: "IX_eval_evaluation_id_fournisseur",
                table: "eval_evaluation",
                column: "id_fournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_eval_evaluation_id_ponderation",
                table: "eval_evaluation",
                column: "id_ponderation");

            migrationBuilder.CreateIndex(
                name: "IX_eval_evaluation_id_resultat",
                table: "eval_evaluation",
                column: "id_resultat");

            migrationBuilder.CreateIndex(
                name: "IX_eval_fournisseur_id_categorie",
                table: "eval_fournisseur",
                column: "id_categorie");

            migrationBuilder.CreateIndex(
                name: "IX_eval_fournisseur_id_fabricant",
                table: "eval_fournisseur",
                column: "id_fabricant");

            migrationBuilder.CreateIndex(
                name: "IX_eval_fournisseur_id_nature",
                table: "eval_fournisseur",
                column: "id_nature");

            migrationBuilder.CreateIndex(
                name: "IX_eval_fournisseur_id_nom_fournisseur",
                table: "eval_fournisseur",
                column: "id_nom_fournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_eval_fournisseur_id_site",
                table: "eval_fournisseur",
                column: "id_site");

            migrationBuilder.CreateIndex(
                name: "IX_eval_nom_fournisseur_id_campagne",
                table: "eval_nom_fournisseur",
                column: "id_campagne");

            migrationBuilder.CreateIndex(
                name: "IX_eval_ponderation_id_categorie",
                table: "eval_ponderation",
                column: "id_categorie",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eval_administrateur");

            migrationBuilder.DropTable(
                name: "eval_evaluation");

            migrationBuilder.DropTable(
                name: "v_ResultatGlobal_Fournisseur");

            migrationBuilder.DropTable(
                name: "eval_fournisseur");

            migrationBuilder.DropTable(
                name: "eval_ponderation");

            migrationBuilder.DropTable(
                name: "eval_resultat");

            migrationBuilder.DropTable(
                name: "eval_fabricant");

            migrationBuilder.DropTable(
                name: "eval_nature");

            migrationBuilder.DropTable(
                name: "eval_nom_fournisseur");

            migrationBuilder.DropTable(
                name: "eval_site");

            migrationBuilder.DropTable(
                name: "eval_categorie");

            migrationBuilder.DropTable(
                name: "eval_campagne");
        }
    }
}
