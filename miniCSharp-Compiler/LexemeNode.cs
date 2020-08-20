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
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int Row { get; set; }
        public char Token { get; set; }
        public string Description { get; set; }
        
        //entrada: hola%2#3estoEsUnID
        //1. Los caracteres no válidos funcionaron como separadores de tokens
        //hola                   --- Identificador columnas 1-4
        //%                      --- Error en columna 5, el caracter % no es reconocido
        //2                      --- Constante  columnas 6-6
        //#                      --- Error en columna 7, el caracter # no es reconocido, se omitió
        //3                      --- Constante columnas 8-8
        //estoEsUnID             --- Identificador columnas 9-18
    }
}
