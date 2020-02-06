using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Video_ClubDB_V2
{
    class Usuario
    {
        public SqlConnection connection = new SqlConnection("Data Source=LAPTOP-93TLR9QC\\SQLEXPRESS; Initial Catalog = Video-Club; Integrated Security = True");
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Usuario()
        {

        }
        public Usuario(string email,string contrasenia)
        {
            Email = email;
            Contrasenia = contrasenia;
        }
        public Usuario(string nombre, string apellido, string email, string contrasenia, DateTime fechaNacimiento)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Contrasenia = contrasenia;
            FechaNacimiento = fechaNacimiento;
        }
        public int GetYearsUser(string email)
        {
            int z = 0;
            DateTime nac;
            //Console.WriteLine(nac.Year)
            connection.Open();
            string query00 = $"select FechaNacimiento as Fechanacimiento from Usuarios where email='{email}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                nac = Convert.ToDateTime(reader["FechaNacimiento"]);
                string n1 = nac.ToString("yyyyMMdd");
                int today = Convert.ToInt32(DateTime.Today.ToString("yyyyMMdd"));
                z = (today - Convert.ToInt32(n1)) / 10000;
            }
            connection.Close();
            return z;
        }
        public bool UserIsLog(string email, string contrasenia)
        {
            bool t = false;
            connection.Open();
            string query00 = $"select*from Usuarios where Email='{email.ToLower()}' and Contrasenia='{contrasenia.ToLower()}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                t = true;
            }
            connection.Close();
            return t;
        }
        public void CreateLogUser()
        {
            Console.WriteLine("Introduce Nombre, apellido");
            string nombre = Console.ReadLine();
            string apellido = Console.ReadLine();
            Console.WriteLine("Introduce email y contraseña");
            string email = Console.ReadLine();
            string contrasenia = Console.ReadLine();
            DateTime born;
            try
            {

            Console.WriteLine("Introduce la fecha de nacimiento en yyyy/MM/dd");
            born = Convert.ToDateTime(Console.ReadLine());
            }
            catch (Exception)
            {
                //Introduzco el minimo de edad para realiza el alquilar
                born = new DateTime(2007,02,01);
            }
            connection.Open();
            string query = $"Insert Into Usuarios(Nombre,Apellido,Email,Contrasenia,FechaNacimiento) values('{nombre.ToLower()}','{apellido.ToLower()}','{email.ToLower()}','{contrasenia.ToLower()}','{born}')";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close();

        }
        public bool Log(string email, string contrasenia)
        {
            bool t = false;
            connection.Open();
            string query00 = $"select*from Usuarios where email='{email.ToLower()}' and contrasenia='{contrasenia.ToLower()}'";
            SqlCommand command00 = new SqlCommand(query00, connection);
            SqlDataReader reader = command00.ExecuteReader();
            if (reader.Read())
            {
                t = true;
            }
            connection.Close();
            return t;
        }
    }
}
