using System;
using System.Collections.Generic;

namespace taide.Models {
    public class NewVariable {
        public string varName { get; set; }
        public VarTypes varType { get; set; }
        public bool isEnvVar { get; set; }
        public string GlobalValue { get; set; }
        public string DevValue { get; set; }
        public string QaValue { get; set; }
        public string QtsValue { get; set; }
        public string ProdValue { get; set; }
    }

    public enum VarTypes {
        StringType,
        IntegerType
    }
}