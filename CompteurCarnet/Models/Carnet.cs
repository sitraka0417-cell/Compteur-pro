using System.Collections.Generic;

namespace CompteurCarnet.Models
{
    public class Carnet
    {
        public string Id { get; set; }
        public string CodeCommune { get; set; }
        public string CodeFokontany { get; set; }
        public string NumCarnet { get; set; }
        public List<string> FeuilletsValides { get; set; } = new();
        public int TotalValide { get; set; }
        public int Progression { get; set; }
        public int AssigneOnglet { get; set; } = -1;
        public bool EnAttenteFinalisation { get; set; }
    }
}

