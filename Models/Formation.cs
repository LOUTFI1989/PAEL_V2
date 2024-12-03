namespace PAEL_V2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Formation
    {
        public int FormationId { get; set; }

        [Required(ErrorMessage = "Le nom de la formation est requis.")]
        [StringLength(100, ErrorMessage = "Le nom de la formation ne doit pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [StringLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La date de début est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; }

        [Required(ErrorMessage = "La date de fin est requise.")]
        [DataType(DataType.Date)]
      // [DateGreaterThan("DateDebut", ErrorMessage = "La date de fin doit être après la date de début.")]
        public DateTime DateFin { get; set; }

        public ICollection<Cours> Cours { get; set; }
        public ICollection<Etudiant> Etudiants { get; set; }
        public ICollection<Inscription> Inscriptions { get; set; }

    }

}


