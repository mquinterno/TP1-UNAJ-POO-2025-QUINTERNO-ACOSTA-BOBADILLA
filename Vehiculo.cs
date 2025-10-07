using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase base VEHÍCULO
    // -----------------------------------------------------------
    // Aplica:
    //  - Herencia (Clase 6)
    //  - Polimorfismo (Clase 7)
    // ===========================================================
    public abstract class Vehiculo
    {
        protected string codigoInterno;
        protected string patente;
        protected double capacidad;
        protected double kilometraje;
        protected double costoBase;
        protected bool disponible;

        // Constructor
        public Vehiculo(string codigoInterno, string patente, double capacidad, double kilometraje, double costoBase)
        {
            this.codigoInterno = codigoInterno;
            this.patente = patente;
            this.capacidad = capacidad;
            this.kilometraje = kilometraje;
            this.costoBase = costoBase;
            this.disponible = true;
        }

        // Propiedades (encapsulamiento)
        public string CodigoInterno { get { return codigoInterno; } }
        public string Patente { get { return patente; } }
        public double Capacidad { get { return capacidad; } }
        public double Kilometraje { get { return kilometraje; } }
        public double CostoBase { get { return costoBase; } }
        public bool Disponible
        {
            get { return disponible; }
            set { disponible = value; }
        }

        // ===========================================================
        // MÉTODO ABSTRACTO - Polimorfismo
        // ===========================================================
        // Cada tipo de vehículo define su propio cálculo de costo.
        public abstract double CalcularCosto(double distancia);

        // ===========================================================
        // Representación textual del objeto
        // ===========================================================
        public override string ToString()
        {
            return "Código: " + codigoInterno +
                   " | Patente: " + patente +
                   " | Capacidad: " + capacidad + " kg" +
                   " | Kilometraje: " + kilometraje +
                   " | Costo Base: $" + costoBase +
                   " | Disponible: " + (disponible ? "Sí" : "No");
        }
    }
}
