namespace ClaseHilos
{
   internal class Producto
   {
      public string Nombre { get; set; }
      public decimal PrecioUnitarioDolares { get; set; }
      public int CantidadEnStock { get; set; }

      public Producto(string nombre, decimal precioUnitario, int cantidadEnStock)
      {
         Nombre = nombre;
         PrecioUnitarioDolares = precioUnitario;
         CantidadEnStock = cantidadEnStock;
      }
   }
   internal class Solution //reference: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock
   {

      static List<Producto> productos = new List<Producto>
        {
            new Producto("Camisa", 10, 50),
            new Producto("Pantalón", 8, 30),
            new Producto("Zapatilla/Champión", 7, 20),
            new Producto("Campera", 25, 100),
            new Producto("Gorra", 16, 10)
        };

      static int precio_dolar = 500;

      static readonly object precio_dolar_lock = new object();
      static readonly object productos_lock = new object();
      static readonly SemaphoreSlim semaphore = new SemaphoreSlim(0, 3);

        static void Tarea1()
        {
            lock (productos_lock)
            {
                foreach (var product in productos)
                {
                    product.CantidadEnStock += 10;
                }
            }
            semaphore.Release();
            Console.WriteLine("Tarea 1 completada: Stock actualizado.");
        }
        static void Tarea2()
        {
            lock (precio_dolar_lock)
            {
                precio_dolar = 1000;
            }
            semaphore.Release();
            Console.WriteLine("Tarea 2 completada: Precio del dólar actualizado.");
        }
      static void Tarea3()
      {
            for (int i = 0; i < 3; i++)
            {
                semaphore.Wait();
            }
            Console.WriteLine("Tarea 3 completada: Generación de informe.");
            decimal valorTotalInventario = 0;
            foreach (var product in productos)
               {
                        decimal precioEnMonedaLocal = product.PrecioUnitarioDolares * precio_dolar;
                        valorTotalInventario += product.CantidadEnStock * precioEnMonedaLocal;
                        Console.WriteLine($"Producto: {product.Nombre}, Stock: {product.CantidadEnStock}, Precio (en moneda local): {precioEnMonedaLocal}");
                }
             Console.WriteLine($"Valor total del inventario: {valorTotalInventario}");
        }

      static void Tarea4()
        {
        lock (productos_lock)
            {
                foreach (var product in productos)
                {
                    product.PrecioUnitarioDolares *= 1.1m;
                }
            }
            semaphore.Release();
            Console.WriteLine("Tarea 4 completada: Precios actualizados.");
        }

        internal static void Excecute()
      {
            Thread t1 = new Thread(Tarea1);
            Thread t2 = new Thread(Tarea2);
            Thread t3 = new Thread(Tarea3);
            Thread t4 = new Thread(Tarea4);

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
        }
   }
}