using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase MOTO DE REPARTO (hereda de Vehículo)
    // ===========================================================
    public class MotoReparto : Vehiculo
    {
        // Constructor que llama al base
        public MotoReparto(string codigoInterno, string patente, double capacidad, double kilometraje, double costoBase)
            : base(codigoInterno, patente, capacidad, kilometraje, costoBase)
        {
        }

        // Implementación polimórfica
        public override double CalcularCosto(double distancia)
        {
            // Fórmula: costo base + (distancia * 0.8)
            return costoBase + (distancia * 0.8);
        }
    }
}
