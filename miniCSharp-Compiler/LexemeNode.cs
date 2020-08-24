using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    public class LexemeNode
    {
        public string Value { get; set; }
        public char Token { get; set; }
        public string Description { get; set; }
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int StartRow { get; set; }
        public int EndRow { get; set; }

        public LexemeNode()
        {
            this.Value = string.Empty;
            this.Description = string.Empty;
        }
        /*
            Tokens code:
                E --> Error
                I --> Identifier
                R --> Reserved word
                D --> double
                B --> bool
                N --> int
                H --> hexadecimal
                X --> Exponential                
                S --> String
                C --> // type comment
                M --> multiline comment
                O --> Operator
         */
    }
}
