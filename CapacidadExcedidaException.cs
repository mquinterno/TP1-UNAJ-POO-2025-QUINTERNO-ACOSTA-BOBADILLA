using System;

namespace TransporteApp
{
    // ===========================================================
    // Excepción personalizada - Carga excedida
    // Aplica: Herencia de Exception (Clase 8)
    // ===========================================================
    public class CapacidadExcedidaException : Exception
    {
        public CapacidadExcedidaException(string mensaje) : base(mensaje)
        {
        }
    }
}
