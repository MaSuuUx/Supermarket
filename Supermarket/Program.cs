using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;


class Program
{
    // Estructura
    struct Sucursal
    {
        public string ID;
        public string Nombre;
        public string Direccion;
        public string Telefono;
        public int NumEmpleados;
    }

    // Nombre del archivo CSV
    static string archivoCSV = "supermarket.csv";
    static List<Sucursal> sucursales = new List<Sucursal>();

    static void Main()
    {
        bool salir = false;
        CargarDatos();

        //Titulo
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("               Supermercado");
        Console.WriteLine("------------------------------------------\n");

        //Bucle del menu
        while (!salir)
        {
            //Entrada de datos
            Console.WriteLine("          Opciones");
            Console.WriteLine("1. Ingresar datos");
            Console.WriteLine("2. Imprimir datos");
            Console.WriteLine("3. Abrir Excel");
            Console.WriteLine("4. Registros del día");
            Console.WriteLine("5. Cerrar");
            Console.Write("Ingrese su opción: ");
            int opcion = Int32.Parse(Console.ReadLine());

            //Estructura selectiva
            switch (opcion)
            {
                case 1:
                    Console.Clear();
                    IngresarDatos();
                    GuardarDatos();
                    break;
                case 2:
                    Console.Clear();
                    VisualizarArchivo();
                    break;
                case 3:
                    Console.Clear();
                    AbrirExcel();
                    break;
                case 4:
                    Console.Clear();
                    MostrarRegistrosDelDia();
                    break;
                case 5:
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida. Vuelva a intentar......");
                    break;
            }
        }
    }

    // Carga los datos existentes en el archivo CSV y los almacena en la lista de sucursales
    static void CargarDatos()
    {
        // Verifica si el archivo CSV existe
        if (File.Exists(archivoCSV))
        {
            try
            {
                // Crea un objeto StreamReader para leer el archivo CSV
                using (StreamReader sr = new StreamReader(archivoCSV))
                {
                    string linea;

                    // Lee cada línea del archivo
                    while ((linea = sr.ReadLine()) != null)
                    {
                        // Divide la línea en campos utilizando la pleca vertical como separador
                        string[] campos = linea.Split(',');

                        Sucursal sucursal = new Sucursal();

                        // Asigna los valores de los campos al objeto Sucursal
                        sucursal.ID = campos[0];
                        sucursal.Nombre = campos[1];
                        sucursal.Direccion = campos[2];
                        sucursal.Telefono = campos[3];
                        sucursal.NumEmpleados = Convert.ToInt32(campos[4]);

                        // Agrega el objeto Sucursal a la lista sucursales
                        sucursales.Add(sucursal);
                    }
                }
            }
            catch (Exception ex)
            {
                //Muestra cualquier excepción ocurrida durante la lectura del archivo
                Console.WriteLine("Error al cargar los datos del archivo: " + ex.Message);
            }
        }
    }


    // Permite al usuario ingresar datos de una nueva sucursal y los agrega a la lista de sucursales
    static void IngresarDatos()
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine("Ingrese los datos de la sucursal:");

        Sucursal sucursal = new Sucursal();

        //Entrada de datos
        Console.Write("\nIngrese el ID de la Sucursal: ");
        sucursal.ID = Console.ReadLine();

        Console.Write("\nIngrese el Nombre de la sucursal: ");
        sucursal.Nombre = Console.ReadLine();

        Console.Write("\nIngrese la direccion de la sucursal: ");
        sucursal.Direccion = Console.ReadLine();

        Console.Write("\nIngrese el número de Teléfono de la sucursal: ");
        sucursal.Telefono = Console.ReadLine();

        Console.Write("\nIngrese el No. de Empleados de la sucursal: ");
        sucursal.NumEmpleados = Convert.ToInt32(Console.ReadLine());

        sucursales.Add(sucursal);

        Console.WriteLine("Datos ingresados correctamente.");
        Console.WriteLine("------------------------------------------");
    }

    // Guarda los datos de las sucursales en el archivo CSV
    static void GuardarDatos()
    {
        try
        {
            // Crea un objeto StreamWriter para escribir en el archivo CSV, "true" indica que se agregan datos al archivo
            using (StreamWriter sw = new StreamWriter(archivoCSV, true))
            {
                // Itera sobre cada objeto Sucursal en la lista sucursales
                foreach (Sucursal sucursal in sucursales)
                {
                    // Escribe una línea en el archivo CSV con los valores de los atributos del objeto Sucursal
                    sw.WriteLine($"{sucursal.ID},{sucursal.Nombre},{sucursal.Direccion},{sucursal.Telefono},{sucursal.NumEmpleados}");
                }
            }
        }
        catch (Exception ex)
        {
            //Muestra cualquier excepción ocurrida durante la escritura en el archivo
            Console.WriteLine("Error al guardar los datos en el archivo: " + ex.Message);
            Console.WriteLine("------------------------------------------"); 
        }
    }

    // Muestra los datos almacenados en el archivo CSV en la consola
    static void VisualizarArchivo()
    {
        Console.WriteLine("Datos del archivo:");

        try
        {
            // Crea un objeto StreamReader para leer el archivo CSV
            using (StreamReader sr = new StreamReader(archivoCSV))
            {
                string linea;

                // Lee cada línea del archivo y la muestra en la consola
                while ((linea = sr.ReadLine()) != null)
                {

                    Console.WriteLine(linea);
                    Console.WriteLine("------------------------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            //Muestra cualquier excepción ocurrida durante la lectura del archivo
            Console.WriteLine("Error al visualizar el archivo: " + ex.Message);
            Console.WriteLine("------------------------------------------");
        }
    }


    // Abre el archivo CSV en Excel utilizando el programa predeterminado asociado con archivos CSV en el sistema operativo
    static void AbrirExcel()
    {
        try
        {
            // Abre el archivo CSV 
            System.Diagnostics.Process.Start(archivoCSV);
        }
        catch (Exception ex)
        {
            //Muestra cualquier excepción ocurrida al intentar abrir el archivo en Excel
            Console.WriteLine("Error al abrir el archivo en Excel: " + ex.Message);
            Console.WriteLine("------------------------------------------");
        }
    }


    //Muestra los registros de las sucursales creados en el mismo día que el archivo CSV
    static void MostrarRegistrosDelDia()
    {
        Console.WriteLine("Registros del día:");

        DateTime fechaActual = DateTime.Today;

        // Filtra los registros de sucursales que tengan la misma fecha de creación que la fecha actual
        var registrosDelDia = sucursales.Where(s => File.GetCreationTime(archivoCSV).Date == fechaActual);

        // Verifica si existen registros del día
        if (registrosDelDia.Any())
        {
            // Itera sobre cada objeto Sucursal en los registros del día y muestra sus atributos en la consola
            foreach (Sucursal sucursal in registrosDelDia)
            {
                Console.WriteLine($"ID: {sucursal.ID}, Nombre: {sucursal.Nombre}, Dirección: {sucursal.Direccion}, Teléfono: {sucursal.Telefono}, No. Empleados: {sucursal.NumEmpleados}");
                Console.WriteLine("------------------------------------------");
            }
        }
        else
        {
            // Si no hay registros del día, imprime un mensaje indicando que no hay registros para mostrar
            Console.WriteLine("No hay registros para mostrar.");
            Console.WriteLine("------------------------------------------");
        }
    }

}




