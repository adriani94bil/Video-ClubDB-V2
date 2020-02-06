using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Video_ClubDB_V2
{
    class Admin:Usuario
    {
        public new SqlConnection connection = new SqlConnection("Data Source=LAPTOP-93TLR9QC\\SQLEXPRESS; Initial Catalog = Video-Club; Integrated Security = True");
        public Usuario User;
        public string Permisos;

        public Admin()
        {

        }
        public Admin(Usuario user)
        {
            User = user;
            Permisos = "";
            //connection.Open();
            //Modificar el usuario, ya que lo defino como Admin
            //string query0 = $"Update Usuarios Set Estado = '{perm}' where Email = '{user.Email.ToLower()}' and Contrasenia='{user.Contrasenia.ToLower()}'";
            //SqlCommand command0 = new SqlCommand(query0, connection);
            //command0.ExecuteNonQuery();
            //connection.Close();
        }
        //Cond
        public bool UserIsAdmin(Admin user)
        {
            bool t = false;
            connection.Open();
            string query00 = $"select*from Usuarios where Email='{user.User.Email.ToLower()}' and Contrasenia='{user.User.Contrasenia.ToLower()}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            while(reader.Read())
            {
                if (reader["Permisos"].ToString() == "Admin")
                {
                    t = true;

                }
            }
            connection.Close();
            return t;
        }
        //Menu
        public void MenuOpAdmin()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("MenuAdmin");
            Console.WriteLine("------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Pulsa 1 para mostrar los usuarios\nPulsa 2 para mostrar peliculas \nPulsa 3 para ver los alquileres desfasados");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void MenuUserAdmin()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Pulsa 1 crear usuarios \n Pulsa 2 eliminar usuarios");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void MenuFilmAdmin()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Pulsa 1 para mostrar peliculas crear usuarios \n Pulsa 2 eliminar usuarios");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //Acciones 
        //Usuarios
        public void ShowAdminUsuarios()
        {
            Usuario user = new Usuario();
            connection.Open();
            string query00 = $"select UsuarioID,Nombre,Apellido,Email,Contrasenia,FechaNacimiento from Usuarios";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            int t;
            while (reader.Read())
            {
                t = user.GetYearsUser(reader["Email"].ToString());
                Console.WriteLine($"{reader["UsuarioID"].ToString()}-->{reader["Nombre"].ToString()}, {reader["Apellido"].ToString()} Edad --{t}, User-->{reader["Email"].ToString()}, {reader["Contrasenia"].ToString()}");
            }
            connection.Close();
        }
        public void GenerarUsuarios(Admin user)
        {
            bool t = user.UserIsAdmin(user);
            if (t == true)
            {
                CreateLogUser();
            }
            else
            {
                Console.WriteLine("Sin permisos");
            }
        }
        public void BorrarUsuarios(Admin user)
        {
            bool t = user.UserIsAdmin(user);
            if (t == true)
            {
                Console.WriteLine("Selecciona el ID del usuario a eliminar");
                int selec = Convert.ToInt32(Console.ReadLine());
                connection.Open();
                string query = $"Delete from Usuarios where UsuarioID={selec}";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                Console.WriteLine("Sin permisos");
            }
        }
        //Peliculas
        public void ShowAdminPelicula()
        {
            connection.Open();
            string query00 = $"select PeliculaID,Titulo,Sinopsis,EdadRecomendada,Estado from Peliculas";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader["PeliculaID"].ToString()}-->{reader["Titulo"].ToString()} -PG- {reader["EdadRecomendada"].ToString()}, {reader["Estado"].ToString()}");
            }
            connection.Close();
        }
        public void GenerarPelicula(Admin user)
        {
            bool t = user.UserIsAdmin(user);
            if (t == true)
            {
                Console.WriteLine("Introduce Titulo, Sinopsis");
                string titulo = Console.ReadLine();
                string sinopsis = Console.ReadLine();
                Console.WriteLine("La edad recomendad");
                int edad = Convert.ToInt32(Console.ReadLine());
                string estado = "Disponible";
                connection.Open();
                string query = $"Insert Into Peliculas(Titulo,Sinopsis,EdadRecomendada,Estado) values('{titulo}','{sinopsis}','{edad}','{estado}')";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                Console.WriteLine("Sin permisos");
            }
        }
        public void BorrarPelicula(Admin user)
        {
            bool t = user.UserIsAdmin(user);
            if (t == true)
            {
                Console.WriteLine("Selecciona el ID de la pelicula a eliminar");
                int selec = Convert.ToInt32(Console.ReadLine());
                connection.Open();
                string query = $"Delete from Peliculas where PeliculasID={selec}";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                Console.WriteLine("Sin permisos");
            }
        }
        //Alquileres 
        public void HistorialMorosos()
        {
            //Dar aviso en caso de que se incumple el limite de tres dias de alquiler
            DateTime fecha1 = DateTime.Now;
            DateTime fecha2 = DateTime.Now;
            List<int> alquilerDesfasado = new List<int>();
            connection.Open();
            string querycond = $"Select*from Alquileres where FechaAlquiler is not null and FechaDevolucion is not null";
            SqlCommand commandcon = new SqlCommand(querycond, connection);
            SqlDataReader reader = commandcon.ExecuteReader();
            //Leo todo el historial de alquileres
            int id;
            while (reader.Read())
            {
                fecha1 = Convert.ToDateTime(reader["FechaAlquiler"]);
                fecha2 = Convert.ToDateTime(reader["FechaDevolucion"]);
                TimeSpan Ts = fecha2 - fecha1;
                int difference = Ts.Days;
                if (difference > 3)
                {
                    //Guardo los id de los alquileres desfasados
                    id = Convert.ToInt32(reader["AlquilerID"]);
                    alquilerDesfasado.Add(id);
                }
            }
            connection.Close();
            foreach (int index in alquilerDesfasado)
            {
                connection.Open();
                string querycond01 = $"Select Usuarios.Email,Alquileres.AlquilerID,Alquileres.FechaAlquiler,Alquileres.FechaDevolucion from Usuarios full outer join Alquileres on Usuarios.UsuarioID=Alquileres.UsuarioID where AlquilerID IS NOT NULL order by Usuarios.UsuarioID";
                SqlCommand commandcon01 = new SqlCommand(querycond01, connection);
                SqlDataReader reader01 = commandcon01.ExecuteReader();
                while (reader01.Read())
                {
                    if (Convert.ToInt32(reader01["AlquilerID"]) == index)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"El usuario {reader01["Email"].ToString()}, en el alquiler {reader01["AlquilerID"].ToString()} fechas {reader01["FechaAlquiler"].ToString()} {reader01["FechaDevolucion"].ToString()} tiene multa");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                connection.Close();
            }
        }

    }
}
