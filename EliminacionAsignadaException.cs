using System;

namespace TransporteApp
{
    // ===========================================================
    // Excepción personalizada - Eliminación no permitida
    // Aplica: Herencia de Exception (Clase 8)
    // ===========================================================
    public class EliminacionAsignadaException : Exception
    {
        public EliminacionAsignadaException(string mensaje) : base(mensaje)
        {
        }
    }
}
