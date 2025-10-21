using System;

namespace TransporteApp
{
    // ===========================================================
    // Excepción personalizada - Eliminación no permitida
    // Aplica: Herencia de Exception
    // ===========================================================
    public class EliminacionAsignadaException : Exception
    {
        public EliminacionAsignadaException(string mensaje) : base(mensaje)
        {
        }
    }
}
