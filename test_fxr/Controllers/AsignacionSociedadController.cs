using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using B1E.Contexts;
using B1E.Entitites;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsignacionSociedadController : ControllerBase
    {

        private readonly BD context;
        private readonly string _connectionString;
        public AsignacionSociedadController (BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpPost]
        public ActionResult Post([FromBody] Asignacion_Sociedad asignacion_Sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                var sql = $"INSERT INTO PERFIL values ({asignacion_Sociedad.cod_usuario},'NO ASIGNADO','GRUPO DEFAULT');";
                connection.Open();

                SqlCommand command2 = new SqlCommand(sql, connection);
                SqlDataReader reader2 = command2.ExecuteReader();

                connection.Close();
                try
                {
                    var sql2 = $"INSERT INTO ASIGNACION_SOCIEDAD VALUES ('{asignacion_Sociedad.cod_sociedad}',{asignacion_Sociedad.cod_usuario},(SELECT TOP 1 COD_PERFIL FROM PERFIL ORDER BY COD_PERFIL DESC) , '{asignacion_Sociedad.codigo_usuario_sap}' , 0 )";
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql2, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return Ok("ASIGNACION CREADA");
                }
                catch (Exception ex)
                {
                    return BadRequest("Error al guardar asignacion " + ex);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al guardar perfil " + ex);
            }

        }

   
        [HttpDelete("{COD_SOC}/{COD_USU}")]
        public ActionResult DELETE(string COD_SOC, int COD_USU)
        {

     
            var data = context.Asignacion_Sociedad.FirstOrDefault(p => p.cod_usuario == COD_USU && p.cod_sociedad == COD_SOC);
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                var sql = $"DELETE  ASIGNACION_SOCIEDAD WHERE COD_SOCIEDAD = '{COD_SOC}' AND COD_USUARIO = {COD_USU}";
                connection.Open();

                SqlCommand command2 = new SqlCommand(sql, connection);
                SqlDataReader reader2 = command2.ExecuteReader();

                connection.Close();
                try
                {
                    var sql2 = $"DELETE PERFIL WHERE COD_PERFIL = {data.cod_perfil} ";

                    connection.Open();
                    SqlCommand command = new SqlCommand(sql2, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();

                    return Ok("ASIGNACION BORRADA");
                }
                catch (Exception ex)
                {
                    return BadRequest("Error al perfil" + ex);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error al borrar asignacion" + ex);
            }
        }
    }
}
