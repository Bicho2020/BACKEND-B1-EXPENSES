using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Dynamic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaMasivaArticulosController : ControllerBase
    {

        private readonly string _connectionString;

        public CargaMasivaArticulosController(IConfiguration configuration)
        {
       
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        [HttpPost]
        public ActionResult Post(Object obj)
        {

            var insert = "INSERT INTO ARTICULO (COD_ARTICULO,COD_SOCIEDAD,COD_CUENTA,ART_NOMBRE,ART_ESTADO) VALUES ";
            var data = "";
            var n = 0;
            var ART = JsonConvert.DeserializeObject<List<ExpandoObject>>(obj.ToString());

            foreach (dynamic x in ART)
            {
                n = n + 1;

                data += $"('{x.COD_ARTICULO}{n}','{x.COD_SOCIEDAD}','{x.COD_CUENTA}','{x.ART_NOMBRE}',1),";

            }

            data = data.TrimEnd(',');

            var query = insert + data;

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Carga Masiva Exitosa");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
