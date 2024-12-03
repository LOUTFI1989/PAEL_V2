namespace PAEL_V2.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Inscription
    {
        public int InscriptionId { get; set; }

        [Required]
        public int FormationId { get; set; }
        public string FormationNom { get; set; } 
        public Formation Formation { get; set; }

        [Required]
        public int EtudiantId { get; set; }
        public Etudiant Etudiant { get; set; }
        public string EtudiantNom { get; set; }
        public string EtudiantPrenom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateInscription { get; set; } = DateTime.Now;
    }
}
