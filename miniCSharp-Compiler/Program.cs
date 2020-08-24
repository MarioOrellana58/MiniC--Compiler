using System;
using System.IO;

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


            var resultFilePath = "C:/lexicalAnalyzer/" + Path.GetFileNameWithoutExtension(path) + ".out";

            var analyze = new LexicalAnalyzer();
            analyze.ReadFileAndAnalyzeDocument(path);

            analyze.PrintResultAndSaveToFile(resultFilePath);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Tu archivo de salida se encuentra en " + resultFilePath);

            //printing area
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("¿Desea imprimir su documento? Presione 'y' para sí y otra tecla para no");
            Console.WriteLine("Esto creará un archivo portable, su ubicación es: C:/lexicalAnalyzer" + Path.ChangeExtension(path, ".pdf"));

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine("\n");
                Console.WriteLine("Asegúrate de que tu impresora esté conectada y encendida");
                Console.WriteLine("Luego de haberte asegurado presiona enter :D");
                Console.WriteLine("\n");
                Console.ReadKey();
                analyze.PrintFile(resultFilePath);
            }
            else
            {
                Console.WriteLine("\n");
                Console.WriteLine("Ten un feliz día :D");
            }

            Console.ReadKey();

        }


    }
}