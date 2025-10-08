# TP Final - Programación con Objetos (C# / .NET 4.0)

Trabajo práctico final de la materia **Programación con Objetos**, Universidad Nacional Arturo Jauretche (UNAJ), 2025.

## Integrantes
- Mauricio Ariel Quinterno
- Santiago Acosta
- Javier Bobadilla

## Descripción
Sistema de gestión de transporte con choferes, vehículos y viajes.  
Aplica conceptos de **herencia, composición, polimorfismo y excepciones personalizadas**.

## Entorno
- SharpDevelop 4.4.1.9729  
- .NET Framework 4.0  
- Lenguaje: C#

## Estructura
- `Empresa.cs`: clase principal de administración  
- `Viaje.cs`: gestiona viajes y validaciones  
- `Vehiculo.cs`, `Furgon.cs`, `MotoReparto.cs`: jerarquía polimórfica  
- `Chofer.cs`: entidad base del sistema  
- Excepciones personalizadas: `ChoferOcupadoException`, `EliminacionAsignadaException`, `CapacidadExcedidaException`

## UML
El diagrama de clases se incluye en `/UML-Grupo 1 - Enunciado 1.png`

## Ejecución
Compilar el proyecto en SharpDevelop o Visual Studio con .NET Framework 4.0 y ejecutar `Program.cs`.



# TP1-UNAJ-POO-2025-QUINTERNO-ACOSTA-BOBADILLA
Trabajo practico Enunciado numero 1 - Grupo 1.
Pautas generales:

1-	Los grupos deben estar conformados por 3 integrantes.
2-	Cuando el proyecto se encuentra finalizado, es decir compila y ejecuta, se entrega en clase presencial (de ser posible) para mostrarle al docente su ejecución. Luego se debe enviar por mail o compartir por Drive, cada uno de los archivos que contienen la definición de una clase, y la aplicación, más un archivo con el diagrama de clases UML.
3-	En caso de estar aprobado, el docente pautará la fecha del coloquio grupal oral. El mismo podrá ser presencial o virtual.
4-	Si el proyecto NO compila o no ejecuta, no se acepta la entrega. Las consultas son previas a la entrega.
Desarrollo: En cada proyecto se espera que:

•	Se haga el diagrama de clases UML.
•	Se diseñe e implemente cada una de las clases completas: variables de instancia, constructores, propiedades y métodos de instancia.
•	Se desarrolle el menú de opciones solicitado en el problema y se implementan las funciones que dan respuesta a esos ítems. En la mayoría de los casos, deben desarrollar funciones del Main que invocan a los métodos de las clases diseñadas.
•	La aplicación debe plantear la lógica de resolución del problema haciendo uso de las clases intervinientes.
•	Deben usar herencia que se verá reflejado en el código y en el diagrama UML.
•	Es obligación que implementen al menos un manejador de excepciones, que se pide en forma explícita en cada proyecto. Si desean agregar otros, no hay problema.
 
Enunciado 1

Empresa de Transporte y Gestión de Flota

Una empresa de transporte administra una flota de vehículos, viajes y personal de conducción para realizar envíos de carga a lo largo del país. La empresa tiene nombre, CUIT y un conjunto de recursos: empleados (choferes), una flota de vehículos y un catálogo de rutas programadas. De cada chofer se registra nombre, dirección, estado civil, fecha de nacimiento y sueldo básico. Además, cada chofer puede calcular su edad a partir de su fecha de nacimiento y puede estar asignado a un viaje. La flota está compuesta por vehículos de distinto tipo: furgones y motos de reparto. Cada vehículo tiene además un código interno, matrícula, capacidad de carga kilometraje, costo base y estado.
La empresa organiza viajes: cada viaje tiene un código asignado, la ruta a cubrir (origen y destino), distancia estimada, fecha programada, la carga total a trasladar y la asignación de un vehículo y uno o más choferes. El costo total de un viaje se compone del costo operativo del vehículo y del sueldo de el/los conductores designados. En los furgones, el costo operativo del vehículo se calcula tomando el costo base que es de
$10000 y se multiplica por el porcentaje de carga transportada respecto de la capacidad total del vehículo. En las motos de reparto, el costo operativo se calcula tomando el costo base que es de $5000 y ajustándolo en función de la distancia recorrida y del porcentaje de carga transportada.
Antes de asignar un vehículo a un viaje se deben verificar que la carga a transportar no exceda la capacidad del vehículo y que el chofer no esté asignado a otro viaje, en caso de incumplimiento se lanzará la excepción ChoferOcupadoException que impida la asignación. Además, si durante la programación de viajes se intenta asignar una carga que supera la capacidad del vehículo se debe levantar la excepción CapacidadExcedidaException.
Por cada viaje programado se debe generar un Informe de Viaje (en formato .csv) que contenga: identificación del viaje, vehículo utilizado, choferes, kilometraje recorrido y costos desglosados. Ademas, se debe persistir la información relativa a la flota y los choferes.
Por otro lado, la empresa calcula semanalmente: monto total a desembolsar en sueldos netos de los choferes, monto total de costos operativos estimados de la flota para la semana, y total facturable por viajes realizados en un período.
 
1.	Menú principal y submenú

2.	Registrar chofer nuevo y persistirlo.
3.	Registrar vehículo nuevo (furgón/moto) y persistir la información cargada.
4.	Planificar viaje.
5.	Eliminar chofer/vehículo (solo si no están asignados a viajes activos, si lo están, lanzar EliminacionAsignadaException).
6.	Acceder al Submenú de impresión / reportes.
o	Listado de choferes: mostrar nombre, edad, sueldo básico y viajes asignados
o	Listado de vehículos por estado (disponible o en viaje): mostrar tipo, matrícula, capacidad y kilometraje.
o	Viajes programados por fecha: mostrar código, ruta, distancia, vehículo y choferes asignados y costo estimado.
7.	Salir.
