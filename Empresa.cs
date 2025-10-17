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

            // cargar viajes y restaurar asignaciones (chofer/vehículo)
            CargarViajesCSV();
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

            // persistir estados tras la asignación del viaje
            GuardarChoferesCSV();    // guarda Asignado (chofer ocupado/libre)
            GuardarVehiculosCSV();   // guarda Disponible (vehículo en viaje/disponible)

            // persistir el viaje (para restaurar al reiniciar)
            GuardarViajesCSV();
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

            // bloquear si el estado persistido indica que está asignado
            if (c.Asignado)
                throw new EliminacionAsignadaException(
                    "No se puede eliminar: el chofer está asignado a un viaje activo.");

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

            // mantener coherencia de viajes (si hiciera falta) y persistir
            GuardarViajesCSV();
        }

        // Solo se permite eliminar si el vehículo no participa en ningún viaje activo
        public void EliminarVehiculo(string codigo)
        {
            Vehiculo v = BuscarVehiculo(codigo);

            if (v == null)
                throw new Exception("Vehículo no encontrado.");

            // bloquear si el estado persistido indica que NO está disponible
            if (!v.Disponible)
                throw new EliminacionAsignadaException(
                    "No se puede eliminar: el vehículo está asignado a un viaje activo.");

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

            // mantener coherencia de viajes (si hiciera falta) y persistir
            GuardarViajesCSV();
        }

        // ===========================================================
        // PERSISTENCIA EN ARCHIVOS CSV
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
                        double sueldo = 0;
                        double.TryParse(partes[1], out sueldo);
                        bool asignado = false;
                        bool.TryParse(partes[2], out asignado);

                        // Campos no persistidos se rellenan con valores por defecto
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

                        double capacidad = 0;
                        double.TryParse(partes[2], out capacidad);

                        bool disponible = true;
                        bool.TryParse(partes[3], out disponible);

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
        // PERSISTENCIA DE VIAJES (solo datos necesarios para asignaciones)
        // Formato por línea:
        // CodigoViaje;Fecha(yyyy-MM-dd);CodigoVehiculo;Chofer1|Chofer2|...
        // ===========================================================
        private void GuardarViajesCSV()
        {
            using (StreamWriter sw = new StreamWriter("viajes.csv", false))
            {
                foreach (Viaje v in viajes)
                {
                    string codVeh = (v.ObtenerVehiculo() != null) ? v.ObtenerVehiculo().CodigoInterno : "";
                    List<Chofer> chs = v.ObtenerChoferes();
                    string nombresChoferes = "";
                    for (int i = 0; i < chs.Count; i++)
                    {
                        nombresChoferes += chs[i].Nombre;
                        if (i < chs.Count - 1) nombresChoferes += "|";
                    }
                    sw.WriteLine(
                        v.Codigo + ";" +
                        v.Fecha.ToString("yyyy-MM-dd") + ";" +
                        codVeh + ";" +
                        nombresChoferes
                    );
                }
            }
        }

private void CargarViajesCSV()
{
    if (!File.Exists("viajes.csv"))
        return;

    // 1) Partimos de un estado limpio en memoria
    viajes.Clear();
    foreach (Chofer c in choferes) c.Asignado = false;
    foreach (Vehiculo vv in vehiculos) vv.Disponible = true;

    using (StreamReader sr = new StreamReader("viajes.csv"))
    {
        string linea;
        while ((linea = sr.ReadLine()) != null)
        {
            string[] partes = linea.Split(';');
            if (partes.Length >= 4)
            {
                string cod = partes[0];

                DateTime fec = DateTime.Now;
                DateTime.TryParse(partes[1], out fec);

                string codVeh = partes[2];
                string choferesStr = partes[3];

                // Creamos un Viaje con placeholders para campos no persistidos
                Viaje vj = new Viaje(cod, "N/D", "N/D", 0, fec, 0);

                // 2) Reasignar vehículo (habilitando temporalmente para no disparar excepción)
                if (!string.IsNullOrEmpty(codVeh))
                {
                    Vehiculo veh = BuscarVehiculo(codVeh);
                    if (veh != null)
                    {
                        veh.Disponible = true;    // habilitamos temporalmente
                        vj.AsignarVehiculo(veh);   // el método lo dejará en false (en viaje)
                    }
                }

                // 3) Reasignar choferes (habilitando temporalmente para no disparar excepción)
                if (!string.IsNullOrEmpty(choferesStr))
                {
                    string[] nombres = choferesStr.Split('|');
                    foreach (string nom in nombres)
                    {
                        Chofer ch = BuscarChofer(nom);
                        if (ch != null)
                        {
                            ch.Asignado = false;    // habilitamos temporalmente
                            vj.AgregarChofer(ch);   // el método lo dejará en true (ocupado)
                        }
                    }
                }

                viajes.Add(vj);
            }
        }
    }

    // 4) Persistimos los estados recalculados al finalizar la carga
    GuardarChoferesCSV();
    GuardarVehiculosCSV();
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
