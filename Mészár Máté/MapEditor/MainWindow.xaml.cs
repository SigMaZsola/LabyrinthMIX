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

            if (txt.Text.Length <= 1)
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
                txt.Text = txt.Text.First().ToString();
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
    }
}