using System;

namespace TransporteApp
{
    // ===========================================================
    // Clase Chofer
    // Aplica: Encapsulamiento, Propiedades, Validación y Excepciones
    // ===========================================================
    public class Chofer
    {
        private string nombre;
        private string direccion;
        private string estadoCivil;
        private DateTime fechaNacimiento;
        private double sueldoBasico;
        private bool asignado; // indica si tiene un viaje en curso

        public Chofer(string nombre, string direccion, string estadoCivil, DateTime fechaNacimiento, double sueldoBasico)
        {
            this.nombre = nombre;
            this.direccion = direccion;
            this.estadoCivil = estadoCivil;
            this.fechaNacimiento = fechaNacimiento;
            this.sueldoBasico = sueldoBasico;
            this.asignado = false;
        }

        // Propiedades 
        public string Nombre
        {
            get { return nombre; }
            set { nombre = (value ?? "").Trim(); }
        }

        public double SueldoBasico
        {
            get { return sueldoBasico; }
            set
            {
                if (value > 0)
                    sueldoBasico = value;
                else
                    throw new Exception("El sueldo debe ser mayor que cero.");
            }
        }

        public bool Asignado
        {
            get { return asignado; }
            set { asignado = value; }
        }

        // Método que aplica uso de DateTime
        public int CalcularEdad()
        {
            int edad = DateTime.Now.Year - fechaNacimiento.Year;
            if (DateTime.Now.DayOfYear < fechaNacimiento.DayOfYear)
                edad--;
            return edad;
        }

        public override string ToString()
        {
            return "Chofer: " + nombre + " | Edad: " + CalcularEdad() + " | Sueldo: $" + sueldoBasico;
        }
    }
}
