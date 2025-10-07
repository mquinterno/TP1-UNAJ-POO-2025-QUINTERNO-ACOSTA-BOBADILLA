using System;
using System.Collections.Generic;
using System.IO;

namespace TransporteApp
{
    // ===========================================================
    // Clase EMPRESA
    // -----------------------------------------------------------
    // Aplica:
    //  - Composición (Clase 5)
    //  - Colecciones (Clase 4)
    //  - Archivos de texto (Clase 5)
    //  - Herencia y Polimorfismo (Clase 6 y 7)
    //  - Excepciones personalizadas (Clase 8)
    // ===========================================================
    public class Empresa
    {
        private string nombre;
        private string cuit;

        private List<Chofer> choferes;
        private List<Vehiculo> vehiculos;
        private List<Viaje> viajes;

        // Constructor
        public Empresa(string nombre, string cuit)
        {
            this.nombre = nombre;
            this.cuit = cuit;
            choferes = new List<Chofer>();
            vehiculos = new List<Vehiculo>();
            viajes = new List<Viaje>();

            // Cargar datos desde archivos (persistencia básica)
            CargarChoferesCSV();
            CargarVehiculosCSV();
        }

        // ===========================================================
        // MÉTODOS DE REGISTRO (Altas)
        // ===========================================================
        public void RegistrarChofer(Chofer c)
        {
            choferes.Add(c);
            GuardarChoferesCSV();
        }

        public void RegistrarVehiculo(Vehiculo v)
        {
            vehiculos.Add(v);
            GuardarVehiculosCSV();
        }

        public void RegistrarViaje(Viaje v)
        {
            viajes.Add(v);
        }

        // ===========================================================
        // MÉTODOS DE BÚSQUEDA
        // ===========================================================
        public Chofer BuscarChofer(string nombre)
        {
            foreach (Chofer c in choferes)
            {
                if (c.Nombre.ToLower() == nombre.ToLower())
                    return c;
            }
            return null;
        }

        public Vehiculo BuscarVehiculo(string codigo)
        {
            foreach (Vehiculo v in vehiculos)
            {
                if (v.CodigoInterno.ToLower() == codigo.ToLower())
                    return v;
            }
            return null;
        }

        // ===========================================================
        // MÉTODOS DE ELIMINACIÓN CON VALIDACIÓN REAL
        // ===========================================================

        // Solo se permite eliminar si el chofer no participa en ningún viaje activo
        public void EliminarChofer(string nombre)
        {
            Chofer c = BuscarChofer(nombre);

            if (c == null)
                throw new Exception("Chofer no encontrado.");

            // Verificar si participa en algún viaje activo
            foreach (Viaje v in viajes)
            {
                foreach (Chofer ch in v.ObtenerChoferes())
                {
                    if (ch == c && !v.Finalizado)
                    {
                        throw new EliminacionAsignadaException(
                            "No se puede eliminar: el chofer está asignado a un viaje activo.");
                    }
                }
            }

            // Si no está en viajes activos, eliminarlo
            choferes.Remove(c);
            GuardarChoferesCSV();
        }

        // Solo se permite eliminar si el vehículo no participa en ningún viaje activo
        public void EliminarVehiculo(string codigo)
        {
            Vehiculo v = BuscarVehiculo(codigo);

            if (v == null)
                throw new Exception("Vehículo no encontrado.");

            foreach (Viaje via in viajes)
            {
                if (via.ObtenerVehiculo() == v && !via.Finalizado)
                {
                    throw new EliminacionAsignadaException(
                        "No se puede eliminar: el vehículo está asignado a un viaje activo.");
                }
            }

            vehiculos.Remove(v);
            GuardarVehiculosCSV();
        }

        // ===========================================================
        // MÉTODOS DE PERSISTENCIA EN ARCHIVOS CSV
        // ===========================================================

        public void GuardarChoferesCSV()
        {
            using (StreamWriter sw = new StreamWriter("choferes.csv", false))
            {
                foreach (Chofer c in choferes)
                {
                    sw.WriteLine(c.Nombre + ";" + c.SueldoBasico + ";" + c.Asignado);
                }
            }
        }

        public void GuardarVehiculosCSV()
        {
            using (StreamWriter sw = new StreamWriter("vehiculos.csv", false))
            {
                foreach (Vehiculo v in vehiculos)
                {
                    sw.WriteLine(v.CodigoInterno + ";" + v.GetType().Name + ";" + v.Capacidad + ";" + v.Disponible);
                }
            }
        }

        public void CargarChoferesCSV()
        {
            if (!File.Exists("choferes.csv"))
                return;

            choferes.Clear();
            using (StreamReader sr = new StreamReader("choferes.csv"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] partes = linea.Split(';');
                    if (partes.Length >= 3)
                    {
                        string nombre = partes[0];
                        double sueldo = double.Parse(partes[1]);
                        bool asignado = bool.Parse(partes[2]);
                        Chofer c = new Chofer(nombre, "Sin dirección", "Soltero", DateTime.Now.AddYears(-30), sueldo);
                        c.Asignado = asignado;
                        choferes.Add(c);
                    }
                }
            }
        }

        public void CargarVehiculosCSV()
        {
            if (!File.Exists("vehiculos.csv"))
                return;

            vehiculos.Clear();
            using (StreamReader sr = new StreamReader("vehiculos.csv"))
            {
                string linea;
                while ((linea = sr.ReadLine()) != null)
                {
                    string[] partes = linea.Split(';');
                    if (partes.Length >= 4)
                    {
                        string codigo = partes[0];
                        string tipo = partes[1];
                        double capacidad = double.Parse(partes[2]);
                        bool disponible = bool.Parse(partes[3]);

                        Vehiculo v;
                        if (tipo == "Furgon")
                            v = new Furgon(codigo, "XXX000", capacidad, 0, 0);
                        else
                            v = new MotoReparto(codigo, "XXX000", capacidad, 0, 0);

                        v.Disponible = disponible;
                        vehiculos.Add(v);
                    }
                }
            }
        }

        // ===========================================================
        // REPORTES (punto 6 del Trabajo Práctico)
        // ===========================================================
        public void ReporteChoferes()
        {
            Console.WriteLine("\n=== LISTADO DE CHOFERES ===");

            foreach (Chofer c in choferes)
            {
                int viajesAsignados = 0;
                foreach (Viaje v in viajes)
                {
                    foreach (Chofer ch in v.ObtenerChoferes())
                    {
                        if (ch == c)
                            viajesAsignados++;
                    }
                }

                Console.WriteLine("Nombre: {0} | Edad: {1} | Sueldo: ${2} | Viajes: {3}",
                    c.Nombre, c.CalcularEdad(), c.SueldoBasico, viajesAsignados);
            }
        }

        public void ReporteVehiculos()
        {
            Console.WriteLine("\n=== LISTADO DE VEHÍCULOS ===");

            foreach (Vehiculo v in vehiculos)
            {
                string estado = v.Disponible ? "Disponible" : "En viaje";
                Console.WriteLine("Tipo: {0} | Código: {1} | Capacidad: {2}kg | Estado: {3}",
                    v.GetType().Name, v.CodigoInterno, v.Capacidad, estado);
            }
        }

        public void ReporteViajesPorFecha(DateTime fecha)
        {
            Console.WriteLine("\n=== VIAJES PROGRAMADOS PARA {0} ===", fecha.ToShortDateString());
            bool hayViajes = false;

            foreach (Viaje v in viajes)
            {
                if (v.Fecha.Date == fecha.Date)
                {
                    hayViajes = true;
                    Console.WriteLine(v.ToString());
                }
            }

            if (!hayViajes)
                Console.WriteLine("No hay viajes programados para esa fecha.");
        }

        // ===========================================================
        // CÁLCULOS GENERALES
        // ===========================================================
        public double CalcularTotalSueldos()
        {
            double total = 0;
            foreach (Chofer c in choferes)
                total += c.SueldoBasico;
            return total;
        }

        public double CalcularTotalFacturacion()
        {
            double total = 0;
            foreach (Viaje v in viajes)
                total += v.CalcularCostoTotal();
            return total;
        }
    }
}
