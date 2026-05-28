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
using System.Windows.Shapes;
using Labirintus;

namespace Faszomat
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        private List<Map> maps = new List<Map>();
        public StartView()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Ha nincs kiválasztva térkép, akkor nem lehet elindítani a játékot
            if (lbSaves.SelectedIndex == -1)
            {
                MessageBox.Show("Válassz mapot!");
                return;
            }

            Map selectedMap = maps[lbSaves.SelectedIndex];

            ((MainWindow)Application.Current.MainWindow)
                .ShowGameView(selectedMap);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtNameGiver.Text == "") {
                MessageBox.Show("Adj nevet a térképednek!");
                return;
            }
            Map map = new Map(txtNameGiver.Text);
            maps.Add(map);
            lbSaves.Items.Add(map.name);
        }
    }
}
