using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    public class SymbolNode
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public dynamic Value { get; set; }
        public int Scope { get; set; }
        public List<string> Parameters { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int StartRow { get; set; }
        public SymbolNode()
        {
            Name = string.Empty;
            Type = string.Empty;
        }

        public void InitParameters()
        {
            Parameters = new List<string>();
        }

    }
}