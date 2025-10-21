using System;

namespace TransporteApp
{
    // ===========================================================
    // Excepción personalizada - Chofer ya ocupado
    // Aplica: Herencia de Exception
    // ===========================================================
    public class ChoferOcupadoException : Exception
    {
        public ChoferOcupadoException(string mensaje) : base(mensaje)
        {
        }
    }
}
