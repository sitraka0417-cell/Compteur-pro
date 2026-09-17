using System.Collections.Generic;

namespace CompteurCarnet.Models
{
    public class HistoriqueEntry
    {
        public string CodeCommune { get; set; }
        public string CodeFokontany { get; set; }
        public string NumCarnet { get; set; }
        public List<string> FeuilletsValides { get; set; } = new();
        public string Date { get; set; }
    }
}
