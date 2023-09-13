﻿using Microsoft.Win32;
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
            //char delimiter = GetSelectedDelimiter(); // TODO: Opravit Delimiter
            DataTable dataTable = new DataTable();

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                if (line != null)
                {
                    // TODO: Opravit Delimiter a headers:
                    //string[] headers = line.Split(delimiter); // TODO: Opravit Delimiter
                    //foreach (string header in headers) // TODO: Opravit headers
                    //{
                    //    dataTable.Columns.Add(header); // TODO: Opravit header
                    //}

                    while ((line = sr.ReadLine()) != null)
                    {
                        //dataTable.Rows.Add(line.Split(delimiter)); // TODO: Opravit Delimiter:
                    }
                }
            }

            dtGrdTabulkaCsvSouboru.ItemsSource = dataTable.DefaultView;
        }

        
    }
}
