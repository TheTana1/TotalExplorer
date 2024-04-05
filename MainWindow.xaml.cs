using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace TotalExplorer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string PathLeft;
        uint countCorrect;
        List<string> FindPaths;
        public MainWindow()
        {
            InitializeComponent();
            FindPaths = new List<string>();
            countCorrect = 0;
            PathLeft = tb_left.Text;
            ShowFilesListView(listviewleft, PathLeft);
            ShowFilesListView(listviewright, PathLeft);



        }

        public void ShowFilesListView(ListView listView, string path)
        {
            listView.Items.Clear();
            listView.Items.Add(Directory.GetParent(PathLeft));
            foreach (string dir in Directory.GetDirectories(path))
            {
                listView.Items.Add(dir);

            }
            foreach (string file in Directory.GetFiles(path))
            {
                listView.Items.Add(file);
            }


        }



        private void listviewleft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedIndex == 0)
            {
                PathLeft = Directory.GetParent(PathLeft).FullName;
            }
            else if (File.Exists((sender as ListView).SelectedItem.ToString()))
            {
                Process.Start((sender as ListView).SelectedItem.ToString());
            }
            else PathLeft = (sender as ListView).SelectedItem as string;
            ShowFilesListView((sender as ListView), PathLeft);
        }

        private async void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                listviewleft.Items.Clear();
                await Task.Run(() =>
                {
                    DirectoryEnum(PathLeft);
                });

                tb_left.Focus();

            }

        }


        void DirectoryEnum(string path)
        {
            try
            {
                {
                    foreach (string f in Directory.GetFiles(path, $"{tb_Search.Text}.*", SearchOption.AllDirectories))
                    {
                        countCorrect++;
                        FindPaths.Add(f);
                    }
                }
                foreach (string directory in Directory.GetDirectories(path, $"{tb_Search.Text}", SearchOption.AllDirectories))
                {
                    countCorrect++;
                    FindPaths.Add(directory);
                }

                if (countCorrect == 0) throw new Exception("Нет такого файла/папки");

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

        }

        private void tb_left_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PathLeft = (sender as TextBox).Text;
                ShowFilesListView(listviewleft, PathLeft);
            }
        }

    }
}


