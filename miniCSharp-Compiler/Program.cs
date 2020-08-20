using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace miniCSharp_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = "pathToFile.txt";//implement IDE for user
            var analyze = new LexicalAnalyzer();
            analyze.ReadFile(path);
            
            
        }
    }
}





