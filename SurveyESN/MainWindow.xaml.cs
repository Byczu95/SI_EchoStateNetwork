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
using LibraryESN;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace SurveyESN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public EchoStateNetwork esn;

        // Status
        public bool fileSelected;
        public String filePath;

        public MainWindow()
        {
            fileSelected = false;
            InitializeComponent();
            CheckStatus();
        }

        public void CheckStatus()
        {
            // Check if data file is selected
            if (fileSelected && esn != null)
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
                if (esn.isTeached == false)
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
            showMessageBox("Now will generate ESN" + Environment.NewLine + "You will be inform about end of process", "Start the process");
            File_New.IsEnabled = false;
            Thread th = new Thread(() => generateNewESN());
            th.Start();
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
            FileStream fs;
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                fs = new FileStream(filename, FileMode.Open);
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    //Deserialize the hashtable from the file and
                    //assign the reference to the local variable.
                    esn = (EchoStateNetwork)formatter.Deserialize(fs);
                    mseValue.Text = esn.mse.ToString();
                    reservoirValue.Text = esn.size.ToString();
                    if (esn.mse == 0) { mseValue.Text = ""; } else { mseValue.Text = esn.mse.ToString(); }  
                    leakValue.Text = esn.a.ToString(); 
                    if (esn.isTeached == true)
                    {
                        askBox.IsEnabled = true;
                        askButton.IsEnabled = true;
                        teach.IsEnabled = false;
                        initValue.IsEnabled = false;
                    }
                    else
                    {
                        askBox.IsEnabled = false;
                        askButton.IsEnabled = false;
                        teach.IsEnabled = true;
                        initValue.IsEnabled = true;
                        loadData.IsEnabled = true;
                    }
                    if(loadDataPath.Text.Equals("Nie wybrano pliku"))
                    {
                        teach.IsEnabled = false;
                        initValue.IsEnabled = false;
                    }
                }
                catch (Exception ex)
                {
                    showMessageBox("Failed to deserialize. Reason: " + ex.Message, "Error");
                    throw;
                }
                fs.Close();
            }
        }

        // Zapisanie obiektu sieci
        private void File_Save_Click(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "ESN  " + DateTime.Today.ToShortDateString(); // Default file name
            dlg.DefaultExt = ".esn"; // Default file extension
            dlg.Filter = "Esn file (.esn)|*.esn"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            FileStream fs;
            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                fs = new FileStream(filename, FileMode.Create);

                // Construct a BinaryFormatter and use it to serialize the data to the stream.
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    EchoStateNetwork temp = esn;
                    temp.data = null;
                    temp.Y = null;
                    temp.Yt = null;
                    temp.X = null;
                    formatter.Serialize(fs, temp);
                }
                catch (Exception ex)
                {
                    showMessageBox("Failed to serialize. Reason: " + ex.Message, "Error");
                    throw;
                }
                fs.Close();
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
            showMessageBox("Adam Matuszak" + Environment.NewLine + "Łukasz Knop" + Environment.NewLine + "Szymon Kaszuba", "Autors");
        }


        // Okienko z linkami do dokumentaji/git
        private void Document_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Tetrach121/SI_ESN");
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
                filePath = dlg.FileName;
                loadDataPath.Text = FileNameShow(dlg.FileName);// Wypisz ścieżkę w label loadDataPath
                fileSelected = true; // Jeśli nie ma wyjątków to fileSelected = true
            }

            CheckStatus();
        }

        private string FileNameShow(string s)
        {
            string[] sSplit = s.Split('\\');
            if(sSplit.Length > 5)
                return "...\\" + sSplit[sSplit.Length - 4] + '\\' + sSplit[sSplit.Length - 3] + sSplit[sSplit.Length - 2] + '\\' + sSplit[sSplit.Length - 1];
            if (sSplit.Length > 3)
                return "...\\" + sSplit[sSplit.Length - 2] + '\\' + sSplit[sSplit.Length - 1];
            return s;
        }

        // Wprowadź zapytanie
        private void askButton_Click(object sender, RoutedEventArgs e)
        {
            double ans = esn.Ask(double.Parse(askBox.Text));
            answer.Text = ans.ToString();
        }

        // Nauczaj sieć wczytanymi danymi
        private void teach_Click(object sender, RoutedEventArgs e)
        {
            esn.Learn(filePath, int.Parse(initValue.Text));
            mseValue.Text = esn.mse.ToString();
            MessageBox.Show(esn.mse.ToString());
            CheckStatus();
        }
        #endregion

        #region Logic

        public void generateNewESN()
        {
            esn = new EchoStateNetwork(1000, 0.3);

            reservoirValue.Dispatcher.Invoke(() => { reservoirValue.Text = "1000"; });
            leakValue.Dispatcher.Invoke(() => { leakValue.Text = "0.3";});
            File_New.Dispatcher.Invoke(() => { File_New.IsEnabled = true; });

            showMessageBox("Complete generate ESN", "Process completed");
        }

        static public void showMessageBox(String msg, String title = "Message Box")
        {
            MessageBox.Show(msg, title, MessageBoxButton.OK);
        }
        #endregion
    }
}
