using ExamenProject_Patrick.Data;
using ExamenProject_Patrick.Models;
using ExamenProject_Patrick.Windows;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExamenProject_Patrick
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        MyDbContext context = new MyDbContext();
        List<Afspraak> afsprakenLijst = null;
        List<Afspraak> afsprakenVerledenLijst = null;
        Onderwerp selectedOnderwerp = null;
        List<Onderwerp> onderwerpen = null;
        public MainWindow()
        {
            Initializer.DbSetInitializer(context);
            InitializeComponent();
            InitializeHourComboBox();

            AfsprakenVerwacht();
            AfsprakenVerleden();

            onderwerpen = context.Onderwerpen.ToList();
            onderwerpenDataGrid.ItemsSource = onderwerpen
                .Where(o => o.Naam != null).ToList();
            onderwerpComboBox.ItemsSource = onderwerpen
                .Where(o => o.Naam != null).ToList();

            App.context = context;
        }

        public void AfsprakenVerwacht()
        {
            afsprakenLijst = context.Afspraken
                .Include(a => a.Onderwerp)
                .Where(c => c.DateTime >= DateTime.Now)
                .OrderBy(c => c.DateTime)
                .ToList();

            // Vul de OnderwerpNaam in voor elke afspraak
            foreach (var afspraak in afsprakenLijst)
            {
                afspraak.OnderwerpNaam = afspraak.Onderwerp?.Naam;
            }

            afspraakGrid.ItemsSource = afsprakenLijst;
        }

        public void AfsprakenVerleden()
        {
            afsprakenVerledenLijst = context.Afspraken
                .Include(a => a.Onderwerp)
                .Where(c => c.DateTime.Day < DateTime.Now.Day)
                .OrderByDescending(c => c.DateTime)
                .ToList();

            foreach (var afspraak in afsprakenVerledenLijst)
            {
                afspraak.OnderwerpNaam = afspraak.Onderwerp?.Naam;
            }

            afspraakVerledenGrid.ItemsSource = afsprakenVerledenLijst;
        }

        private void InitializeHourComboBox()
        {
            UpdateHourComboBox();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateHourComboBox();
            InitializeHourComboBox();

        }

        private void UpdateHourComboBox()
        {
            ClearMessage();
            if (datePicker.SelectedDate.HasValue && uurComboBox != null)
            {
                DateTime selectedDate = datePicker.SelectedDate.Value;
                DateTime now = DateTime.Now;

                // Haal de geboekte uren op voor de geselecteerde datum
                var geboekteUren = context.Afspraken
                    .Where(a => a.DateTime.Date == selectedDate.Date)
                    .Select(a => a.DateTime.Hour)
                    .ToList();

                // Controleer of de geselecteerde datum in de toekomst ligt
                if (selectedDate.Date > now.Date)
                {
                    uurComboBox.Items.Clear();

                    // Toon alleen uren die nog niet zijn geboekt
                    for (int hour = 9; hour < 17; hour++)
                    {
                        if (!geboekteUren.Contains(hour))
                        {
                            uurComboBox.Items.Add($"{hour}:00");
                        }
                    }
                }
                else if (selectedDate.Date == now.Date) 
                {
                    uurComboBox.Items.Clear();

                    // Toon uren van now.Hour tot 17 uur, afhankelijk van de huidige tijd
                    int startUur = (now.Hour >= 9) ? now.Hour+1 : 9;

                    for (int hour = startUur; hour < 17; hour++)
                    {
                        if (!geboekteUren.Contains(hour))
                        {
                            uurComboBox.Items.Add($"{hour}:00");
                        }
                    }
                } else
                {
                    // een datum in het verleden
                    uurComboBox.Items.Clear();
                    uurComboBox.Items.Add("Geen beschikbare uren");
                }
            }
        }

        private void AddAfspraak_Click(object sender, RoutedEventArgs e)
        {
            ClearMessage();
            try
            {
                DateTime selectedDate = datePicker.SelectedDate ?? DateTime.Now;

                // Controleer of uurComboBox leeg is of null
                if (uurComboBox.SelectedItem == null)
                {
                    ShowMessage("Selecteer een uur voordat je de afspraak toevoegt.");
                    return;
                }

                string selectedHour = uurComboBox.SelectedItem.ToString();

                if (selectedDate != null && !string.IsNullOrEmpty(selectedHour))
                {
                    // Controleer of selectedHour al aanwezig is in afsprakenLijst
                    if (afsprakenLijst.Any(a => a.DateTime.Date == selectedDate.Date && a.DateTime.Hour == int.Parse(selectedHour.Split(':')[0])))
                    {
                        ShowMessage("Er is al een afspraak op geselecteerde datum en uur. Kies een andere datum of uur.");
                        return;
                    }

                    DateTime afspraakDateTime = selectedDate.Date.AddHours(int.Parse(selectedHour.Split(':')[0]));

                    // Verkrijg de geselecteerde waarde van de ComboBox als een Onderwerp
                    Onderwerp selectedOnderwerp = onderwerpComboBox.SelectedItem as Onderwerp;

                    if (selectedOnderwerp == null) // voeg product toe
                    {
                        ShowMessage("Selecteer een onderwerp voordat je de afspraak toevoegt.");
                        return;
                    }

                    Afspraak afspraak = new Afspraak
                    {
                        DateTime = afspraakDateTime,
                        Naam = "test",
                        Onderwerp = selectedOnderwerp // Gebruik de Naam in plaats van het hele Onderwerp-object
                    };
                    context.Add(afspraak);
                    context.SaveChanges();

                    // Haal de afspraken op uit de database voor vandaag en de toekomst, en ordineer ze op datum
                    AfsprakenVerwacht();

                    // Werk de DataGrid bij met de vernieuwde lijst
                    afspraakGrid.ItemsSource = afsprakenLijst;
                }
            }
            catch
            {
                ShowMessage("Er is iets misgelopen, kijk of je een juiste uur en onderwerp heb gekozen");
            }
        }

        private void btBewaarOnderwerp_Click(object sender, RoutedEventArgs e)
        {
            /* Deze event handler wordt zowel gebruikt voor het toevoegen van een nieuw product, 
             * als voor het bewaren van de wijzigingen aan een bestaand product */

            ClearMessage();
            try
            {
                // Controleer of de invoer leeg is
                if (string.IsNullOrWhiteSpace(tbOnderwerpNaam.Text))
                {
                    ShowMessage("Voer een geldige naam in voor het onderwerp.");
                    return;
                }

                // Controleer of het onderwerp al bestaat in de database
                var bestaandOnderwerp = context.Onderwerpen.FirstOrDefault(o => o.Naam == tbOnderwerpNaam.Text);

                if (selectedOnderwerp == null) // Voeg onderwerp toe
                {
                    if (bestaandOnderwerp != null)
                    {
                        ShowMessage("Dit onderwerp bestaat al.");
                        return;
                    }

                    Onderwerp onderwerp = new Onderwerp
                    {
                        Naam = tbOnderwerpNaam.Text
                    };
                    context.Onderwerpen.Add(onderwerp);
                }
                else // Wijzig bestaand onderwerp
                {
                    if (bestaandOnderwerp != null && bestaandOnderwerp.Id != selectedOnderwerp.Id)
                    {
                        ShowMessage("Dit onderwerp bestaat al.");
                        return;
                    }

                    selectedOnderwerp.Naam = tbOnderwerpNaam.Text;
                    context.Update(selectedOnderwerp);
                }

                context.SaveChanges();
                onderwerpen = context.Onderwerpen.ToList(); // Haal alle onderwerpen opnieuw op
                onderwerpenDataGrid.ItemsSource = onderwerpen;
            }
            catch
            {
                ShowMessage("Voer een onderwerp in de textbox in.");
            }
        }

        private void miLogin_Click(object sender, RoutedEventArgs e)
        {
            new Login().Show();
        }
        private void miRegistreren_Click(object sender, RoutedEventArgs e)
        {
            new Registratie().Show();
        }

        private void miAfmelden_Click(object sender, RoutedEventArgs e) // zal niet zerken als geen referentie in app
        {
            App.mainWindow.miAfmelden.Visibility = Visibility.Collapsed;
            App.mainWindow.miRegistreren.Visibility = Visibility.Collapsed;
            App.mainWindow.miLogin.Visibility = Visibility.Collapsed;
            this.Close();
        }

        private void ClearMessage()
        {
            // toon een foutboodschap

            tbMessage.Visibility = Visibility.Collapsed;
        }

        private void ShowMessage(string message, bool serious = true)
        {
            tbMessage.Text = message;
            tbMessage.Background = serious ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);
            tbMessage.Height = 30;
            tbMessage.FontWeight = FontWeights.Bold;
            tbMessage.FontSize = 13;
            tbMessage.VerticalAlignment = VerticalAlignment.Center;
            tbMessage.Visibility = Visibility.Visible;
        }
    }
}
