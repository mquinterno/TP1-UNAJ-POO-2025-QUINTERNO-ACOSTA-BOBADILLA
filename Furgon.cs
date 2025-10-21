using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase FURGÓN (hereda de Vehículo)
    // -----------------------------------------------------------
    // Nuevo cálculo de costo operativo:
    //   costo = 10000 × (carga / capacidad)
    // ===========================================================
    public class Furgon : Vehiculo
    {
        // Constructor
        public Furgon(string codigoInterno, string patente, double capacidad, double kilometraje, double costoBase)
            : base(codigoInterno, patente, capacidad, kilometraje, costoBase)
        {
        }

        // Mantenemos metodo original.
        public override double CalcularCosto(double distancia)
        {
             
            return costoBase + (distancia * 1.5);
        }

        // ===========================================================
        // Nuevo método de costo operativo
        // ===========================================================
        public override double CalcularCostoOperativo(double carga, double distancia)
        {
            if (capacidad <= 0)
                throw new Exception("Capacidad del furgón no puede ser cero.");

            double porcentajeCarga = carga / capacidad;
            double costo = 10000 * porcentajeCarga;
            return Math.Round(costo, 2);
        }
    }
}
