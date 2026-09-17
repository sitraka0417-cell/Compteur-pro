using System.Collections.Generic;
using System.Windows;
using CompteurCarnet.Models;
using CompteurCarnet.Services;

namespace CompteurCarnet
{
    public partial class SelectCarnetWindow : Window
    {
        private readonly List<Carnet> _disponibles;

        public Carnet CarnetChoisi { get; private set; }

        public SelectCarnetWindow(List<Carnet> disponibles)
        {
            InitializeComponent();
            _disponibles = disponibles;

            foreach (var c in _disponibles)
            {
                var nc = CommunesData.NomCommune(c.CodeCommune);
                var nf = CommunesData.NomFokontany(c.CodeFokontany);
                ListeCarnets.Items.Add($"Carnet {c.NumCarnet} - {nc} / {nf}");
            }

            if (ListeCarnets.Items.Count > 0)
            {
                ListeCarnets.SelectedIndex = 0;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var index = ListeCarnets.SelectedIndex;
            if (index < 0 || index >= _disponibles.Count)
            {
                DialogResult = false;
                return;
            }

            CarnetChoisi = _disponibles[index];
            DialogResult = true;
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
