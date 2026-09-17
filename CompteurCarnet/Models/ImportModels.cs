using System.Collections.Generic;

namespace CompteurCarnet.Models
{
    // These classes mirror the structure of the JSON files that get imported
    // (the same format used by the original Android application).
    public class ImportRoot
    {
        public List<ImportCommune> communes { get; set; }
    }

    public class ImportCommune
    {
        public string code_commune { get; set; }
        public List<ImportFokontany> fokontany { get; set; }
    }

    public class ImportFokontany
    {
        public string code_fokontany { get; set; }
        public List<ImportCarnet> carnets { get; set; }
    }

    public class ImportCarnet
    {
        public string num_carnet { get; set; }
        public List<ImportFeuillet> feuillets { get; set; }
    }

    public class ImportFeuillet
    {
        public string num_feuillet { get; set; }
        public string statut { get; set; }
    }
}
