using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase MOTO DE REPARTO (hereda de Vehículo)
    // -----------------------------------------------------------
    // Nuevo cálculo de costo operativo:
    //   costo = 5000 × (1 + distancia/100) × (carga / capacidad)
    // ===========================================================
    public class MotoReparto : Vehiculo
    {
        // Constructor
        public MotoReparto(string codigoInterno, string patente, double capacidad, double kilometraje, double costoBase)
            : base(codigoInterno, patente, capacidad, kilometraje, costoBase)
        {
        }

        
        public override double CalcularCosto(double distancia)
        {
            return costoBase + (distancia * 0.8);
        }

        // ===========================================================
        // Nuevo método de costo operativo
        // ===========================================================
        public override double CalcularCostoOperativo(double carga, double distancia)
        {
            if (capacidad <= 0)
                throw new Exception("Capacidad de la moto no puede ser cero.");

            double porcentajeCarga = carga / capacidad;
            double costo = 5000 * (1 + (distancia / 100)) * porcentajeCarga;
            return Math.Round(costo, 2);
        }
    }
}
