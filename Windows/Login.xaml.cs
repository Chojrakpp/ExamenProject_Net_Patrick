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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btAanmelden_Click(object sender, RoutedEventArgs e)
        {
            Gebruiker? gebruiker = null;
            try
            {
                gebruiker = App.context.Gebruikers.First(g => g.Email == tbEmail.Text && g.Wachtwoord == pbWachtwoord.Password);
            }
            catch
            {
                tbMededeling.Text = "Foutive aanmeldingspoging";

                tbMededeling.Visibility = Visibility.Visible;
            }
            if (gebruiker != null) // als gebruiker verschillend is dan null, aanmelden
            {
                App.gebruiker = gebruiker;
                App.mainWindow.main.Visibility = Visibility.Visible;
                App.mainWindow.miAangemeld.Text = gebruiker.Naam;
                App.mainWindow.miAfmelden.Visibility = Visibility.Visible;
                App.mainWindow.miRegistreren.Visibility = Visibility.Collapsed;
                App.mainWindow.miLogin.Visibility = Visibility.Collapsed;

                if (gebruiker.Naam == "admin") {
                    App.mainWindow.adminDashboard.Visibility = Visibility.Visible;
                }

                App.mainWindow.AfsprakenVerwacht();
                App.mainWindow.AfsprakenVerleden();
                this.Close();
            }
        }
    }
}
