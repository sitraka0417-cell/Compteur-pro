using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using CompteurCarnet.Models;
using CompteurCarnet.Services;

namespace CompteurCarnet
{
    public partial class ListeView : UserControl
    {
        private readonly MainWindow _owner;
        private AppData _data;

        public ListeView(MainWindow owner)
        {
            InitializeComponent();
            _owner = owner;
            _data = DataStore.Load();
            RefreshList();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            _owner.ShowMenu();
        }

        private void BtnImporter_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choisir des fichiers JSON",
                Filter = "Fichiers JSON (*.json)|*.json|Tous les fichiers (*.*)|*.*",
                Multiselect = true
            };

            if (dialog.ShowDialog(_owner) != true) return;

            int ajoutes = 0;
            int erreurs = 0;

            foreach (var path in dialog.FileNames)
            {
                try
                {
                    ajoutes += ImporterFichier(path);
                }
                catch
                {
                    erreurs++;
                }
            }

            DataStore.Save(_data);
            RefreshList();

            string message = $"{ajoutes} carnet(s) importé(s).";
            if (erreurs > 0) message += $"\n{erreurs} fichier(s) n'a/n'ont pas pu être lu(s).";
            MessageBox.Show(_owner, message, "Importation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private int ImporterFichier(string path)
        {
            var json = File.ReadAllText(path);
            var root = JsonSerializer.Deserialize<ImportRoot>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (root?.communes == null) return 0;

            int compteur = 0;

            foreach (var commune in root.communes)
            {
                var codeCommune = commune.code_commune;
                if (commune.fokontany == null) continue;

                foreach (var fokontany in commune.fokontany)
                {
                    var codeFokontany = fokontany.code_fokontany;
                    if (fokontany.carnets == null) continue;

                    foreach (var carnet in fokontany.carnets)
                    {
                        var numCarnet = carnet.num_carnet;
                        var feuilletsValides = new List<string>();

                        if (carnet.feuillets != null)
                        {
                            foreach (var feuillet in carnet.feuillets)
                            {
                                var statut = feuillet.statut ?? "";
                                if (string.IsNullOrWhiteSpace(statut))
                                {
                                    feuilletsValides.Add(feuillet.num_feuillet);
                                }
                            }
                        }

                        var id = $"{codeCommune}_{numCarnet}";
                        bool dejaPresent = _data.CarnetsDisponibles.Any(c => c.Id == id);
                        if (dejaPresent) continue;

                        _data.CarnetsDisponibles.Add(new Carnet
                        {
                            Id = id,
                            CodeCommune = codeCommune,
                            CodeFokontany = codeFokontany,
                            NumCarnet = numCarnet,
                            FeuilletsValides = feuilletsValides,
                            TotalValide = feuilletsValides.Count,
                            Progression = 0,
                            AssigneOnglet = -1,
                            EnAttenteFinalisation = false
                        });
                        compteur++;
                    }
                }
            }

            return compteur;
        }

        private void RefreshList()
        {
            CarnetsList.Items.Clear();

            if (_data.CarnetsDisponibles.Count == 0)
            {
                CarnetsList.Items.Add(new TextBlock
                {
                    Text = "Aucun carnet importé pour le moment.",
                    Foreground = (Brush)FindResource("MutedTextBrush"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 40, 0, 0)
                });
                return;
            }

            foreach (var carnet in _data.CarnetsDisponibles)
            {
                CarnetsList.Items.Add(BuildCard(carnet));
            }
        }

        private Border BuildCard(Carnet carnet)
        {
            var nomCommune = CommunesData.NomCommune(carnet.CodeCommune);
            var nomFokontany = CommunesData.NomFokontany(carnet.CodeFokontany);

            var texte = $"Carnet {carnet.NumCarnet}\nCommune: {nomCommune}\nFokontany: {nomFokontany}\n{carnet.TotalValide} feuillets";
            if (carnet.AssigneOnglet != -1)
            {
                texte += $"\n(En cours - Onglet {carnet.AssigneOnglet})";
            }

            var stack = new StackPanel();
            stack.Children.Add(new TextBlock
            {
                Text = texte,
                Foreground = Brushes.White,
                FontSize = 15,
                Margin = new Thickness(0, 0, 0, 12)
            });

            var btnSupprimer = new Button
            {
                Content = "Supprimer",
                Style = (Style)FindResource("SmallButton"),
                Background = (Brush)FindResource("RedBrush"),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            btnSupprimer.Click += (s, e) =>
            {
                _data.CarnetsDisponibles.RemoveAll(c => c.Id == carnet.Id);
                DataStore.Save(_data);
                RefreshList();
            };
            stack.Children.Add(btnSupprimer);

            var border = new Border
            {
                Background = (Brush)FindResource("CardBrush"),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x1E, 0x88, 0xE5)),
                BorderThickness = new Thickness(2),
                CornerRadius = new CornerRadius(14),
                Padding = new Thickness(18),
                Margin = new Thickness(0, 0, 0, 16),
                Child = stack
            };

            return border;
        }
    }
}
