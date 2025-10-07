using System;

namespace TransporteApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            // Creación de la empresa (Clase 5 - Composición)
            Empresa empresa = new Empresa("Transporte Quilmes", "30-12345678-9");
            int opcion = 0;

            // Bucle principal (Clase 2 - estructuras de control)
            do
            {
                Console.Clear();
                Console.WriteLine("=== SISTEMA DE GESTIÓN EMPRESARIAL ===");
                Console.WriteLine("1. Registrar Chofer");
                Console.WriteLine("2. Registrar Vehículo");
                Console.WriteLine("3. Planificar Viaje");
                Console.WriteLine("4. Listar Choferes");
                Console.WriteLine("5. Listar Viajes por Fecha");
                Console.WriteLine("6. Submenú de Reportes");
                Console.WriteLine("7. Eliminar Chofer");
                Console.WriteLine("8. Eliminar Vehículo");
                Console.WriteLine("0. Salir");
                Console.WriteLine("--------------------------------------");
                Console.Write("Seleccione una opción: ");

                try
                {
                    opcion = int.Parse(Console.ReadLine());
                }
                catch
                {
                    opcion = -1; // si el usuario ingresa texto
                }

                Console.Clear();

                try
                {
                    switch (opcion)
                    {
                        // =======================================================
                        // 1️⃣ REGISTRAR CHOFER
                        // =======================================================
                        case 1:
                            Console.WriteLine("=== REGISTRAR NUEVO CHOFER ===");
                            Console.Write("Nombre: ");
                            string n = Console.ReadLine();

                            Console.Write("Dirección: ");
                            string d = Console.ReadLine();

                            Console.Write("Estado civil: ");
                            string e = Console.ReadLine();

                            Console.Write("Fecha de nacimiento (aaaa-mm-dd): ");
                            DateTime fn = DateTime.Parse(Console.ReadLine());

                            Console.Write("Sueldo básico: ");
                            double s = double.Parse(Console.ReadLine());

                            empresa.RegistrarChofer(new Chofer(n, d, e, fn, s));

                            Console.WriteLine("\nChofer agregado correctamente.");
                            break;

                        // =======================================================
                        // 2️⃣ REGISTRAR VEHÍCULO
                        // =======================================================
                        case 2:
                            Console.WriteLine("=== REGISTRAR VEHÍCULO ===");
                            Console.Write("Código interno: ");
                            string cod = Console.ReadLine();

                            Console.Write("Patente: ");
                            string pat = Console.ReadLine();

                            Console.Write("Capacidad de carga (kg): ");
                            double cap = double.Parse(Console.ReadLine());

                            Console.Write("Kilometraje: ");
                            double km = double.Parse(Console.ReadLine());

                            Console.Write("Costo base: ");
                            double cb = double.Parse(Console.ReadLine());

                            Console.Write("Tipo de vehículo (1 = Furgón, 2 = Moto de Reparto): ");
                            int tipo = int.Parse(Console.ReadLine());

                            Vehiculo v;
                            if (tipo == 1)
                                v = new Furgon(cod, pat, cap, km, cb);
                            else
                                v = new MotoReparto(cod, pat, cap, km, cb);

                            empresa.RegistrarVehiculo(v);
                            Console.WriteLine("\nVehículo registrado correctamente.");
                            break;

                        // =======================================================
                        // 3️⃣ PLANIFICAR VIAJE
                        // =======================================================
                        case 3:
                            Console.WriteLine("=== PLANIFICAR VIAJE ===");
                            Console.Write("Código de viaje: ");
                            string codviaje = Console.ReadLine();

                            Console.Write("Origen: ");
                            string origen = Console.ReadLine();

                            Console.Write("Destino: ");
                            string destino = Console.ReadLine();

                            Console.Write("Distancia (km): ");
                            double dist = double.Parse(Console.ReadLine());

                            Console.Write("Carga (kg): ");
                            double carga = double.Parse(Console.ReadLine());

                            Viaje viaje = new Viaje(codviaje, origen, destino, dist, DateTime.Now, carga);

                            Console.Write("Código del vehículo asignado: ");
                            string codVehiculo = Console.ReadLine();
                            Vehiculo veh = empresa.BuscarVehiculo(codVehiculo);
                            viaje.AsignarVehiculo(veh);

                            Console.Write("Nombre del chofer asignado: ");
                            string nomChofer = Console.ReadLine();
                            Chofer ch = empresa.BuscarChofer(nomChofer);
                            viaje.AgregarChofer(ch);

                            empresa.RegistrarViaje(viaje);
                            viaje.GenerarInformeCSV();

                            Console.WriteLine("\nViaje planificado correctamente.");
                            break;

                        // =======================================================
                        // 4️⃣ LISTAR CHOFERES
                        // =======================================================
                        case 4:
                            Console.WriteLine("=== LISTADO DE CHOFERES ===");
                            empresa.ReporteChoferes();
                            break;

                        // =======================================================
                        // 5️⃣ LISTAR VIAJES POR FECHA
                        // =======================================================
                        case 5:
                            Console.Write("Ingrese la fecha (aaaa-mm-dd): ");
                            DateTime f = DateTime.Parse(Console.ReadLine());
                            empresa.ReporteViajesPorFecha(f);
                            break;

                        // =======================================================
                        // 6️⃣ SUBMENÚ DE REPORTES
                        // =======================================================
                        case 6:
                            int subop = 0;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine("=== SUBMENÚ DE REPORTES ===");
                                Console.WriteLine("1. Listado de Choferes");
                                Console.WriteLine("2. Listado de Vehículos por Estado");
                                Console.WriteLine("3. Viajes Programados por Fecha");
                                Console.WriteLine("0. Volver al Menú Principal");
                                Console.WriteLine("----------------------------");
                                Console.Write("Seleccione una opción: ");

                                try { subop = int.Parse(Console.ReadLine()); }
                                catch { subop = -1; }

                                Console.Clear();

                                switch (subop)
                                {
                                    case 1:
                                        empresa.ReporteChoferes();
                                        break;
                                    case 2:
                                        empresa.ReporteVehiculos();
                                        break;
                                    case 3:
                                        Console.Write("Ingrese la fecha (aaaa-mm-dd): ");
                                        DateTime f2 = DateTime.Parse(Console.ReadLine());
                                        empresa.ReporteViajesPorFecha(f2);
                                        break;
                                    case 0:
                                        Console.WriteLine("Volviendo al menú principal...");
                                        break;
                                    default:
                                        Console.WriteLine("Opción inválida.");
                                        break;
                                }

                                Console.WriteLine("\nPresione una tecla para continuar...");
                                Console.ReadKey(true);

                            } while (subop != 0);
                            break;

                        // =======================================================
                        // 7️⃣ ELIMINAR CHOFER
                        // =======================================================
                        case 7:
                            Console.WriteLine("=== ELIMINAR CHOFER ===");
                            Console.Write("Ingrese el nombre del chofer a eliminar: ");
                            string nom = Console.ReadLine();

                            try
                            {
                                empresa.EliminarChofer(nom);
                                Console.WriteLine("Chofer eliminado correctamente.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            break;

                        // =======================================================
                        // 8️⃣ ELIMINAR VEHÍCULO
                        // =======================================================
                        case 8:
                            Console.WriteLine("=== ELIMINAR VEHÍCULO ===");
                            Console.Write("Ingrese el código interno del vehículo a eliminar: ");
                            string codelim = Console.ReadLine();

                            try
                            {
                                empresa.EliminarVehiculo(codelim);
                                Console.WriteLine("Vehículo eliminado correctamente.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Error: " + ex.Message);
                            }
                            break;

                        // =======================================================
                        // 0️⃣ SALIR
                        // =======================================================
                        case 0:
                            Console.WriteLine("Fin del programa. Gracias por usar el sistema.");
                            break;

                        default:
                            Console.WriteLine("Opción inválida. Intente nuevamente.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }

                Console.WriteLine("\nPresione una tecla para volver al menú...");
                Console.ReadKey(true);

            } while (opcion != 0);

            Console.ReadKey(true);
        }
    }
}
