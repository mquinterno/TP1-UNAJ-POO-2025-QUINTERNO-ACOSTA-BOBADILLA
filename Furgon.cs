using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase FURGÓN (hereda de Vehículo)
    // ===========================================================
    public class Furgon : Vehiculo
    {
        // Constructor que llama al base
        public Furgon(string codigoInterno, string patente, double capacidad, double kilometraje, double costoBase)
            : base(codigoInterno, patente, capacidad, kilometraje, costoBase)
        {
        }

        // Implementación polimórfica
        public override double CalcularCosto(double distancia)
        {
            // Fórmula: costo base + (distancia * 1.5)
            return costoBase + (distancia * 1.5);
        }
    }
}
