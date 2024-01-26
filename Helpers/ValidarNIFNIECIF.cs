using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ikigenda.Helpers
{
    public static class ValidarNIFNIECIF
    {
        
    

        private enum TipoValidacion { NIF = 0, NIE = 1, CIF = 2}

        /// <summary>
        /// http://www.interior.gob.es/web/servicios-al-ciudadano/dni/calculo-del-digito-de-control-del-nif-nie
        /// 
        ///        Cálculo del dígito de control del NIF/NIE
        ///        El artículo 11 del Real Decreto 1553/2005, de 23 de diciembre, establece que el Documento Nacional de Identidad recogerá el número personal del DNI
        ///        y carácter de verificación correspondiente al número de Identificación Fiscal.
        ///           Para verificar el NIF de españoles residentes mayores de edad, el algoritmo de cálculo del dígito de control es el siguiente:
        ///       Se divide el número entre 23 y el resto se sustituye por una letra que se determina por inspección mediante la siguiente tabla:
        ///         RESTO   0 	1 	2 	3 	4 	5 	6 	7 	8 	9 	10 	11
        ///         LETRA T   R W   A G   M Y   F P   D X   B
        ///         RESTO 	12 	13 	14 	15 	16 	17 	18 	19 	20 	21 	22
        ///         LETRA N   J Z   S Q   V H   L C   K E
        ///       Por ejemplo, si el número del DNI es 12345678, dividido entre 23 da de resto 14, luego la letra sería la Z: 12345678Z.
        ///           Los NIE's de extranjeros residentes en España tienen una letra (X, Y, Z), 7 números y dígito de control.
        ///       Para el cálculo del dígito de control se sustituye:
        ///           X → 0
        ///           Y → 1
        ///           Z → 2
        ///       y se aplica el mismo algoritmo que para el NIF.
        ///       
        /// El CIF consta de 9 caracteres. El primero (posición 1) es una letra que sigue los siguientes criterios:
        ///     A.Sociedades anónimas.
        ///     B.Sociedades de responsabilidad limitada.
        ///     C.Sociedades colectivas.
        ///     D.Sociedades comanditarias.
        ///     E.Comunidades de bienes.
        ///     F.Sociedades cooperativas.
        ///     G.Asociaciones y fundaciones.
        ///     H.Comunidades de propietarios en régimen de propiedad horizontal.
        ///     J.Sociedades civiles.
        ///     N.Entidades no residentes.
        ///     P.Corporaciones locales.
        ///     Q.Organismos autónomos, estatales o no, y asimilados, y congregaciones e instituciones religiosas.
        ///     R.Congregaciones e instituciones religiosas (desde 2008, ORDEN EHA/451/2008)
        ///     S.Órganos de la Administración del Estado y comunidades autónomas
        ///     U.Uniones Temporales de Empresas.
        ///     V.Sociedad Agraria de Transformación.
        ///     W.Establecimientos permanentes de entidades no residentes en España
        ///     
        /// Las dos primeras indican la provincia.
        /// 
        /// Los cinco siguientes dígitos (posiciones 4 a 8) constituyen un número correlativo de inscripción de la organización en 
        ///     el registro provincial, y el último dígito (posición 9)
        ///     es un código de control que puede ser un número o una letra:
        ///     Será una LETRA si la clave de entidad es P, Q, S o W.O también si los dos dígitos iniciales indican "No Residente"
        ///     Será un NÚMERO si la entidad es A, B, E o H.
        ///     Para otras claves de entidad: el dígito podrá ser tanto número como letra.
        /// Las operaciones para calcular el dígito de control se realizan sobre los siete dígitos centrales y son las siguientes:
        ///     Sumar los dígitos de las posiciones pares.Suma = A
        ///     Para cada uno de los dígitos de las posiciones impares, multiplicarlo por 2 y sumar los dígitos del resultado.
        ///         Ej.: ( 8 * 2 = 16 --> 1 + 6 = 7 )
        /// Acumular el resultado.Suma = B
        /// Sumar A + B = C
        /// Tomar sólo el dígito de las unidades de C.Lo llamaremos dígito E.
        /// Si el dígito E es distinto de 0 lo restaremos a 10. D = 10 - E.Esta resta nos da D. Si no, si el dígito E es 0 entonces D = 0 y no hacemos resta.
        /// A partir de D ya se obtiene el dígito de control.Si ha de ser numérico es directamente D y si se trata de una letra se corresponde con la relación:
        ///    J = 0, A = 1, B = 2, C= 3, D = 4, E = 5, F = 6, G = 7, H = 8, I = 9
        /// 
        /// </summary>
        public static char CalculaDigitoDeControl(string nifNieCif)
        {
            if (string.IsNullOrWhiteSpace(nifNieCif))
                throw new ArgumentException("El NIF/NIE/CIF no puede ser vacío");

            if(nifNieCif.Length < 8 || nifNieCif.Length > 9)
                throw new ArgumentException("El NIF/NIE/CIF debe tener al menos 8 caracteres + dígito de control");

            nifNieCif = nifNieCif.ToUpperInvariant();

            const string letrasNifNie = "TRWAGMYFPDXBNJZSQVHLCKE";
            const string regExpEsNie = "^(X|Y|Z)";
            bool esNie = Regex.Match(nifNieCif, regExpEsNie).Success;
            const string regExpEsCif = "^[A-W]";
            bool esCif = !esNie && Regex.Match(nifNieCif, regExpEsCif).Success;
            if (esNie)
            {
                switch(nifNieCif[0])
                {
                    case 'X': nifNieCif = ReemplazarPrimerCaracter(nifNieCif, "X", "0"); break;
                    case 'Y': nifNieCif = ReemplazarPrimerCaracter(nifNieCif, "Y", "1"); break;
                    case 'Z': nifNieCif = ReemplazarPrimerCaracter(nifNieCif, "Z", "2"); break;
                }
            }

            int valorEntero = Convert.ToInt32(new String(nifNieCif.Where(Char.IsDigit).ToArray()));

            if (esCif)
            {
                if (valorEntero.ToString().Length.Equals(8))
                    valorEntero = valorEntero / 10;

                var digitosValor = DameDigitos(valorEntero);

                int sumaPares =
                    digitosValor[1] +
                    digitosValor[3] +
                    digitosValor[5];

                int sumaImparesPor2 =
                    SumaDigitos(digitosValor[0]*2) +
                    SumaDigitos(digitosValor[2]*2) +
                    SumaDigitos(digitosValor[4]*2) +
                    SumaDigitos(digitosValor[6]*2);

                int suma = sumaPares + sumaImparesPor2;
                int unidad = suma % 10;
                int digitoCif = unidad == 0 ? unidad : 10 - unidad;

                const string regExpEsSociedad = "^(A|B|E|H)";
                bool esNumero = Regex.Match(nifNieCif, regExpEsSociedad).Success;
                bool noResidente = valorEntero < 100000;
                const string regExpEsOrganismo = "^(K|P|Q|S|W)";
                bool esOrganismo = Regex.Match(nifNieCif, regExpEsOrganismo).Success;
                bool esLetra = esOrganismo || noResidente;
                bool esIndefinido = !esLetra && !esNumero;

                if(esNumero || esIndefinido)
                {
                    return digitoCif.ToString().ToArray()[0];
                }
                else
                {
                    const string letrasCIF = "JABCDEFGHI";
                    return letrasCIF[digitoCif];
                }
            }

            int restoDe23 = valorEntero % 23;
            return letrasNifNie[restoDe23];
        }

        public static bool Validar(string nifNieCif)
        {
            if (String.IsNullOrWhiteSpace(nifNieCif)) return false;
            if (nifNieCif.Length < 8 || nifNieCif.Length > 9) return false;
            return nifNieCif.Last().Equals(CalculaDigitoDeControl(nifNieCif));
        }

        private static string ReemplazarPrimerCaracter(string entrada, string reemplazable, string reemplazo)
        {
            var regex = new Regex(Regex.Escape(reemplazable));
            return regex.Replace(entrada, reemplazo, 1);
        }

        private static int[] DameDigitos(int numero)
        {
            string temp = numero.ToString("0000000");
            var resul = new int[temp.Length];
            for (int i = 0; i < resul.Length; i++)
            {
                resul[i] = Convert.ToInt32(temp[i].ToString());
            }
            return resul;
        }

        private static int SumaDigitos(int v)
        {
            if (v < 5) return v;
            return (v % 10) + (v / 10);
        }
    }
}
