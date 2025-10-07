using System;
using System.Collections.Generic;
using System.IO;

namespace TransporteApp
{
    // ===========================================================
    // Clase VIAJE
    // -----------------------------------------------------------
    // Aplica:
    //  - Composición (usa choferes y vehículo)
    //  - Polimorfismo (vehículos diferentes)
    //  - Archivos (generación de informe)
    //  - Excepciones personalizadas
    // ===========================================================
    public class Viaje
    {
        private string codigo;
        private string origen;
        private string destino;
        private double distancia;
        private DateTime fecha;
        private double carga;
        private Vehiculo vehiculo;
        private List<Chofer> choferes;

        // Nueva propiedad agregada (para validaciones en Empresa)
        public bool Finalizado { get; set; }

        // Constructor
        public Viaje(string codigo, string origen, string destino, double distancia, DateTime fecha, double carga)
        {
            this.codigo = codigo;
            this.origen = origen;
            this.destino = destino;
            this.distancia = distancia;
            this.fecha = fecha;
            this.carga = carga;
            this.choferes = new List<Chofer>();
            this.Finalizado = false; // Por defecto, el viaje comienza como activo
        }

        // Propiedades de solo lectura (acceso externo)
        public string Codigo { get { return codigo; } }
        public DateTime Fecha { get { return fecha; } }

        // ===========================================================
        // MÉTODOS DE COMPOSICIÓN
        // ===========================================================

        // Asignar un vehículo al viaje
        public void AsignarVehiculo(Vehiculo v)
        {
        	if (!v.Disponible)
        		throw new EliminacionAsignadaException("El vehículo ya está asignado a otro viaje.");

        	// Validar capacidad antes de asignar
        	if (this.carga > v.Capacidad)
        		throw new CapacidadExcedidaException("La carga excede la capacidad del vehículo.");

        	this.vehiculo = v;
        	v.Disponible = false; // Marca el vehículo como en uso
        }


        // Asignar un chofer al viaje
        public void AgregarChofer(Chofer c)
        {
            if (c.Asignado)
                throw new ChoferOcupadoException("El chofer ya tiene un viaje asignado.");

            choferes.Add(c);
            c.Asignado = true; // Marca al chofer como ocupado
        }

        // ===========================================================
        // MÉTODOS DE CONSULTA (usados por Empresa)
        // ===========================================================

        // Retorna el vehículo asignado al viaje
        public Vehiculo ObtenerVehiculo()
        {
            return vehiculo;
        }

        // Retorna la lista de choferes asignados
        public List<Chofer> ObtenerChoferes()
        {
            return choferes;
        }

        // ===========================================================
        // MÉTODOS DE CÁLCULO Y SALIDA
        // ===========================================================

        // Costo estimado (puede incluir polimorfismo)
        public double CalcularCostoTotal()
        {
            double costo = 0;
            if (vehiculo != null)
                costo = vehiculo.CalcularCosto(distancia);

            return costo;
        }

        // Finalizar el viaje (libera choferes y vehículo)
        public void FinalizarViaje()
        {
            this.Finalizado = true;

            if (vehiculo != null)
                vehiculo.Disponible = true;

            foreach (Chofer c in choferes)
            {
                c.Asignado = false;
            }
        }

        // ===========================================================
        // SALIDA DE INFORMACIÓN (Archivo CSV)
        // ===========================================================

        // Generar archivo de registro de viaje
        public void GenerarInformeCSV()
        {
            using (StreamWriter sw = new StreamWriter("viajes.csv", true))
            {
                sw.WriteLine(codigo + ";" + origen + ";" + destino + ";" + distancia + ";" + carga + ";" +
                             vehiculo.CodigoInterno + ";" + CalcularCostoTotal());
            }
        }

        // Mostrar información del viaje
        public override string ToString()
        {
            string infoChoferes = "";
            foreach (Chofer c in choferes)
            {
                infoChoferes += c.Nombre + " ";
            }

            string estado = Finalizado ? "Finalizado" : "Activo";

            return "Código: " + codigo +
                   " | Ruta: " + origen + " - " + destino +
                   " | Distancia: " + distancia + " km" +
                   " | Vehículo: " + (vehiculo != null ? vehiculo.CodigoInterno : "Sin asignar") +
                   " | Chofer(es): " + infoChoferes +
                   " | Estado: " + estado +
                   " | Costo estimado: $" + CalcularCostoTotal();
        }
    }
}
