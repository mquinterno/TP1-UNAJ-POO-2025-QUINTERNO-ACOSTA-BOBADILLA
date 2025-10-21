using System;

namespace TransporteApp
{
    // ===========================================================
    // Excepción personalizada - Carga excedida
    // Aplica: Herencia de Exception
    // ===========================================================
    public class CapacidadExcedidaException : Exception
    {
        public CapacidadExcedidaException(string mensaje) : base(mensaje)
        {
        }
    }
}
