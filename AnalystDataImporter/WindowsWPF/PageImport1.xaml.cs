using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace AnalystDataImporter.WindowsWPF
{
    /// <summary>
    /// Interakční logika pro PageImport1.xaml
    /// </summary>
    public partial class PageImport1 : Page
    {
        public PageImport1()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                txtFilePath.Text = openFileDialog.FileName;
                LoadFile(openFileDialog.FileName);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            string filePath = txtFilePath.Text;
            // Process the file using the specified mappings and perform the import.
            MessageBox.Show("Import complete!");
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && (files[0].EndsWith(".txt") || files[0].EndsWith(".csv")))
                {
                    e.Effects = DragDropEffects.Copy;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
            }

            e.Handled = true;
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && (files[0].EndsWith(".txt") || files[0].EndsWith(".csv")))
                {
                    txtFilePath.Text = files[0];
                    LoadFile(files[0]);
                }
            }
        }

        private void LoadFile(string filePath)
        {
            char delimiter = GetSelectedDelimiter();
            DataTable dataTable = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                if (line != null)
                {
                    string[] headers = line.Split(delimiter);
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header);
                    }

                    while ((line = sr.ReadLine()) != null)
                    {
                        dataTable.Rows.Add(line.Split(delimiter));
                    }
                }
            }

            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private char GetSelectedDelimiter()
        {
            switch (cbxDelimiter.SelectedItem)
            {
                case ComboBoxItem item when item.Content.ToString() == "Tab":
                    return '\t';
                case ComboBoxItem item when item.Content.ToString() == "Semicolon":
                    return ';';
                default: // Default to comma
                    return ',';
            }
        }
    }
}
