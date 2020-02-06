using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Video_ClubDB_V2
{
    class Program
    {

        public SqlConnection connection = new SqlConnection("Data Source=LAPTOP-93TLR9QC\\SQLEXPRESS; Initial Catalog = Video-Club; Integrated Security = True");
        static void Main(string[] args)
        {
            Pelicula film = new Pelicula();
            Reserva op = new Reserva();
            Admin admin = new Admin();
            List<Pelicula> listaShow = new List<Pelicula>();
            List<int> listaIdes = new List<int>();
            //Inicio
            Titulo();
            Console.WriteLine("Introduce el usuario y contraseña");
            string usuario00 = Console.ReadLine();
            string contrasenia00 = Console.ReadLine();
            Usuario user = new Usuario(usuario00,contrasenia00);
            bool t;
            int empl=0;
            int selOP=0;
            int selOption = 0;
            int selcOpUser = 0;
            //Zona Loggear
            do
            {
                t = user.UserIsLog(usuario00, contrasenia00);
                if (t == false)
                {
                    Console.WriteLine("Usuario incorrecto:\nPulsa s para volver a introducer contraseña \nPulsa n para crear nuevo usuario");
                    string conUs = Console.ReadLine();
                    if (conUs == "s")   //Si queremos volver a entrar
                    {
                        Console.Clear();
                        Console.WriteLine("Introduce el usuario y contraseña");
                        usuario00 = Console.ReadLine();
                        contrasenia00 = Console.ReadLine();
                        t = user.UserIsLog(usuario00, contrasenia00);
                        Titulo();
                        Console.WriteLine("Se vuelve a introducir");
                    }
                    else if (conUs == "n")   //Si queremos crear un nuevo usuario
                    {
                        Console.Clear();
                        user.CreateLogUser();
                        Console.WriteLine("Usuario creado");
                        t = true;
                    }
                    else
                    {
                        t = false;
                        Console.WriteLine("Login no Ok");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("LogIn OK");
                }

            } while (t == false);
            Admin user00 = new Admin(user);
            //Zona Admin,Empleados
            do
            {
                Titulo();
                Console.WriteLine("Selecciona tu Area:\n Area empleados (Pulsa 1)\n Area usuarios (Pulsa 2) ");
                //Try de area
                try
                {
                    selOP = Convert.ToInt32(Console.ReadLine());
                    if (selOP == 2) //Quiere las opciones de usuario
                    {
                        Console.Clear();
                        break;
                    }
                }
                catch (Exception)
                {

                    empl = 0;
                }
                //Zona Admin
                if (user00.UserIsAdmin(user00) == true && selOP==1)
                {
                    Console.Clear();
                    Console.WriteLine("Eres administrador: Tus Opciones son: \n");
                    user00.MenuOpAdmin();
                    Console.WriteLine("\n Introduce la opcion deseada");

                    try
                    {
                        selOption = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        empl = 0;
                    }
                    if (selOption == 1) //Show usuarios si opcion 1
                    {

                        Console.Clear();
                        Titulo();
                        user00.ShowAdminUsuarios();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nSelecciona 1 para generar usuarios o 2 para eliminar uno de la lista.\nPulsa otra tecla para salir");
                        Console.ForegroundColor = ConsoleColor.White;
                        try
                        {
                            selcOpUser=Convert.ToInt32(Console.ReadLine());
                            if (selcOpUser == 1)
                            {
                                user00.GenerarUsuarios(user00);
                            }
                            else if (selcOpUser == 2)
                            {
                                user00.BorrarUsuarios(user00);
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            empl = 0;
                        }
                        
                    }
                    else if (selOption == 2) //Show peliculas si opcion2
                    {
                        Console.Clear();
                        Titulo();
                        user00.ShowAdminPelicula();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nSelecciona 1 para generar peliculas o 2 para eliminar una pelicula de la lista.\nPulsa otra tecla para salir");
                        Console.ForegroundColor = ConsoleColor.White;
                        try
                        {
                            selcOpUser = Convert.ToInt32(Console.ReadLine());
                            if (selcOpUser == 1)
                            {
                                user00.GenerarPelicula(user00);
                            }
                            else if (selcOpUser == 2)
                            {
                                user00.BorrarPelicula(user00);
                            }
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            empl = 0;
                        }
                       
                    }
                    else if (selOption==3)
                    {
                        user00.HistorialMorosos(); //Mostrar morosos
                    }
                    else
                    {
                        empl = 0;
                    }
                }
                //Redirigir a zona Usuario si no tiene permisos
                else
                {
                    Console.WriteLine("Sin permisos......Redigiendo a zona usuarios");
                    empl = 1;
                }
            } while (empl == 0);
            //Zona BD
            int r;
            do
            {
                Titulo();
                Console.WriteLine($"\n Pulsa 1 para ver tus películas disponibles \n Pulsa 2 para alquilar una película disponible \n Pulsa 3 para ver tus alquileres y/o devolver \n Pulsa 4 para el logout");
                try
                {
                    r = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    r = 12;
                }
                switch (r)
                {
                    case 1:
                        Console.Clear();
                        Titulo();
                        Console.WriteLine("El listado disponible es");
                        Console.WriteLine($"Titulo\n");
                        //Obtenemos edad del usuario
                        listaShow = op.ShowPeliculaFilt(usuario00);
                        listaIdes = op.FiltroID(listaShow); //Ambas tiene misma longitud
                        int cont = 0;
                        foreach (Pelicula peliculon in listaShow)
                        {
                            Console.WriteLine($"{listaIdes[cont]}\t {peliculon.Titulo}");
                            cont++;
                        }
                        try
                        {
                            Console.WriteLine("Selecciona la película o pulsa cualquier otra tecla para continuar");
                            int selecc = Convert.ToInt32(Console.ReadLine());
                            op.MostrarPel(listaShow, selecc);

                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Loading....................");
                        }
                        break;
                    case 2:
                        Console.Clear();
                        Titulo();
                        Console.WriteLine("El listado de peliculas a alquilar es");
                        Console.WriteLine($"   Titulo\n");
                        //Obtenemos edad del usuario
                        listaShow = op.ShowPeliculaAlq(usuario00);
                        listaIdes = op.FiltroID(listaShow); //Ambas tiene misma longitud
                        int contad = 0;
                        foreach (Pelicula peliculon in listaShow)
                        {
                            Console.WriteLine($"{listaIdes[contad]}\t {peliculon.Titulo}");
                            contad++;
                        }
                        try
                        {

                            Console.WriteLine("Selecciona la película disponible o pulsa cualquier otra tecla para continuar");
                            int selecAlquiler = Convert.ToInt32(Console.ReadLine());
                            op.GenerarAlquiler(listaShow, selecAlquiler, usuario00);
                            Console.WriteLine("Pelicula alquilada");
                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Loading....................");
                        }
                        break;
                    case 3:
                        Console.Clear();
                        Titulo();
                        Console.WriteLine("El listado de tus peliculas es");
                        listaShow = op.ShowTusPeliculas(usuario00);
                        listaIdes = op.FiltroID(listaShow);
                        int contador = 0;
                        foreach (Pelicula peliculon in listaShow)
                        {
                            Console.WriteLine($"{listaIdes[contador]}\t {peliculon.Titulo}");
                            contador++;
                        }
                        Console.WriteLine("Selecciona la película a devolver o pulsa cualquier otra tecla para continuar");
                        int selecDevolucion;
                        try
                        {
                            selecDevolucion = Convert.ToInt32(Console.ReadLine());
                            op.GenerarDevolucion(listaShow, selecDevolucion, usuario00);
                            Console.WriteLine("Pelicula devuelta");

                        }
                        catch (Exception)
                        {
                            Console.Clear();
                            Console.WriteLine("Loading....................");
                        }
                        //Damos el aviso que imprime en pantalla que se ha superado los tres días de devolución
                        break;
                    case 4:
                        Console.WriteLine("Logout");
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("MENU");
                        break;
                }
            } while (r != 4);
        }
        public static void Titulo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("*****************************************");
            Console.WriteLine("Video-Club Retr0");
            Console.WriteLine("*****************************************");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
