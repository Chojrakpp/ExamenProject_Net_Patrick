using ExamenProject_Patrick.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ExamenProject_Patrick
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static MyDbContext context = null;
        internal static MainWindow mainWindow = null;
        internal static Gebruiker gebruiker = null;
    }
}
