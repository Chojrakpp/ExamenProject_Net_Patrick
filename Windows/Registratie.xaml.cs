using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ExamenProject_Patrick.Models;

namespace ExamenProject_Patrick.Windows
{
    /// <summary>
    /// Interaction logic for Registratie.xaml
    /// </summary>
    public partial class Registratie : Window
    {
        public Registratie()
        {
            InitializeComponent();
        }

        private void btRegistreren_Click(object sender, RoutedEventArgs e)
        {
            if (pbWachtwoord.Password == "" || tbNaam.Text == "" || tbEmail.Text == "")
            {
                tbMededeling.Text = "Alle velden moeten ingevuld zijn";
            }
            else if (pbWachtwoord.Password != pbHerhaling.Password)
            {
                tbMededeling.Text = "Geef 2 maal hetzelfde wachtwoord in";
            }
            else
            {
                Gebruiker? gebruiker = null;
                try
                {
                    gebruiker = App.context.Gebruikers.First(g => g.Naam == tbNaam.Text);
                }
                catch
                {
                    gebruiker = new Gebruiker { Naam = tbNaam.Text, Email = tbEmail.Text, Wachtwoord = pbWachtwoord.Password };
                    App.context.Add(gebruiker);
                    App.context.SaveChanges();
                    this.Close();
                }
                tbMededeling.Text = "Deze gebruiker bestaat al, geef een andere gebruikersnaam in";
            }
            tbMededeling.Visibility = Visibility.Visible;
        }
    }
}
