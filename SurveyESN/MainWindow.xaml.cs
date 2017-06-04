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
            }
            else
            {
                teach.IsEnabled = false;
            }

            // Check if ESN was teached
            try
            {
                if (esn.teached == false)
                {
                    askBox.IsEnabled = false;
                }
                else
                {
                    askBox.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                askBox.IsEnabled = false;
            }
        }

        #region Buttons

        // Utworzenie nowej sieci (okno z parametrami)
        private void File_New_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "File " + DateTime.Today.ToShortDateString(); // Default file name
            dlg.DefaultExt = ".esn"; // Default file extension
            dlg.Filter = "Esn file (.esn)|*.esn"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
            }
        }

        // Otwarcie zapisanej wcześniej sieci
        private void File_Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "File"; // Default file name
            dlg.DefaultExt = ".esn"; // Default file extension
            dlg.Filter = "ESN file (.esn)|*.esn"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
            }
        }

        // Zapisanie obiektu sieci
        private void File_Save_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "File " + DateTime.Today.ToShortDateString(); // Default file name
            dlg.DefaultExt = ".esn"; // Default file extension
            dlg.Filter = "Esn file (.esn)|*.esn"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
            }
        }

        // Zamknięcie programu
        private void File_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "File"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Txt file (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName; 
                loadDataPath.Text = dlg.FileName;// Wypisz ścieżkę w label loadDataPath
                fileSelected = true; // Jeśli nie ma wyjątków to fileSelected = true
            }

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
