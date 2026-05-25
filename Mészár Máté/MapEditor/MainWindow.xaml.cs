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

        char[] symbols = ['═', '╬', '╦', '╩', '║', '╣', '╠', '╗', '╝', '╚', '╔'];

        public MainWindow()
        {
            InitializeComponent();

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
                btn.Click += ChangeSymbol;
                Grid.SetColumn(btn, i);
                grdPaths.Children.Add(btn);
            }
        }

        private void ChangeSymbol(object sender, RoutedEventArgs e)
        {
            txtCurrent.Text = sender.ToString().Substring(sender.ToString().Length - 1);
        }
    }
}