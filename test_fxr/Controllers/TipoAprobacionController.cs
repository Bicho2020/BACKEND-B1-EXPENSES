using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using B1E.Contexts;
using B1EXPENSES.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoAprobacionController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public TipoAprobacionController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        [HttpGet("{cod_sociedad}")]
        public ActionResult Get(string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            var sql = $"SELECT COD_TIPO_APROBACION , COD_SOCIEDAD , TA_TIPO_DOCUMENTO , TA_OPCION FROM TIPO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}'";

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_tipo_aprobacion = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        ta_tipo_documento = reader[2].ToString(),
                        ta_opcion = reader[3].ToString(),
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{cod_sociedad}/{tipo_doc}")]
        public ActionResult GetActual(string cod_sociedad , int tipo_doc)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            var sql = $"SELECT COD_TIPO_APROBACION , COD_SOCIEDAD , TA_TIPO_DOCUMENTO , TA_OPCION FROM TIPO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}' and TA_TIPO_DOCUMENTO = {tipo_doc}";

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_tipo_aprobacion = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        ta_tipo_documento = reader[2].ToString(),
                        ta_opcion = reader[3].ToString(),
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{cod_sociedad}/{doc}/{opcion}")]
        public ActionResult Post(string cod_sociedad , int doc , int opcion )
        {
            SqlConnection connection = new SqlConnection(_connectionString);
                    var sql = "DECLARE " +
                    "@datos int  " +
                    $"SET @datos = (SELECT COUNT(*) FROM TIPO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}' AND TA_TIPO_DOCUMENTO = {doc} AND TA_OPCION = {opcion}  ); " +
                    $"BEGIN  " +
                    $"DELETE FROM TIPO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}' AND TA_TIPO_DOCUMENTO = {doc} AND TA_OPCION<> {opcion}; " +
                    $"IF @datos = 0  " +
                    $"INSERT INTO TIPO_APROBACION VALUES('{cod_sociedad}',{doc},{opcion}); " +
                    "ELSE  " +
                    "PRINT 'NO HACE NADA'  " +
                    "END; ";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                connection.Close();
                return Ok("Guardado agregado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

     }
   

        [HttpDelete("{cod_sociedad}")]
        public ActionResult delete(string cod_sociedad)
        {

            var sql = $"DELETE FROM TIPO_APROBACION  WHERE COD_SOCIEDAD =  '{cod_sociedad}' ";

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                connection.Close();


                return Ok();
            }
          
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("{opcion}/{tipo_documento}/{cod_sociedad}")]
        public ActionResult Update( int opcion , int tipo_documento, string cod_sociedad)
        {

            var sql = $"UPDATE TIPO_APROBACION SET TA_OPCION = {opcion} WHERE TA_TIPO_DOCUMENTO = {tipo_documento} AND COD_SOCIEDAD = '{cod_sociedad}'";

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataReader reader = command.ExecuteReader();

                connection.Close();

                return Ok("Actualizado");
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
