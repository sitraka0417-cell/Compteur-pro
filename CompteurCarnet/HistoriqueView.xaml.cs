using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CompteurCarnet.Services;

namespace CompteurCarnet
{
    public partial class HistoriqueView : UserControl
    {
        private readonly MainWindow _owner;

        public HistoriqueView(MainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            Charger();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            _owner.ShowMenu();
        }

        private void Charger()
        {
            var data = DataStore.Load();
            HistoriqueList.Items.Clear();

            if (data.Historique.Count == 0)
            {
                HistoriqueList.Items.Add(new TextBlock
                {
                    Text = "Aucun carnet terminé pour le moment.",
                    Foreground = (Brush)FindResource("MutedTextBrush"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 40, 0, 0)
                });
                return;
            }

            // Most recent first, mirroring the original app.
            for (int i = data.Historique.Count - 1; i >= 0; i--)
            {
                var entree = data.Historique[i];
                var nomCommune = CommunesData.NomCommune(entree.CodeCommune);
                var nomFokontany = CommunesData.NomFokontany(entree.CodeFokontany);
                var listeFeuillets = string.Join(", ", entree.FeuilletsValides);

                var texte = $"Carnet {entree.NumCarnet}\nCommune: {nomCommune}\nFokontany: {nomFokontany}\n" +
                            $"Terminé le {entree.Date}\n{entree.FeuilletsValides.Count} feuillets: {listeFeuillets}";

                var border = new Border
                {
                    Background = (Brush)FindResource("CardBrush"),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x6A, 0x1B, 0x9A)),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(14),
                    Padding = new Thickness(18),
                    Margin = new Thickness(0, 0, 0, 16),
                    Child = new TextBlock
                    {
                        Text = texte,
                        Foreground = Brushes.White,
                        FontSize = 14,
                        TextWrapping = TextWrapping.Wrap
                    }
                };

                HistoriqueList.Items.Add(border);
            }
        }
    }
}
