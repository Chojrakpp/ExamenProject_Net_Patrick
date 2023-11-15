using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProject_Patrick.Models
{
    internal class Afspraak
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        public string Naam { get; set; }

        public Onderwerp Onderwerp { get; set; }
        [ForeignKey("Onderwerpen")]
        public int OnderwerpId { get; set; }

        [NotMapped]
        public string OnderwerpNaam { get; set; }
    }
}
