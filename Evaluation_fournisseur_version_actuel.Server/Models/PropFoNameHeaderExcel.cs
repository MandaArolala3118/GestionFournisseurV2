using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*  Cette classe est juste utilisé pour l'entete du tableau Excel(Export) */
namespace Evaluation_Fournisseur.Models
{
    public class PropFoNameHeaderExcel
    {
        public int Identifiant { get; set; }
        public string Fournisseur { get; set; }
        public string Fabricant { get; set; }
        public string Categorie { get; set; }
        public string Nature { get; set; }

        public string Sites { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Notes3 { get; set; }
        public string Notes4 { get; set; }
        public string Notes5 { get; set; }
        public string Notes6 { get; set; }
        public string Notes7 { get; set; }
        public string Notes8 { get; set; }
        public string Notes9 { get; set; }
        public string Notes10 { get; set; }
        public string Notes11 { get; set; }
        public string Notes12 { get; set; }
        public string Notes13 { get; set; }

        public string Total { get; set; }
        public string Resultat { get; set; }
        public string Observation { get; set; }
        public string Proposition { get; set; }
        public string Responsable { get; set; }
        public string NomEvaluateur { get; set; }
        public string Email { get; set; }
    }
}

