using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Video_ClubDB_V2
{
    class Reserva
    {
        public SqlConnection connection = new SqlConnection("Data Source=LAPTOP-93TLR9QC\\SQLEXPRESS; Initial Catalog = Video-Club; Integrated Security = True");
        public Usuario UsuarioID { get; set; }
        public Pelicula PeliculaID { get; set; }
        public DateTime FechaAlquiler { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public Reserva()
        {

        }
        public Reserva(Usuario usuarioid, Pelicula peliculaid, DateTime fechaalquiler, DateTime fechadevolucion)
        {
            UsuarioID = usuarioid;
            PeliculaID = peliculaid;
            FechaAlquiler = fechaalquiler;
            FechaDevolucion = fechadevolucion;
        }

        //Conseguir ver las películas consultando a la BBDD teniendo en cuenta la edad del usuario mediante 
        //la cláusula where.
        public List<Pelicula> ShowPeliculaFilt(string email)
        {
            Usuario per = new Usuario();
            int edad = per.GetYearsUser(email);
            List<Pelicula> filmo = new List<Pelicula>();
            connection.Open();
            string query00 = $"select Titulo,Sinopsis,EdadRecomendada,Estado from Peliculas where Edadrecomendada<={edad}";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            while (reader.Read())
            {
                Pelicula peliculon = new Pelicula(reader["Titulo"].ToString(), reader["Sinopsis"].ToString(), Convert.ToInt32(reader["EdadRecomendada"]), reader["Estado"].ToString());
                filmo.Add(peliculon);
            }
            connection.Close();
            return filmo;
        }
        //Filtro Id muestra indices en función del filtro.
        public List<int> FiltroID(List<Pelicula> peli)
        {
            List<int> ides = new List<int>();
            for (int i = 1; i <= peli.Count; i++)
            {
                ides.Add(i);
            }
            return ides;
        }
        //Mostrar los datos de la pelicula seleccionada dentro de una lista
        public void MostrarPel(List<Pelicula> peli, int r)
        {
            Console.Clear();
            Pelicula peliculaSelec = peli[r - 1];   //Buscon el indice que he mostrado en pantalla
            connection.Open();
            string query00 = $"select*from Peliculas where Titulo='{peliculaSelec.Titulo}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                Console.WriteLine($"{reader["Titulo"]}\n~{reader["Sinopsis"]}~ -PG- {reader["EdadRecomendada"]} Estado--> {reader["Estado"]}");
            }
            connection.Close();
        }
        //Genereación de alquiler seleccionando los datos de una pelicula
        public void GenerarAlquiler(List<Pelicula> peli, int r, string usuario)
        {
            Reserva op = new Reserva();
            //Obtengo la pelicula
            Pelicula peliculaSelec = peli[r - 1];   //Buscon el indice que he mostrado en pantalla
            //Modifico el estado de la pelicula
            connection.Open();
            //Modificar la pelicula
            string query0 = $"Update Peliculas Set Estado = 'Ocupada' where Titulo = '{peliculaSelec.Titulo}'";
            SqlCommand command0 = new SqlCommand(query0, connection);
            command0.ExecuteNonQuery();
            connection.Close();
            DateTime date = DateTime.Now;   //Fecha Alquiler
            //Busco ID pelicula
            int idpeli = op.GetPeliculaID(peliculaSelec.Titulo);
            //Busco ID persona
            int idusuario = op.GetUsuarioID(usuario);
            //Genero la reserva
            connection.Open();
            string queryreser = $"Insert into Alquileres(UsuarioID,PeliculaID,FechaAlquiler) VALUES('{idusuario}','{idpeli}','{date}')";
            SqlCommand commandreser = new SqlCommand(queryreser, connection);
            commandreser.ExecuteNonQuery();
            connection.Close();
        }
        //Muestra las peliculas disponibles para alquilar en un usuario
        public List<Pelicula> ShowPeliculaAlq(string email)
        {
            Usuario per = new Usuario();
            int edad = per.GetYearsUser(email);
            List<Pelicula> filmo = new List<Pelicula>();
            connection.Open();
            string query00 = $"select Titulo,Sinopsis,EdadRecomendada,Estado from Peliculas where Edadrecomendada<={edad} and Estado='Disponible'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            while (reader.Read())
            {
                Pelicula peliculon = new Pelicula(reader["Titulo"].ToString(), reader["Sinopsis"].ToString(), Convert.ToInt32(reader["EdadRecomendada"]), reader["Estado"].ToString());
                filmo.Add(peliculon);
            }
            connection.Close();
            return filmo;
        }
        //Da el UsuarioID
        public int GetUsuarioID(string usuario)
        {
            int z = 0;
            connection.Open();
            string query00 = $"select UsuarioID from Usuarios where Email='{usuario}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                z = Convert.ToInt32(reader["UsuarioID"]);
            }
            connection.Close();
            return z;
        }
        //Da la peliculaID
        public int GetPeliculaID(string titulo)
        {
            int z = 0;
            connection.Open();
            string query00 = $"select PeliculaID from Peliculas where Titulo='{titulo}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                z = Convert.ToInt32(reader["PeliculaID"]);
            }
            connection.Close();
            return z;
        }
        //Muestra las peliculas que tenemos en curso
        public List<Pelicula> ShowTusPeliculas(string email)
        {
            Reserva op = new Reserva();
            int id = op.GetUsuarioID(email);
            List<Pelicula> filmo = new List<Pelicula>();
            List<int> IdPeliculas = new List<int>();
            //Saco el 
            connection.Open();
            string query00 = $"select*from Alquileres where UsuarioID={id} and FechaDevolucion is null";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            while (reader.Read())
            {
                IdPeliculas.Add(Convert.ToInt32(reader["PeliculaID"]));
            }
            connection.Close();
            //Guardo las peliculas
            foreach (int identificador in IdPeliculas)
            {
                connection.Open();
                string query0 = $"select*from Peliculas where PeliculaID={identificador}";
                SqlCommand command0 = new SqlCommand(query0, connection);
                SqlDataReader readerE = command0.ExecuteReader();
                while (readerE.Read())
                {
                    Pelicula peliculon = new Pelicula(readerE["Titulo"].ToString(), readerE["Sinopsis"].ToString(), Convert.ToInt32(readerE["EdadRecomendada"]), readerE["Estado"].ToString());
                    filmo.Add(peliculon);
                }
                connection.Close();
            }
            return filmo;
        }
        //Genera la devolución final
        public void GenerarDevolucion(List<Pelicula> peli, int r, string usuario)
        {
            Reserva op = new Reserva();
            //Obtengo la pelicula
            Pelicula peliculaSelec = peli[r - 1];   //Buscon el indice que he mostrado en pantalla
            //Modifico el estado de la pelicula
            connection.Open();
            //Modificar el estado
            string query0 = $"Update Peliculas Set Estado = 'Disponible' where Titulo = '{peliculaSelec.Titulo}'";
            SqlCommand command0 = new SqlCommand(query0, connection);
            command0.ExecuteNonQuery();
            connection.Close();
            DateTime date = DateTime.Now;   //Fecha Alquiler
            //Busco ID pelicula
            int idpeli = op.GetPeliculaID(peliculaSelec.Titulo);
            //Busco ID persona
            int idusuario = op.GetUsuarioID(usuario);
            //Modifico la fecha
            connection.Open();
            string queryreser = $"Update Alquileres Set FechaDevolucion='{date}' where PeliculaID='{idpeli}' and UsuarioID='{idusuario}'";
            SqlCommand commandreser = new SqlCommand(queryreser, connection);
            commandreser.ExecuteNonQuery();
            connection.Close();
            //Dar aviso en caso de que se incumple el limite de tres dias de alquiler
            DateTime fecha1 = DateTime.Now;
            DateTime fecha2 = DateTime.Now;
            connection.Open();
            string querycond = $"Select FechaAlquiler,FechaDevolucion from Alquileres where PeliculaID='{idpeli}' and UsuarioID='{idusuario}'and FechaDevolucion='{date}'";
            SqlCommand commandcon = new SqlCommand(querycond, connection);
            SqlDataReader reader = commandcon.ExecuteReader();
            while (reader.Read())
            {
                fecha1 = Convert.ToDateTime(reader["FechaAlquiler"]);
                fecha2 = Convert.ToDateTime(reader["FechaDevolucion"]);
            }
            connection.Close();
            TimeSpan Ts = fecha2 - fecha1;
            int difference = Ts.Days;
            if (difference > 3)
            {
                //Ponemos el color a rojo
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Se ha superado el limite de días fecha alquiler {fecha1.ToString("yyyy/MM/dd")} y fecha devolucion {fecha2.ToString("yyyy/MM/dd")}");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
