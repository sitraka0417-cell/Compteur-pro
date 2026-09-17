using System.Windows;

namespace CompteurCarnet
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ShowMenu();
        }

        public void ShowMenu() => MainContent.Content = new MenuView(this);

        public void ShowListe() => MainContent.Content = new ListeView(this);

        public void ShowTaches() => MainContent.Content = new TachesView(this);

        public void ShowHistorique() => MainContent.Content = new HistoriqueView(this);
    }
}
