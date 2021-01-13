using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using B1E.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace B1EXPENSES.Entitites
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionCCController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public AsignacionCCController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        [HttpGet("{COD_SOC}/{COD_USUARIO}")]
        public ActionResult CambiarEstado(string COD_SOC, int COD_USUARIO)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            var sql = $"SELECT CC.COD_ASIGNACION_CC ,  US.USU_NOMBRE +' '+ US.USU_APELLIDO AS 'NOMBRES' , CC.COD_CENTRO_DE_COSTO , CC2.CDC_NOMBRE " +
                      "FROM ASIGNACION_CC AS CC " +
                      "LEFT JOIN USUARIO US " +
                      "ON CC.COD_USUARIO = US.COD_USUARIO " +
                      "JOIN CENTRO_DE_COSTO AS CC2 " +
                      $"ON CC2.COD_CENTRO_DE_COSTO = CC.COD_CENTRO_DE_COSTO WHERE US.COD_USUARIO = {COD_USUARIO} AND CC2.COD_SOCIEDAD = '{COD_SOC}'; ";

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
                        cod_asignacion_cc = reader[0].ToString(),
                        nombre = reader[1].ToString(),
                        cod_cc = reader[2].ToString(),
                        cc_nombre = reader[3].ToString()

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
        [HttpPost]
        public ActionResult Post([FromBody] ASIGNACION_CC cc)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"INSERT INTO ASIGNACION_CC VALUES ({cc.ACC_TIPO_DOCUMENTO},'{cc.COD_SOCIEDAD}',{cc.COD_USUARIO},'{cc.COD_CENTRO_DE_COSTO}')";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Asignacion creada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpDelete("{COD_ASIGNACION_CC}")]
        public ActionResult Post(int COD_ASIGNACION_CC)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"DELETE ASIGNACION_CC WHERE COD_ASIGNACION_CC = {COD_ASIGNACION_CC}";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Asignacion eliminada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{COD_CC}/{COD_ACC}")]
        public ActionResult Actualizar(string COD_CC , int COD_ACC )
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"UPDATE ASIGNACION_CC SET COD_CENTRO_DE_COSTO = '{COD_CC}' WHERE COD_ASIGNACION_CC = {COD_ACC}";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Asignacion Actualizada");

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
