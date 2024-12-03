namespace PAEL_V2.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Cours
    {
        public int CoursId { get; set; }

        [Required(ErrorMessage = "Le titre est requis.")]
        [StringLength(100, ErrorMessage = "Le titre ne doit pas dépasser 100 caractères.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "La description ne doit pas dépasser 500 caractères.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La date de début est requise.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La date de fin est requise.")]
        [DataType(DataType.Date)]
        //[DateGreaterThan("StartDate", ErrorMessage = "La date de fin doit être après la date de début.")]
        public DateTime EndDate { get; set; }

        public ICollection<Inscription> Inscriptions { get; set; }


    }
}



