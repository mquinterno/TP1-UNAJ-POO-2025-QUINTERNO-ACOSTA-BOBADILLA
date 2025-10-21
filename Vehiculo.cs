using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase base VEHÍCULO
    // -----------------------------------------------------------
    // Aplica:
    //  - Herencia 
    //  - Polimorfismo 
    //  - Interfaz para cálculo de costo operativo 
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

        // Propiedades (con encapsulamiento)
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
        // MÉTODOS POLIMÓRFICOS
        // ===========================================================
        // Se mantiene el método abstracto original (distancia),
        // pero agregamos una sobrecarga opcional con carga y distancia.
        // ===========================================================
        public abstract double CalcularCosto(double distancia);

        
        public virtual double CalcularCostoOperativo(double carga, double distancia)
        {
            // Método virtual vacío que las subclases sobrescriben
            return 0;
        }

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
