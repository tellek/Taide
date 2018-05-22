using System;
using System.Collections.Generic;

namespace taide.Models {
    public class AggregateData {
        public List<string> GlobalData { get; set; }
        public List<string> DevData { get; set; }
        public List<string> QaData { get; set; }
        public List<string> QtsData { get; set; }
        public List<string> ProdData { get; set; }

        public string GlobalText { get; set; }
        public string DevText { get; set; }
        public string QaText { get; set; }
        public string QtsText { get; set; }
        public string ProdText { get; set; }

        public List<string> VariableNames { get; set; }
        public string VariableText { get; set; }
    }
}