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
            Gebruiker dummy = null;
            if (!context.Gebruikers.Any())
            {
                dummy = new Gebruiker { Naam = "-", Email = "-", Wachtwoord = "shit" };
                context.Add(dummy);
                context.SaveChanges();
            }

            if (dummy == null)
                dummy = context.Gebruikers.First(g => g.Naam == "-");

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
            if (!context.Afspraken.Any(a => a.Naam == "DummyVerleden"))
            {
                Afspraak dummyAfspraakVerleden = new Afspraak
                {
                    DateTime = DateTime.Now.AddHours(-24), // Een uur in de toekomst
                    Naam = "DummyVerleden",
                    Onderwerp = dummyOnderwerp
                };
                context.Afspraken.Add(dummyAfspraakVerleden);
            }

            if (!context.Afspraken.Any(a => a.Naam == "Dummy"))
            {
                Afspraak dummyAfspraak = new Afspraak
                {
                    DateTime = DateTime.Now.AddHours(+48), // Een uur in de toekomst
                    Naam = "Dummy",
                    Onderwerp = dummyOnderwerp
                };
                context.Afspraken.Add(dummyAfspraak);
            }

            context.SaveChanges();
        }
    }
}
