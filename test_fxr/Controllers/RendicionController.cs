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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendicionController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public RendicionController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet("{COD_USUARIO}/{COD_SOCIEDAD}/{DATO}/{VALUE}")]
        public ActionResult Get(int COD_USUARIO, string COD_SOCIEDAD, string DATO, string VALUE)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT REN.COD_RENDICION , REN.COD_SOCIEDAD  ,  US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRE' , REN.REND_ESTADO , "+
                        "CONVERT(varchar,REN.REND_FECHA_CREACION,23) , REN.REND_APROBACION_JEFE , REN.REND_COMENTARIO , CONVERT(varchar,REN.REND_FECHA_REQUERIDA,23)," +
                        "PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE " +
                        "FROM RENDICION AS REN " +
                        "JOIN USUARIO AS US " +
                        "ON US.COD_USUARIO = REN.COD_USUARIO " +
                        "JOIN PROYECTO AS PRO " +
                        "ON PRO.COD_PROYECTO = REN.COD_PROYECTO " +
                        "JOIN CENTRO_DE_COSTO AS CC " +
                        $"ON CC.COD_CENTRO_DE_COSTO = REN.COD_CENTRO_DE_COSTO WHERE REN.COD_USUARIO = {COD_USUARIO} AND REN.COD_SOCIEDAD = '{COD_SOCIEDAD}' ORDER BY REN.{DATO} {VALUE} "; 
            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_RENDICION = reader[0].ToString(),
                        COD_SOCIEDAD = reader[1].ToString(),
                        NOMBRE = reader[2].ToString(),
                        ESTADO = reader[3].ToString(),
                        FECHA_CREACION = reader[4].ToString(),
                        APROBACION = reader[5].ToString(),
                        COMENTARIO = reader[6].ToString(),
                        FECHA_REQUERIDA = reader[7].ToString(),
                        PROYECTO = reader[8].ToString(),
                        CENTRO_DE_COSTO = reader[9].ToString(),

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

        [HttpGet("FILTRO/{COD_USUARIO}/{COD_SOCIEDAD}/{VALUE}")]
        public ActionResult GetFILTRO(int COD_USUARIO, string COD_SOCIEDAD, string VALUE)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "";

            if (VALUE == "x") // X ES CUANDO ESTA EN ALGun PROCESO
            {
                query = $"SELECT REN.COD_RENDICION , REN.COD_SOCIEDAD  ,  US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRE' , REN.REND_ESTADO , " +
                        $"CONVERT(varchar,REN.REND_FECHA_CREACION,23) , REN.REND_APROBACION_JEFE , REN.REND_COMENTARIO , CONVERT(varchar,REN.REND_FECHA_REQUERIDA,23), " +
                        $"PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE " +
                        $"FROM RENDICION AS REN " +
                        $"JOIN USUARIO AS US " +
                        $"ON US.COD_USUARIO = REN.COD_USUARIO " +
                        $"JOIN PROYECTO AS PRO " +
                        $"ON PRO.COD_PROYECTO = REN.COD_PROYECTO " +
                        $"JOIN CENTRO_DE_COSTO AS CC " +
                        $"ON CC.COD_CENTRO_DE_COSTO = REN.COD_CENTRO_DE_COSTO WHERE REN.COD_USUARIO = {COD_USUARIO} AND REN.COD_SOCIEDAD = '{COD_SOCIEDAD}' AND REN.REND_APROBACION_JEFE <> 99 AND REN.REND_APROBACION_JEFE <> 100 AND REN.REND_APROBACION_JEFE <> 101  ";
            }
            else
            {
                query = $"SELECT REN.COD_RENDICION , REN.COD_SOCIEDAD  ,  US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRE' , REN.REND_ESTADO , " +
                      $"CONVERT(varchar,REN.REND_FECHA_CREACION,23) , REN.REND_APROBACION_JEFE , REN.REND_COMENTARIO , CONVERT(varchar,REN.REND_FECHA_REQUERIDA,23), " +
                      $"PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE " +
                      $"FROM RENDICION AS REN " +
                      $"JOIN USUARIO AS US " +
                      $"ON US.COD_USUARIO = REN.COD_USUARIO " +
                      $"JOIN PROYECTO AS PRO " +
                      $"ON PRO.COD_PROYECTO = REN.COD_PROYECTO " +
                      $"JOIN CENTRO_DE_COSTO AS CC " +
                      $"ON CC.COD_CENTRO_DE_COSTO = REN.COD_CENTRO_DE_COSTO WHERE REN.COD_USUARIO = {COD_USUARIO} AND REN.COD_SOCIEDAD = '{COD_SOCIEDAD}' AND REN.REND_APROBACION_JEFE = {VALUE} ";
            }

            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_RENDICION = reader[0].ToString(),
                        COD_SOCIEDAD = reader[1].ToString(),
                        NOMBRE = reader[2].ToString(),
                        ESTADO = reader[3].ToString(),
                        FECHA_CREACION = reader[4].ToString(),
                        APROBACION = reader[5].ToString(),
                        COMENTARIO = reader[6].ToString(),
                        FECHA_REQUERIDA = reader[7].ToString(),
                        PROYECTO = reader[8].ToString(),
                        CENTRO_DE_COSTO = reader[9].ToString(),

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
        public ActionResult Post([FromBody] Rendicion Rendicion)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                context.Rendicion.Add(Rendicion);
                context.SaveChanges();
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT TOP 1 COD_RENDICION FROM RENDICION ORDER BY COD_RENDICION DESC", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_rendicion = reader[0].ToString()
                    });
                }
                connection.Close();

                return Ok(objs[0]);
            }
            catch (Exception EX)
            {
                return BadRequest(EX);
            }

        }


        [HttpGet("solicitudes/{cod_jefe}/{cod_sociedad}")]
        public ActionResult GetSolicitudes(int cod_jefe, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT FXR.COD_RENDICION , FXR.COD_SOCIEDAD , US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRE',  FXR.REND_APROBACION_JEFE ,  CONVERT(varchar,FXR.REND_FECHA_CREACION,23) , FXR.REND_COMENTARIO , " +
                        "CONVERT(varchar, FXR.REND_FECHA_REQUERIDA, 23)  " +
                        "FROM RENDICION AS FXR " +
                        "JOIN USUARIO AS US  " +
                        "ON US.COD_USUARIO = FXR.COD_USUARIO " +
                        $"WHERE FXR.COD_USUARIO IN(SELECT COD_USUARIO FROM ASIGNACION_SOCIEDAD WHERE COD_JEFE = {cod_jefe}) AND REND_APROBACION_JEFE = 0 AND COD_SOCIEDAD = '{cod_sociedad}'";

            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_doc = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        cod_usuario = reader[2].ToString(),
                        fxr_estado = reader[3].ToString(),
                        fxr_fecha_creacion = reader[4].ToString(),
                        fxr_comentario = reader[5].ToString(),
                        fxr_fecha_necesaria = reader[6].ToString(),

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

        [HttpPut("actualizar/{fecha}/{comentario}/{codigo}")]
        public ActionResult Actualizar(string fecha, string comentario, int codigo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"UPDATE RENDICION SET REND_FECHA_REQUERIDA = '{fecha}' ,  REND_COMENTARIO = '{comentario}' WHERE COD_RENDICION = ${codigo};";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 connection.Close();
                return Ok("Actualizado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{COD_RENDICION}")]
        public ActionResult Delete(int COD_RENDICION)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            var query = "BEGIN " +
                        $"DELETE FROM RENDICION WHERE COD_RENDICION = '{COD_RENDICION}' " +
                        $"DELETE FROM RENDICION_DETALLE WHERE COD_RENDICION = '{COD_RENDICION}'" +
                        "END; ";

            try
            {
           
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                
                connection.Close();

                return Ok("Eliminado");
            }
            catch (Exception EX)
            {
                return BadRequest(EX);
            }
        }
    }
}
