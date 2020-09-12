using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    public class SyntaxNode
    {
        public List<SyntaxNode> Sons { get; set; }
        public string Value { get; set; }

        public SyntaxNode(string value)
        {
            this.Value = value;
            this.Sons = new List<SyntaxNode>();
        }
    }
}
