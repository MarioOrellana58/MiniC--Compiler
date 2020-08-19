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

            var path = "fileName.txt";//implement IDE for user
            int column = 0;
            int row = 0;

            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                row++;
                var fileLine = sr.ReadLine();
                while (fileLine != null)
                {
                    row++;
                    for (int i = 0; i < fileLine.Length; i++)
                    {
                        column++;
                    }
                    fileLine = sr.ReadLine();
                }
            }
        }
    }
}
