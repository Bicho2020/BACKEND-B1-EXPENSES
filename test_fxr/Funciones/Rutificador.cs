using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Funciones
{
    public class Rutificador
    {
        public static Boolean Ruti(string rut)
        {

            rut = rut.ToUpper();
            rut = rut.Replace(".", "");
            rut = rut.Replace("-", "");
            int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));

            char dv = char.Parse(rut.Substring(rut.Length - 1, 1));

            int m = 0, s = 1;
            for (; rutAux != 0; rutAux /= 10)
            {
                s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
            }
            if (dv == (char)(s != 0 ? s + 47 : 75))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
