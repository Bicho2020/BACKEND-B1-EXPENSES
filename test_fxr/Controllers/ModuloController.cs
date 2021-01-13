using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using B1EXPENSES.Entitites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuloController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public ModuloController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet("admnistradores/{cod_usuario}/{cod_sociedad}")]
        public ActionResult Get(int cod_usuario,string cod_sociedad)
        {

            var sql = "SELECT MO.COD_MODULO , MO.MOD_NOMBRE , "+
                       " CASE " +
                     $"  WHEN MO.COD_MODULO IN(SELECT COD_MODULO FROM ASIGNACION_MODULO WHERE COD_SOCIEDAD = '{cod_sociedad}' AND COD_USUARIO = {cod_usuario})   THEN 'ASIGNADO' ELSE 'NO ASIGNADO' END 'ASIGNACION' " +
                     "   FROM MODULO  AS MO WHERE COD_PERFIL_MODULO = 1; ";

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
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
                        cod_modulo = reader[0].ToString(),
                        mod_nombre = reader[1].ToString(),
                        mod_asignacion = reader[2].ToString(),

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

        [HttpGet("usuarios/{cod_usuario}/{cod_sociedad}")]
        public ActionResult GetUsuarios(int cod_usuario, string cod_sociedad)
        {

            var sql = "SELECT MO.COD_MODULO , MO.MOD_NOMBRE , " +
                       " CASE " +
                     $"  WHEN MO.COD_MODULO IN(SELECT COD_MODULO FROM ASIGNACION_MODULO WHERE COD_SOCIEDAD = '{cod_sociedad}' AND COD_USUARIO = {cod_usuario})   THEN 'ASIGNADO' ELSE 'NO ASIGNADO' END 'ASIGNACION' " +
                     "   FROM MODULO  AS MO WHERE COD_PERFIL_MODULO = 3; ";

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
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
                        cod_modulo = reader[0].ToString(),
                        mod_nombre = reader[1].ToString(),
                        mod_asignacion = reader[2].ToString(),

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


        [HttpGet("autorizador/{cod_usuario}/{cod_sociedad}")]
        public ActionResult GetAutorizadoreS(int cod_usuario, string cod_sociedad)
        {

            var sql = "SELECT MO.COD_MODULO , MO.MOD_NOMBRE , " +
                       " CASE " +
                     $"  WHEN MO.COD_MODULO IN(SELECT COD_MODULO FROM ASIGNACION_MODULO WHERE COD_SOCIEDAD = '{cod_sociedad}' AND COD_USUARIO = {cod_usuario})   THEN 'ASIGNADO' ELSE 'NO ASIGNADO' END 'ASIGNACION' " +
                     "   FROM MODULO  AS MO WHERE COD_PERFIL_MODULO = 2; ";

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
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
                        cod_modulo = reader[0].ToString(),
                        mod_nombre = reader[1].ToString(),
                        mod_asignacion = reader[2].ToString(),

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
        public ActionResult Post([FromBody] Asignacion_Modulo ASM)
        {
     
        try
         {
           context.Asignacion_Modulo.Add(ASM);
           context.SaveChanges();
           return Ok("Modulo asignado");
         }
         catch (Exception ex)
         {
            return BadRequest(ex);
         }

         
        }

        [HttpDelete("{cod_modulo}/{cod_sociedad}/{cod_usuario}")]
        public ActionResult Delete(int cod_modulo , string cod_sociedad , int cod_usuario )
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"DELETE FROM ASIGNACION_MODULO WHERE COD_MODULO = {cod_modulo} AND COD_SOCIEDAD = '{cod_sociedad}' AND COD_USUARIO = {cod_usuario} ";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

             

                connection.Close();

                return Ok("Modulo Desactivado");
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

    }
}
