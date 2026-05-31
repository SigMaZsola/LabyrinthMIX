using System;
using System.Windows;
using System.Windows.Controls;
using Labirintus;

namespace Faszomat
{
    public partial class StartView : UserControl
    {
        private MainWindow Main => (MainWindow)Application.Current.MainWindow;

        public StartView()
        {
            InitializeComponent();

            // Lista betöltése a MainWindow-ból
            lbSaves.Items.Clear();

            foreach (var map in Main.Maps)
            {
                lbSaves.Items.Add(map.name);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (lbSaves.SelectedIndex == -1)
            {
                MessageBox.Show("Válassz térképet!");
                return;
            }

            Map selectedMap = Main.Maps[lbSaves.SelectedIndex];

            Main.ShowGameView(selectedMap);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {


            Map map = new Map(txtNameGiver.Text);

            Main.Maps.Add(map);

            lbSaves.Items.Add(map.name);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNameGiver.Text))
            {
                MessageBox.Show("Adj nevet a térképednek!");
                return;
            }

            Map map = new Map(txtNameGiver.Text);

            Main.Maps.Add(map);

            lbSaves.Items.Add(map.name);
        }
    }
}