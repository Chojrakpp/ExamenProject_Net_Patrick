using ExamenProject_Patrick.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProject_Patrick
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Afspraak> Afspraken { get; set; }

        public DbSet<Onderwerp> Onderwerpen { get; set; }

        public DbSet<Gebruiker> Gebruikers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-RS1U0QK\SQLEXPRESS;Database=WPF_ExamenProject_Kalender;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true");
        }
    }
}
