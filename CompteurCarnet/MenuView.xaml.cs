using System.Windows;
using System.Windows.Controls;

namespace CompteurCarnet
{
    public partial class MenuView : UserControl
    {
        private readonly MainWindow _owner;

        public MenuView(MainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
        }

        private void BtnListe_Click(object sender, RoutedEventArgs e)
        {
            _owner.ShowListe();
        }

        private void BtnTaches_Click(object sender, RoutedEventArgs e)
        {
            _owner.ShowTaches();
        }

        private void BtnHistorique_Click(object sender, RoutedEventArgs e)
        {
            _owner.ShowHistorique();
        }
    }
}
