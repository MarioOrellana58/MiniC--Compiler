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
            var fileExists = false;
            var path = string.Empty;
            do
            {
                Console.Clear();
                Console.WriteLine("Ingrese la ruta al archivo que desea analizar");
                path = Console.ReadLine();
                fileExists = File.Exists(path) ? true : false;
            } while (!fileExists);

            
            var resultFilePath = "C:/lexicalAnalyzer/" + Path.ChangeExtension(path, ".out");

            var analyze = new LexicalAnalyzer();
            analyze.ReadFileAndAnalyzeDocument(path);

            analyze.PrintResultAndSaveToFile(resultFilePath);

            Console.ForegroundColor = ConsoleColor.Cyan;    
            Console.WriteLine("Tu archivo de salida se encuentra en " + resultFilePath);
            Console.ReadKey();
            
        }

        
    }
}





