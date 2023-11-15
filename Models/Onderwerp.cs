using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProject_Patrick.Models
{
    internal class Onderwerp
    {
        public int Id { get; set; }

        public string Naam { get; set; }

        public List<Afspraak> Afspraken { get; set; }
    }
}
