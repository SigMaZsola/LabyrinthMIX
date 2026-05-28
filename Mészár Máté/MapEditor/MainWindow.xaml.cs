using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            btnGenerate.IsEnabled = false;
            btnSave.IsEnabled = false;

            GeneratePathSelector();
        }

        private void GeneratePathSelector()
        {
            char[] symbols = ['═', '╬', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔'];

            for (int i = 0; i < symbols.Length; i++)
            {
                grdPaths.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < symbols.Length; i++)
            {
                Button btn = new Button();
                btn.Content = symbols[i];
                btn.FontSize = 30;
                btn.Click += ChangePath;
                btn.FontFamily = new FontFamily("Consolas");
                Grid.SetColumn(btn, i);
                grdPaths.Children.Add(btn);
            }
        }

        private void ChangePath(object sender, RoutedEventArgs e)
        {
            string btnText = (sender as Button).Content.ToString();
            txtCurrent.Text = btnText == "🗑️" ? "" : btnText;
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = e.Source as TextBox;

            if (txt.Text.Length <= 2)
            {
                try
                {
                    Convert.ToInt32(txt.Text);
                }
                catch (Exception ex)
                {
                    txt.Text = "";
                }
            }
            else
            {
                txt.Text = "";
            }

            GenCheck();
        }

        private void GenCheck()
        {
            if (txtHeight.Text != "" && txtWidth.Text != "" && Convert.ToInt32(txtHeight.Text) >= 3 && Convert.ToInt32(txtWidth.Text) >= 3)
            {
                btnGenerate.IsEnabled = true;
            }
            else
            {
                btnGenerate.IsEnabled = false;
            }
        }

        Labyrinth lab;
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            grdGameField.Children.Clear();
            grdGameField.ColumnDefinitions.Clear();
            grdGameField.RowDefinitions.Clear();

            int row = Convert.ToInt32(txtHeight.Text);
            int col = Convert.ToInt32(txtWidth.Text);

            lab = new Labyrinth(row, col);

            for (int i = 0; i < row; i++)
            {
                grdGameField.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < col; i++)
            {
                grdGameField.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Button btn = new Button();
                    btn.Background = Brushes.Beige;
                    btn.FontFamily = new FontFamily("Consolas");
                    btn.BorderBrush = new SolidColorBrush(Colors.Black);
                    btn.FontSize = 20;
                    btn.Click += PathPlace;
                    btn.Tag = (i, j);

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                    grdGameField.Children.Add(btn);
                }
            }
            btnSave.IsEnabled = true;
        }
        private void PathPlace(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not (int r, int c)) return;

            btn.Content = txtCurrent.Text;
            lab.AddPath(btn.Content.ToString(), r, c);

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Szöveges dokumentum (*.txt)|*.txt";
            if (sfd.ShowDialog() == false) return;

            File.WriteAllLines(sfd.FileName, lab.ToStringRows());
        }

        private void rbEn_Checked(object sender, RoutedEventArgs e)
        {
            txtBHeight.Text = "Height: ";
            txtBWidth.Text = "Width: ";
            btnGenerate.Content = "Generate";
            btnSave.Content = "Save";
        }

        private void rbHu_Checked(object sender, RoutedEventArgs e)
        {
            if (txtBHeight is null) return;

            txtBHeight.Text = "Szélesség: ";
            txtBWidth.Text = "Magasság: ";
            btnGenerate.Content = "Generálás";
            btnSave.Content = "Mentés";
        }
    }
}