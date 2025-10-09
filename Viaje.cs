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
    //  - Nuevo: cálculo de costo operativo (según TP)
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

        // Nueva propiedad: costo operativo real del viaje
        private double costoOperativo;

        // Propiedad usada por Empresa para controlar estado
        public bool Finalizado { get; set; }

        // ===========================================================
        // Constructor
        // ===========================================================
        public Viaje(string codigo, string origen, string destino, double distancia, DateTime fecha, double carga)
        {
            this.codigo = codigo;
            this.origen = origen;
            this.destino = destino;
            this.distancia = distancia;
            this.fecha = fecha;
            this.carga = carga;
            this.choferes = new List<Chofer>();
            this.Finalizado = false; // Por defecto, viaje activo
            this.costoOperativo = 0; // Inicializado en cero
        }

        // ===========================================================
        // Propiedades de solo lectura
        // ===========================================================
        public string Codigo { get { return codigo; } }
        public DateTime Fecha { get { return fecha; } }
        public double CostoOperativo { get { return costoOperativo; } }

        // ===========================================================
        // MÉTODOS DE COMPOSICIÓN
        // ===========================================================
        public void AsignarVehiculo(Vehiculo v)
        {
            if (!v.Disponible)
                throw new EliminacionAsignadaException("El vehículo ya está asignado a otro viaje.");

            if (this.carga > v.Capacidad)
                throw new CapacidadExcedidaException("La carga excede la capacidad del vehículo.");

            this.vehiculo = v;
            v.Disponible = false; // Marca el vehículo como en uso

            // Cálculo automático del costo operativo
            CalcularCostoOperativo();
        }

        public void AgregarChofer(Chofer c)
        {
            if (c.Asignado)
                throw new ChoferOcupadoException("El chofer ya tiene un viaje asignado.");

            choferes.Add(c);
            c.Asignado = true;
        }

        // ===========================================================
        // MÉTODOS DE CONSULTA
        // ===========================================================
        public Vehiculo ObtenerVehiculo()
        {
            return vehiculo;
        }

        public List<Chofer> ObtenerChoferes()
        {
            return choferes;
        }

        // ===========================================================
        // MÉTODOS DE CÁLCULO
        // ===========================================================

        // ✅ Nuevo: cálculo del costo operativo (según tipo de vehículo)
        private void CalcularCostoOperativo()
        {
            if (vehiculo != null)
            {
                // Usa polimorfismo: cada tipo de vehículo aplica su fórmula
                costoOperativo = vehiculo.CalcularCostoOperativo(carga, distancia);
            }
            else
            {
                costoOperativo = 0;
            }
        }

        // Método anterior (se mantiene para compatibilidad)
        public double CalcularCostoTotal()
        {
            double costo = 0;
            if (vehiculo != null)
                costo = vehiculo.CalcularCosto(distancia);
            return costo;
        }

        // ===========================================================
        // FINALIZAR VIAJE
        // ===========================================================
        public void FinalizarViaje()
        {
            this.Finalizado = true;

            if (vehiculo != null)
                vehiculo.Disponible = true;

            foreach (Chofer c in choferes)
                c.Asignado = false;
        }

        // ===========================================================
        // SALIDA DE INFORMACIÓN
        // ===========================================================
        public void GenerarInformeCSV()
        {
            using (StreamWriter sw = new StreamWriter("viajes.csv", true))
            {
                // Agregamos el costo operativo real al registro
                sw.WriteLine(codigo + ";" + origen + ";" + destino + ";" + distancia + ";" + carga + ";" +
                             vehiculo.CodigoInterno + ";" + CostoOperativo);
            }
        }

        public override string ToString()
        {
            string infoChoferes = "";
            foreach (Chofer c in choferes)
                infoChoferes += c.Nombre + " ";

            string estado = Finalizado ? "Finalizado" : "Activo";

            return "Código: " + codigo +
                   " | Ruta: " + origen + " - " + destino +
                   " | Distancia: " + distancia + " km" +
                   " | Vehículo: " + (vehiculo != null ? vehiculo.CodigoInterno : "Sin asignar") +
                   " | Chofer(es): " + infoChoferes +
                   " | Estado: " + estado +
                   " | Costo operativo: $" + CostoOperativo.ToString("F2");
        }
    }
}
