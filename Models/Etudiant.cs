namespace PAEL_V2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Etudiant
    {
        public int EtudiantId { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [StringLength(50, ErrorMessage = "Le nom ne doit pas dépasser 50 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [StringLength(50, ErrorMessage = "Le prénom ne doit pas dépasser 50 caractères.")]
        public string Prenom { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Adresse e-mail non valide.")]
        [StringLength(100, ErrorMessage = "L'email ne doit pas dépasser 100 caractères.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La date d'inscription est requise.")]
        [DataType(DataType.Date)]
        public DateTime DateInscription { get; set; }

        public ICollection<Inscription> Inscriptions { get; set; }

    }
}
