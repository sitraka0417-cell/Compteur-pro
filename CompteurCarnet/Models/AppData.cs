using System.Collections.Generic;

namespace CompteurCarnet.Models
{
    public class AppData
    {
        public List<Carnet> CarnetsDisponibles { get; set; } = new();
        public List<HistoriqueEntry> Historique { get; set; } = new();
        public int NombreOngletsTotal { get; set; } = 1;
    }
}
