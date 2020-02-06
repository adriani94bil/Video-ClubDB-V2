using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Video_ClubDB_V2
{
    class Pelicula
    {
        public SqlConnection connection = new SqlConnection("Data Source=LAPTOP-93TLR9QC\\SQLEXPRESS; Initial Catalog = Video-Club; Integrated Security = True");
        public string Titulo { get; set; }
        public string Sinopsis { get; set; }
        public string Estado { get; set; }
        public int EdadRecomendada { get; set; }

        public Pelicula()
        {

        }
        public Pelicula(string titulo, string sinopsis, int edad, string estado)
        {
            Titulo = titulo;
            Sinopsis = sinopsis;
            Estado = estado;
            EdadRecomendada = edad;
        }
        //Conseguir ver las películas consultando a la BBDD usando un Select y 
        //guardarlas en una lista de objetos Película.
        public List<Pelicula> ShowPelicula()
        {
            List<Pelicula> filmo = new List<Pelicula>();
            connection.Open();
            string query00 = $"select Titulo,Sinopsis,EdadRecomendada,Estado from Peliculas";
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
    }
}
