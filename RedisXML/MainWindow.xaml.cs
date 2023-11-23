using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;

namespace gender_bender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public string? FilepathTextToEdit;
        public string[]? FileLines;
        public XDocument? ConvertedFile;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RedisToXMLBtn_Click(object sender, RoutedEventArgs e)
        {
            if(FileLines is null)
            {
                MessageBox.Show("Brak załączonego pliku!");
                return;
            }

            var (IsGood, message) = RedisToXML.CheckFileCompatibility(FileLines);

            if (IsGood == true)
            {
                ConvertedFile = RedisToXML.ConvertRedisTXT_To_XMLTXT(FileLines);
                UpdateControls();
            }
            else
            {
                MessageBox.Show(message);
            }

        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[]? droppedFile = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                try
                {
                    droppedFile = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Błąd - " + ex.Message);
                }
            }

            if(droppedFile is null)
            {
                MessageBox.Show("Błąd pliku");
                return;
            }
            else
            {
                FileLines = null;
                ConvertedFile = null;
            }

            FilepathTextToEdit = droppedFile[0];

            FileLines = ReadFile(FilepathTextToEdit);

            UpdateControls();
        }

        private void UpdateControls()
        {
            if(ConvertedFile is not null)
            {
                XMLFilePreviewTb.Text = string.Join("\n", ConvertedFile);
            }

            if(FileLines is not null)
            {
                FilePreviewTb.Text = string.Join("\n", FileLines);

                string[] s = FilepathTextToEdit.Split("\\");
                FileStatusLb.Content = $"Załączony plik: {s[s.Length - 1]}";
            }
        }

        public void ClearControls()
        {
            XMLFilePreviewTb.Text = string.Empty;
            FilePreviewTb.Text = string.Empty;
            FileStatusLb.Content = "Załączony plik: Brak";
        }

        public string[]? ReadFile(string? filepath)
        {
            if (filepath is null) return null;
            string[] tmp = File.ReadAllLines(filepath);
            tmp = tmp.Where(item => item != string.Empty && item != "\n" && item != "" && item.Length > 0).ToArray();
            return tmp;
        }

        private void AttachFileRB_Click(object sender, RoutedEventArgs e)
        {
            (string[]? lines, string? filepath) = RedisToXML.Open();

            if(lines is not null && filepath is not null)
            {
                FileLines = lines;
                FilepathTextToEdit = filepath;

                UpdateControls();
            }
            else
            {
                MessageBox.Show("Błąd podczas załączania pliku");
            }
        }

        private void UnAttachFileBtn_Click(object sender, RoutedEventArgs e)
        {
            FilepathTextToEdit = null;
            ConvertedFile = null;
            FileLines = null;
            ClearControls();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                if (FilepathTextToEdit == null || ConvertedFile == null || FileLines == null) return;

                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.Title = "XML File save";

                saveFileDialog.Filter = "xml file (*.xml)|*.xml";

                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                bool? b = saveFileDialog.ShowDialog();

                string output;
                if (b == true)
                {
                    ConvertedFile.Save(saveFileDialog.FileName);
                    output = "Pomyślnie zapisano plik -> " + saveFileDialog.FileName;
                }
                else if (b == false)
                    output = "Anulowano zapisywanie pliku!";
                else
                    output = "Błąd w zapisywaniu pliku";
                MessageBox.Show(output);
            }
            );
        }
    }
}
