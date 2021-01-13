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
    public class ProcesoAprobacionController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public ProcesoAprobacionController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }




        [HttpGet("{cod_sociedad}/{cod_usuario}")]
        public IActionResult GetID(string cod_sociedad, int cod_usuario)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT ET.COD_ETAPA , ET.ETA_NUMERO , ET.ETA_NOMBRE FROM ETAPA AS ET  " +
                         "JOIN PROCESO_APROBACION AS PA " +
                         "ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION " +
                         $"WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.COD_USUARIO = {cod_usuario}";
            try
            {


                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_etapa = reader[0].ToString(),
                        eta_numero = reader[1].ToString(),
                        eta_nombre = reader[2].ToString(),

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
        public ActionResult Post([FromBody] Proceso_aprobacion PA)
        {


            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);


            try
            {
                context.Proceso_aprobacion.Add(PA);
                context.SaveChanges();


                connection.Open();

                SqlCommand command = new SqlCommand($"SELECT TOP 1 COD_PROCESO_APROBACION FROM PROCESO_APROBACION ORDER BY COD_PROCESO_APROBACION DESC", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_proceso_aprobacion = reader[0].ToString()
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


        [HttpPost("aceptar/{cod_sociedad}/{cod_fondo_rendir}/{num}")]
        public ActionResult Post(string cod_sociedad, int cod_fondo_rendir, int num)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            var query = "DECLARE " +
                      "  @datos int " +
                      "  SET @datos = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                      "       JOIN ETAPA AS ET " +
                      $"        ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = {num} ); " +
                      "  BEGIN " +
                      $"      UPDATE FONDO_POR_RENDIR_DETALLE SET FXRD_LINEA = 0 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir}; " +
                      "      IF @datos > 0 " +
                      $"       UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = FXR_APROBACION_JEFE + 1 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir}; " +
                      "       ELSE " +
                      $"       UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = 100 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir} " +
                      "       END";

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

        [HttpPost("aceptar/rendicion/{cod_sociedad}/{COD_RENDICION}/{num}/")]
        public ActionResult PostRendicion(string cod_sociedad, int COD_RENDICION, int num)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            var query = "DECLARE " +
                        "@datos int " +
                        "SET @datos = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                        "JOIN ETAPA AS ET " +
                        $"ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = 2 ); " +
                        "BEGIN " +
                        $"UPDATE RENDICION_DETALLE SET REND_LINEA = 0 WHERE COD_RENDICION = {COD_RENDICION}; " +
                        "IF @datos > 0  " +
                        $"UPDATE RENDICION SET REND_APROBACION_JEFE = REND_APROBACION_JEFE + 1 WHERE COD_RENDICION = {COD_RENDICION} ; " +
                        "ELSE " +
                        $" UPDATE RENDICION SET REND_APROBACION_JEFE = 100 WHERE COD_RENDICION = {COD_RENDICION}; " +
                        "END";

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

        [HttpPost("aceptar/{cod_sociedad}/{cod_fondo_rendir}/{num}/1")]
        public ActionResult Post2(string cod_sociedad, int cod_fondo_rendir, int num)
        {

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "";

            if(num == 1)
            {
                 query = "DECLARE " +
                    "  @datos int " +
                    "  SET @datos = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                    "       JOIN ETAPA AS ET " +
                    $"        ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = 1 ); " +
                    "  BEGIN " +
                    "      IF @datos > 0 " +
                    $"       UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = FXR_APROBACION_JEFE + 1 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir}; " +
                    "       ELSE " +
                    $"       UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE = 100 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir} " +
                    "       END";
            }



            if (num == 2)
            {
                 query = "DECLARE " +
                    "  @datos int " +
                    "  SET @datos = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                    "       JOIN ETAPA AS ET " +
                    $"        ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = 2 ); " +
                    "  BEGIN " +
                    "      IF @datos > 0 " +
                    $"      UPDATE RENDICION SET REND_APROBACION_JEFE = REND_APROBACION_JEFE + 1 WHERE COD_RENDICION =  {cod_fondo_rendir}; " +
                    "       ELSE " +
                    $"     UPDATE RENDICION SET REND_APROBACION_JEFE = 100 WHERE COD_RENDICION  = {cod_fondo_rendir} " +
                    "       END";
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

        [HttpPost("aceptar/autoFXR/{cod_sociedad}/{cod_fondo_rendir}/{etapa}/{doc}")]
        public ActionResult PostAutorizador(string cod_sociedad, int cod_fondo_rendir, int etapa , int doc)
        {
            var lasEtapa = 0;

            if (etapa == 50)
            {
                 lasEtapa = 1;
            }
            else
            {
                 lasEtapa = etapa;
            }
    
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = "DECLARE " +
                        "@COUNT int " +
                        "SET @COUNT = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                        "    JOIN ETAPA AS ET " +
                        $"    ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = {doc}); " +
                        "BEGIN " +
                        $"    IF @COUNT <= {lasEtapa}" +
                        $"	  UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE =  100 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir}; " +
                        "	ELSE " +
                        $"	 UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE =  FXR_APROBACION_JEFE + 1 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir}; " +
                        "END "; 
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

        [HttpPost("aceptar/autoRendicion/{cod_sociedad}/{cod_fondo_rendir}/{etapa}/{doc}")]
        public ActionResult PostAutoRendicion(string cod_sociedad, int cod_fondo_rendir, int etapa, int doc)
        {
            var lasEtapa = 0;

            if (etapa == 50)
            {
                lasEtapa = 1;
            }
            else
            {
                lasEtapa = etapa;
            }

            SqlConnection connection = new SqlConnection(_connectionString);
            var query = "DECLARE " +
                        "@COUNT int " +
                        "SET @COUNT = (SELECT COUNT(*) from PROCESO_APROBACION AS PA " +
                        "    JOIN ETAPA AS ET  " +
                        $"    ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION WHERE COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = {doc});  " +
                        "BEGIN  " +
                        $"    IF @COUNT <= {lasEtapa} " +
                        $"	UPDATE RENDICION SET REND_APROBACION_JEFE =  100 WHERE COD_RENDICION = {cod_fondo_rendir}; " +
                        "	ELSE " +
                        $"	 UPDATE RENDICION SET REND_APROBACION_JEFE =  REND_APROBACION_JEFE + 1 WHERE COD_RENDICION = {cod_fondo_rendir};  " +
                        "END";

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

        [HttpPost("rechazar/{cod_fondo_rendir}")]
        public ActionResult PostR(int cod_fondo_rendir)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"UPDATE FONDO_POR_RENDIR SET FXR_APROBACION_JEFE =  99 WHERE COD_FONDO_POR_RENDIR = {cod_fondo_rendir};";

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
        

        [HttpPost("rechazar/rendicion/{cod_fondo_rendir}")]
        public ActionResult PostRendicion(int cod_fondo_rendir)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"UPDATE RENDICION SET REND_APROBACION_JEFE =  99 WHERE COD_RENDICION = {cod_fondo_rendir};";

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



        [HttpGet("etapa/{id}/{doc}")]
        public IActionResult Get(string id , int doc)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

           
                try
                {
                   

                    connection.Open();

                    SqlCommand command = new SqlCommand($"SELECT ET.COD_ETAPA ,  ET.ETA_NUMERO , ET.ETA_NOMBRE  , US.USU_NOMBRE , US.USU_APELLIDO  FROM ETAPA AS ET JOIN PROCESO_APROBACION AS PA ON ET.COD_PROCESO_APROBACION = PA.COD_PROCESO_APROBACION JOIN USUARIO US  ON US.COD_USUARIO = ET.COD_USUARIO WHERE PA.COD_SOCIEDAD = '{id}' AND ET.ETA_ISVALIDO = {doc} order by ET.ETA_NUMERO ASC ", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        objs.Add(new
                        {
                            cod = reader[0].ToString(),
                            etapa_numero = reader[1].ToString(),
                            etapa_nombre = reader[2].ToString(),
                            usu_nombre = reader[3].ToString(),
                            usu_apellido = reader[4].ToString(),
                            
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


        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Proceso_aprobacion Proceso_aprobacion)
        {
            if (Proceso_aprobacion.cod_proceso_aprobacion == id)
            {
                context.Entry(Proceso_aprobacion).State = EntityState.Modified;
                context.SaveChanges();
                
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}/{cod}")]
        public ActionResult Delete(string id , int cod)
        {

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
           
                connection.Open();
                SqlCommand command = new SqlCommand($"DELETE ET FROM ETAPA  ET INNER JOIN PROCESO_APROBACION AP ON ET.COD_PROCESO_APROBACION = AP.COD_PROCESO_APROBACION WHERE AP.COD_SOCIEDAD = '{id}' AND ET.ETA_ISVALIDO = {cod} ", connection);
                command.ExecuteReader();
                connection.Close();

                try
                {
                    connection.Open();

                    SqlCommand command2 = new SqlCommand($"DELETE FROM PROCESO_APROBACION WHERE COD_SOCIEDAD = '{id}'", connection);
                    command2.ExecuteReader();
                    connection.Close();
                }
                catch (Exception ex)
                {

                    return BadRequest("Error con eliminar proceso" + ex);
                }

                return Ok("Listo");
            }

            catch (Exception ex)
            {
                return BadRequest("Error con eliminar etapa" + ex);
            }


        }

        [HttpGet("solicitudes/{cod_jefe}/{cod_sociedad}/{cod}")]
        public IActionResult SolicitudAproacion( int cod_jefe ,string cod_sociedad,int cod)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT " +
                        "FPR.COD_FONDO_POR_RENDIR , " +
                        "US.USU_NOMBRE + ' ' + US.USU_APELLIDO AS 'NOMBRES' , " +
                        "CONVERT(varchar,FPR.FXR_FECHA_CREACION,23) AS 'FECHA CREACION' ,  " +
                        "CONVERT(varchar,FPR.FXR_FECHA_NECESARIO,23) AS 'FECHA REQUERIDA', " +
                        "FPR.FXR_APROBACION_JEFE ,  " +
                        "FPR.FXR_COMENTARIOS  " +
                        "FROM FONDO_POR_RENDIR AS FPR  " +
                        "JOIN USUARIO AS US " +
                        "ON US.COD_USUARIO = FPR.COD_USUARIO " +
                        "WHERE FPR.FXR_APROBACION_JEFE IN " +
                        "(SELECT ET.ETA_NUMERO FROM ETAPA AS ET JOIN PROCESO_APROBACION AS PA " +
                        "ON PA.COD_PROCESO_APROBACION = ET.COD_PROCESO_APROBACION " +
                        $"WHERE ET.COD_USUARIO = {cod_jefe} AND PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = {cod} )";

            try
            { 
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_FONDO_POR_RENDIR = reader[0].ToString(),
                        NOMBRES = reader[1].ToString(),
                        FECHA_CREACION = reader[2].ToString(),
                        FECHA_REQUERIDA = reader[3].ToString(),
                        ETAPA = reader[4].ToString(),
                        COMENTARIO = reader[5].ToString(),

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


        [HttpGet("solicitudes/rendicion/{cod_jefe}/{cod_sociedad}/{cod}")]
        public IActionResult SolicitudRendicion(int cod_jefe, string cod_sociedad, int cod)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT R.COD_RENDICION , " +
                        "U.USU_NOMBRE + ' ' + U.USU_APELLIDO  as 'Nombre' , " +
                        "CONVERT(varchar,R.REND_FECHA_CREACION,23) , " +
                        "CONVERT(varchar,R.REND_FECHA_REQUERIDA,23) , " +
                        "R.REND_APROBACION_JEFE AS ETAPA , " +
                        "R.REND_COMENTARIO " +
                        "FROM RENDICION AS R " +
                        "JOIN USUARIO AS U " +
                        "ON U.COD_USUARIO = R.COD_USUARIO " +
                        "WHERE R.REND_APROBACION_JEFE IN  " +
                        "(SELECT ET.ETA_NUMERO FROM ETAPA AS ET JOIN PROCESO_APROBACION AS PA  " +
                        "ON PA.COD_PROCESO_APROBACION = ET.COD_PROCESO_APROBACION  " +
                        $"WHERE ET.COD_USUARIO = {cod_jefe} AND PA.COD_SOCIEDAD = '{cod_sociedad}' AND ET.ETA_ISVALIDO = {cod} )";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_FONDO_POR_RENDIR = reader[0].ToString(),
                        NOMBRES = reader[1].ToString(),
                        FECHA_CREACION = reader[2].ToString(),
                        FECHA_REQUERIDA = reader[3].ToString(),
                        ETAPA = reader[4].ToString(),
                        COMENTARIO = reader[5].ToString(),

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

        [HttpPut("editar/{cod_usuario}/{cod_etapa}")]
        public IActionResult ActualizarProcesoAprobacion(int cod_usuario , int cod_etapa)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"UPDATE ETAPA SET COD_USUARIO = {cod_usuario} WHERE COD_ETAPA = {cod_etapa}";

            try {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return Ok("Actualizado");
                
            }catch (Exception ex){
                return BadRequest(ex);
            }

        }

        [HttpGet("JefesActualUsuario/{COD_SOCIEDAD}/{COD_RENDICION}")]
        public IActionResult GetNombre(string COD_SOCIEDAD , int COD_RENDICION)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"EXECUTE JEFE_ACTUAL @COD_SOCIEDAD = {COD_SOCIEDAD} , @COD_RENDICION = {COD_RENDICION}";

            try {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_FONDO_POR_RENDIR = reader[0].ToString(),

                    });
                }
                connection.Close();
                return Ok(objs);
                
            }catch (Exception ex){
                return BadRequest(ex);
            }

        }

        [HttpGet("JefesActualUsuarioFondo/{COD_SOCIEDAD}/{COD_FONDO}")]
        public IActionResult GetNombreFondo(string COD_SOCIEDAD , int COD_FONDO)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"EXECUTE JEFE_ACTUAL_FONDO @COD_SOCIEDAD = '{COD_SOCIEDAD}' , @COD_FONDO = {COD_FONDO}";

            try {

                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_FONDO_POR_RENDIR = reader[0].ToString(),

                    });
                }
                connection.Close();
                return Ok(objs);
                
            }catch (Exception ex){
                return BadRequest(ex);
            }

        }


    }
}
