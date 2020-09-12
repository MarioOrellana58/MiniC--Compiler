using System;
using System.IO;
using System.Threading;

namespace miniCSharp_Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileExists = false;
            var path = string.Empty;
            var englishVersion = false;
            Console.WriteLine("Presiona cualquier tecla para la versión en español" + "\n");
            Console.WriteLine("Press 'E' for english version");

            if (Console.ReadKey().Key == ConsoleKey.E)
            {
                englishVersion = true;
            }
            var analyze = new LexicalAnalyzer(englishVersion);

            do
            {
                Console.Clear();
                Console.WriteLine(!englishVersion ? "Ingrese la ruta al archivo que desea analizar" : "Introduce your file path");
                path = Console.ReadLine();
                fileExists = File.Exists(path) ? true : false;
            } while (!fileExists);


            var resultFilePath = "C:/lexicalAnalyzer/" + Path.GetFileNameWithoutExtension(path) + ".out";

            analyze.ReadFileAndAnalyzeDocument(path);

            var areLexemesCorrect = analyze.PrintResultAndSaveToFile(resultFilePath);

            if (areLexemesCorrect)
            {
                var analizeSyntax = new SyntaxAnalyzer(analyze.Lexemes);
                analizeSyntax.ReadLexemes();
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(!englishVersion ? "Tu archivo de salida se encuentra en " : "You can find your output file in ");
            Console.Write(resultFilePath);

            //printing area
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(!englishVersion ? "¿Desea imprimir su documento? Presione 'y' para sí y otra tecla para no" : "Would you like to print your file? Press 'y' for yes or any other key for no");
            Console.WriteLine(!englishVersion ? "Esto creará un archivo portable, su ubicación es: " : "This will create a portable file that you can find in: ");
            Console.Write("C:/lexicalAnalyzer" + Path.ChangeExtension(path, ".pdf" + "\n"));

            var printingResult = string.Empty;
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\n");
                Console.WriteLine(!englishVersion ? "Asegúrate de que tu impresora esté conectada y encendida" : "Make sure your printer is connected and turned on");
                Console.WriteLine(!englishVersion ? "Luego de haberte asegurado presiona enter :D" : "After making sure, press enter :D");
                Console.WriteLine("\n");
                Console.ReadKey();
                printingResult = analyze.PrintFile(resultFilePath);
            }

            Console.WriteLine("\n");
            var goodbyeMessage = string.Empty;
            if (DateTime.Now.Hour < 12)
            {
                goodbyeMessage = !englishVersion ? "Ten un feliz día :D" : "Have a nice day :D";
            }
            else if (DateTime.Now.Hour < 18)
            {
                goodbyeMessage = !englishVersion ? "Ten una feliz tarde :D" : "Have a nice afternoon :D";
            }
            else
            {
                goodbyeMessage = !englishVersion ? "Ten una feliz noche :D" : "Have a nice evening :D";
            }

            for (int i = 10; i >= 0; i--)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(printingResult + "\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(goodbyeMessage + "\n");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine((!englishVersion ? "Esta consola se cerrará en " : "This terminal will close in ") + i.ToString());
                Console.ForegroundColor = ConsoleColor.Green;

                Thread.Sleep(1000);
            }
        }

    }
}