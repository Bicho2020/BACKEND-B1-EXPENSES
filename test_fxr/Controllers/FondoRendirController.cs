using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FondoRendirController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public FondoRendirController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet]
        public IEnumerable<Fondo_por_Rendir> Get()
        {

            var data = context.Fondo_Por_Rendir.ToList();
            return data;

        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Fondo_por_Rendir Fondo_por_Rendir)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            {
                try
                {
                    context.Fondo_Por_Rendir.Add(Fondo_por_Rendir);
                    context.SaveChanges();

                    connection.Open();

                    SqlCommand command = new SqlCommand($"SELECT TOP 1 COD_FONDO_POR_RENDIR FROM FONDO_POR_RENDIR ORDER BY COD_FONDO_POR_RENDIR DESC", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        objs.Add(new
                        {
                            cod_fondo_por_rendir = reader[0].ToString()
                        });
                    }
                    connection.Close();
                    return Ok(objs[0]);
                }

                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }
        }



        [HttpGet("{cod_usuario}/{cod_sociedad}/{dato}/{value}")]
        public ActionResult Get(int cod_usuario, string cod_sociedad , string dato , string value)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT FXR.COD_FONDO_POR_RENDIR , FXR.COD_SOCIEDAD , US.USU_NOMBRE +' '+US.USU_APELLIDO AS 'NOMBRE' , FXR.FXR_ESTADO , CONVERT(varchar,FXR.FXR_FECHA_CREACION,23) , FXR.FXR_APROBACION_JEFE , FXR.FXR_COMENTARIOS , " +
                        "CONVERT(varchar,FXR.FXR_FECHA_NECESARIO,23) , PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE FROM FONDO_POR_RENDIR AS FXR JOIN USUARIO AS US " +
                        $"ON FXR.COD_USUARIO = US.COD_USUARIO LEFT JOIN PROYECTO AS PRO ON PRO.COD_PROYECTO = FXR.COD_PROYECTO LEFT JOIN CENTRO_dE_COSTO CC ON CC.COD_CENTRO_DE_COSTO = FXR.COD_CENTRO_DE_COSTO WHERE FXR.COD_USUARIO = {cod_usuario} and FXR.COD_SOCIEDAD = '{cod_sociedad}' ORDER BY FXR.{dato} {value}; ";
             
                try
                {
             
                    connection.Open();

                    SqlCommand command = new SqlCommand(query , connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        objs.Add(new
                        {
                            cod_fondo_por_rendir = reader[0].ToString(),
                            cod_sociedad = reader[1].ToString(),
                            cod_usuario = reader[2].ToString(),
                            fxr_estado = reader[3].ToString(),
                            fxr_fecha_creacion = reader[4].ToString(),
                            fxr_aprobacion_jefe = reader[5].ToString(),
                            fxr_comentarios = reader[6].ToString(),
                            FXR_FECHA_NECESARIO = reader[7].ToString(),
                            pro_nombre = reader[8].ToString(),
                            cc = reader[9].ToString(),


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

        [HttpGet("filtro/{cod_usuario}/{cod_sociedad}/{value}")]
        public ActionResult Filtro(int cod_usuario, string cod_sociedad,  string value)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "";

            if (value == "x") // X ES CUANDO ESTA EN ALGO PROCESO
            {
                 query = "SELECT FXR.COD_FONDO_POR_RENDIR , FXR.COD_SOCIEDAD , US.USU_NOMBRE +' '+US.USU_APELLIDO AS 'NOMBRE' , FXR.FXR_ESTADO , CONVERT(varchar,FXR.FXR_FECHA_CREACION,23) , FXR.FXR_APROBACION_JEFE , FXR.FXR_COMENTARIOS , " +
                       "CONVERT(varchar,FXR.FXR_FECHA_NECESARIO,23) , PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE FROM FONDO_POR_RENDIR AS FXR JOIN USUARIO AS US " +
                       $"ON FXR.COD_USUARIO = US.COD_USUARIO LEFT JOIN PROYECTO AS PRO ON PRO.COD_PROYECTO = FXR.COD_PROYECTO LEFT JOIN CENTRO_dE_COSTO CC ON CC.COD_CENTRO_DE_COSTO = FXR.COD_CENTRO_DE_COSTO WHERE FXR.COD_USUARIO = {cod_usuario} and FXR.COD_SOCIEDAD = '{cod_sociedad}' and FXR.FXR_APROBACION_JEFE <> 99 and FXR.FXR_APROBACION_JEFE <> 100 and FXR.FXR_APROBACION_JEFE <> 101 ";
            }
            else
            {
                query = "SELECT FXR.COD_FONDO_POR_RENDIR , FXR.COD_SOCIEDAD , US.USU_NOMBRE +' '+US.USU_APELLIDO AS 'NOMBRE' , FXR.FXR_ESTADO , CONVERT(varchar,FXR.FXR_FECHA_CREACION,23) , FXR.FXR_APROBACION_JEFE , FXR.FXR_COMENTARIOS , " +
                       "CONVERT(varchar,FXR.FXR_FECHA_NECESARIO,23) , PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE FROM FONDO_POR_RENDIR AS FXR JOIN USUARIO AS US " +
                       $"ON FXR.COD_USUARIO = US.COD_USUARIO LEFT JOIN PROYECTO AS PRO ON PRO.COD_PROYECTO = FXR.COD_PROYECTO LEFT JOIN CENTRO_dE_COSTO CC ON CC.COD_CENTRO_DE_COSTO = FXR.COD_CENTRO_DE_COSTO WHERE FXR.COD_USUARIO = {cod_usuario} and FXR.COD_SOCIEDAD = '{cod_sociedad}' and FXR.FXR_APROBACION_JEFE = {value} ";
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
                        cod_fondo_por_rendir = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        cod_usuario = reader[2].ToString(),
                        fxr_estado = reader[3].ToString(),
                        fxr_fecha_creacion = reader[4].ToString(),
                        fxr_aprobacion_jefe = reader[5].ToString(),
                        fxr_comentarios = reader[6].ToString(),
                        FXR_FECHA_NECESARIO = reader[7].ToString(),
                        pro_nombre = reader[8].ToString(),
                        cc = reader[9].ToString(),


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

        [HttpGet("solicitudes/{cod_jefe}/{cod_sociedad}")]
        public ActionResult GetSolicitudes(int cod_jefe, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT FXR.COD_FONDO_POR_RENDIR , FXR.COD_SOCIEDAD , US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRE', FXR.FXR_ESTADO , CONVERT(varchar,FXR.FXR_FECHA_CREACION,23) , FXR.FXR_COMENTARIOS , CONVERT(varchar,FXR.FXR_FECHA_NECESARIO,23) , FXR.FXR_APROBACION_JEFE " +
                        " FROM FONDO_POR_RENDIR AS FXR  " +
                        " JOIN USUARIO AS US  " +
                        " ON US.COD_USUARIO = FXR.COD_USUARIO  " +
                        $" WHERE FXR.COD_USUARIO IN(SELECT COD_USUARIO FROM ASIGNACION_SOCIEDAD WHERE COD_JEFE = {cod_jefe} ) AND FXR_APROBACION_JEFE = 0 AND COD_SOCIEDAD = '{cod_sociedad}'";

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


        [HttpPut("{cod_sociedad}/{cod_usuario}/{cod_fondo}/{doc}")]
        public ActionResult Put(string cod_sociedad , int cod_usuario , int cod_fondo , int doc)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "";

            if (doc == 1)
            {
                query = "DECLARE " +
               "@jefe  int, " +
               "@aprobacion  int  " +
               $"SET @aprobacion = (SELECT COUNT(*) FROM ETAPA AS ET JOIN PROCESO_APROBACION AS PA ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ETA_ISVALIDO = 1); " +
               $"SET @jefe = (SELECT COUNT(COD_JEFE) FROM ASIGNACION_SOCIEDAD WHERE COD_USUARIO = {cod_usuario} AND COD_SOCIEDAD = '{cod_sociedad}' AND COD_JEFE <> 0);  " +
               "BEGIN " +
               "IF @jefe > 0 " +
               $"UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = 0 WHERE COD_FONDO_POR_RENDIR = {cod_fondo}; " +
               "ELSE " +
               "IF @aprobacion > 0 " +
               $"UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = 1 WHERE COD_FONDO_POR_RENDIR = {cod_fondo}; " +
               "ELSE " +
               $"UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = 100 WHERE COD_FONDO_POR_RENDIR = {cod_fondo} ; " +
               "END";
            }
            else
            {
                query = "DECLARE " +
                "@jefe  int, " +
                "@aprobacion  int " +
                $"SET @aprobacion = (SELECT COUNT(*) FROM ETAPA AS ET JOIN PROCESO_APROBACION AS PA ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad  }' AND ETA_ISVALIDO = 2); " +
                $"SET @jefe = (SELECT COUNT(COD_JEFE) FROM ASIGNACION_SOCIEDAD WHERE COD_USUARIO = {cod_usuario} AND COD_SOCIEDAD = '{cod_sociedad}' AND COD_JEFE<> 0); " +
                "BEGIN " +
                "IF @jefe > 0 " +
                $"UPDATE RENDICION SET REND_APROBACION_JEFE = 0 WHERE COD_RENDICION = {cod_fondo} ; " +
                "ELSE " +
                "IF @aprobacion > 0 " +
                $"UPDATE RENDICION SET REND_APROBACION_JEFE = 1 WHERE COD_RENDICION = {cod_fondo}  " +
                "ELSE " +
                $"UPDATE RENDICION SET REND_APROBACION_JEFE = 100 WHERE COD_RENDICION = {cod_fondo} ; " +
                "END";
            }

    

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
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "BEGIN " +
                        $"DELETE FROM FONDO_POR_RENDIR_DETALLE WHERE   COD_FONDO_POR_RENDIR = {id}; " +
                        $"DELETE FROM FONDO_POR_RENDIR WHERE COD_FONDO_POR_RENDIR = {id}; " +
                        "END; ";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 connection.Close();
                return Ok("Eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("actualizar/{fecha}/{comentario}/{codigo}")]
        public ActionResult Actualizar(string fecha , string comentario , int codigo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"UPDATE FONDO_POR_RENDIR SET FXR_FECHA_NECESARIO = '{fecha}' ,  FXR_COMENTARIOS = '{comentario}' WHERE COD_FONDO_POR_RENDIR = ${codigo};";

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

    }
}
