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
            btnRoom.IsEnabled = false;
            btnSave.IsEnabled = false;

            GeneratePathSelector();
        }

        private void GeneratePathSelector()
        {
            char[] symbols = ['═', '╬', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔'];

            txtCurrent.Text = symbols[0].ToString();

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
            txtCurrent.Text = (e.Source as Button).Content.ToString();
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
            RoomCheck();
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

        private void RoomCheck()
        {
            if (txtRoomNum.Text != "" && txtRoomNum.Text != "0")
            {
                btnRoom.IsEnabled = true;
            }
            else
            {
                btnRoom.IsEnabled = false;
                txtCurrent.Text = "═";
            }
        }

        string[,] map;

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            grdGameField.Children.Clear();
            grdGameField.ColumnDefinitions.Clear();
            grdGameField.RowDefinitions.Clear();

            int row = Convert.ToInt32(txtHeight.Text);
            int col = Convert.ToInt32(txtWidth.Text);

            map = new string[row, col];

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
                    map[i, j] = ".";

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
            map[r, c] = btn.Content.ToString();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}