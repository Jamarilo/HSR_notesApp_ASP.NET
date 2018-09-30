using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoteApp.Models
{
    public class Note
    {

        public static Note CreateNew()
        {
            Note newNote = new Note();
            newNote.ID = 0;
            newNote.CreatedDate = DateTime.Now;
            newNote.FinishDate = DateTime.Now;
            newNote.Importance = 1;
            return newNote;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [Range(1, 5)]
        public int Importance { get; set; }

        [Required]
        [Display(Name = "Finish Date")]
        [DataType(DataType.Date)]
        public DateTime FinishDate { get; set; }

        public bool Finished { get; set; }
    }
}
