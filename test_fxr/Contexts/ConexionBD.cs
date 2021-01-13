using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Contexts
{
    public class ConexionBD
    {
        public static SqlConnection GetConexion()
        {
            SqlConnection connection = new SqlConnection("Server=localhost;Database=B1S;User Id=vicho;Password=Qazxsw2134e.;");
            return connection;
        }
    }
}
