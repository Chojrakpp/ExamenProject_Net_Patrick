using ExamenProject_Patrick.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProject_Patrick.Data
{
    internal static class Initializer
    {
        internal static void DbSetInitializer(MyDbContext context)
        {
            Gebruiker admin = null;
            if (!context.Gebruikers.Any())
            {
                admin = new Gebruiker { Naam = "admin", Email = "admin@admin.com", Wachtwoord = "admin" };
                context.Add(admin);
                context.SaveChanges();
            }

            if (admin == null)
                admin = context.Gebruikers.First(g => g.Naam == "admin");

            if (!context.Onderwerpen.Any())
            {
                context.Add(new Onderwerp { Naam = "testOnderwerp" });
                context.Add(new Onderwerp { Naam = "Banden veranderen"});
                context.Add(new Onderwerp { Naam = "Wiel balans" });
                context.Add(new Onderwerp { Naam = "Routine onderhoud" });
                context.Add(new Onderwerp { Naam = "Carrosserie" });
                context.SaveChanges();
            }

            Onderwerp dummyOnderwerp = context.Onderwerpen.First(c => c.Naam == "testOnderwerp");

            // Voeg een dummy afspraak toe bij het initialiseren van de database
            if (!context.Afspraken.Any())
            {
                Afspraak dummyAfspraakVerleden = new Afspraak
                {
                    DateTime = DateTime.Now.AddHours(-24), // Een uur in de toekomst
                    Gebruiker = admin,
                    Onderwerp = dummyOnderwerp
                };
                context.Afspraken.Add(dummyAfspraakVerleden);
            }

            if (!context.Afspraken.Any())
            {
                Afspraak dummyAfspraak = new Afspraak
                {
                    DateTime = DateTime.Now.AddHours(+48), // Een uur in de toekomst
                    Gebruiker = admin,
                    Onderwerp = dummyOnderwerp
                };
                context.Afspraken.Add(dummyAfspraak);
            }

            context.SaveChanges();
        }
    }
}
