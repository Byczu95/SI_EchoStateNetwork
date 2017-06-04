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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SurveyESN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LibraryESN.EchoStateNetwork esn;

        // Status
        public bool fileSelected;

        public MainWindow()
        {
            fileSelected = false;
            InitializeComponent();
            CheckStatus();
        }

        public void CheckStatus()
        {
            // Check if data file is selected
            if (fileSelected)
            {
                teach.IsEnabled = true;
                initValue.IsEnabled = true;
            }
            else
            {
                teach.IsEnabled = false;
                initValue.IsEnabled = false;
            }

            // Check if ESN was teached
            try
            {
                if (esn.teached == false)
                {
                    askBox.IsEnabled = false;
                    askButton.IsEnabled = false;
                }
                else
                {
                    askBox.IsEnabled = true;
                    askButton.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                askBox.IsEnabled = false;
                askButton.IsEnabled = false;
            }
        }

        #region Buttons

        // Utworzenie nowej sieci (okno z parametrami)
        private void File_New_Click(object sender, RoutedEventArgs e)
        {

        }

        // Otwarcie zapisanej wcześniej sieci
        private void File_Open_Click(object sender, RoutedEventArgs e)
        {

        }

        // Zapisanie obiektu sieci
        private void File_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        // Zamknięcie programu
        private void File_Exit_Click(object sender, RoutedEventArgs e)
        {

        }


        // Okno z nami
        private void Autors_Click(object sender, RoutedEventArgs e)
        {

        }


        // Okienko z linkami do dokumentaji/git
        private void Document_Click(object sender, RoutedEventArgs e)
        {

        }

        // Wczytanie danych do nauki sieci
        private void loadData_Click(object sender, RoutedEventArgs e)
        {
            // Wczytaj dane
            // Wypisz ścieżkę w label loadDataPath
            // Jeśli nie ma tyjątków to fileSelected = true
            CheckStatus();
        }

        // Wprowadź zapytanie
        private void askButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Nauczaj sieć wczytanymi danymi
        private void teach_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


    }
}
