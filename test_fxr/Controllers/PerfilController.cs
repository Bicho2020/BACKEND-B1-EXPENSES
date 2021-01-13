using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;



namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public PerfilController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet]
        public IEnumerable<Perfil> Get()
        {

            var data = context.Perfil.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Perfil Perfil)
        {
         
              try
              {
                    context.Perfil.Add(Perfil);
                    context.SaveChanges();
                    return Ok();
              }
               catch (Exception)
              {
                    return BadRequest();
              }

        
        }
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var query = $"SELECT PER.COD_PERFIL , SO.COD_SOCIEDAD , SO.SOC_NOMBRE , PER.PERF_NOMBRE , ASI.CODIGO_USUARIO_SAP , " +
                        "CASE " +
                        "WHEN COD_JEFE IS NOT NULL THEN(SELECT USU_NOMBRE + ' ' + USU_APELLIDO FROM USUARIO WHERE COD_USUARIO = ASI.COD_JEFE) ELSE '0' END 'JEFE' FROM ASIGNACION_SOCIEDAD ASI JOIN PERFIL PER ON ASI.COD_PERFIL = PER.COD_PERFIL " +
                        $"JOIN SOCIEDAD SO ON SO.COD_SOCIEDAD = ASI.COD_SOCIEDAD WHERE ASI.COD_USUARIO = {id} AND SO.SOC_ESACTIVA = 1";

            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_perfil = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        soc_nombre = reader[2].ToString(),
                        perf_nombre = reader[3].ToString(),
                        codigo_usuario_sap = reader[4].ToString(),
                        cod_jefe = reader[5].ToString(),
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al listar los perfiles" + ex);
            }


        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var data = context.Perfil.FirstOrDefault(p => p.cod_perfil == id);
            if (data != null)
            {
                context.Perfil.Remove(data);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("actual/{id}")]
        
        public ActionResult GetQuery(int id)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);
            

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT PERF_NONBRE FROM PERFIL WHERE COD_USUARIO = {id} ", connection);
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        PERF_NOMBRE = reader[0].ToString(),
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

        [HttpGet("query/{id}")]
        public ActionResult GetCountPerfil(int id)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT DISTINCT(SELECT COUNT(COD_PERFIL) FROM PERFIL WHERE PERF_NOMBRE = 'ADMINISTRADOR' AND COD_USUARIO IN ( SELECT COD_USUARIO FROM USUARIO WHERE COD_EMPRESA = {id} AND USU_ESACTIVO = 1 )) AS 'ADMINISTRADOR',(SELECT COUNT(COD_PERFIL) FROM PERFIL WHERE PERF_NOMBRE = 'USUARIO' AND COD_USUARIO IN ( SELECT COD_USUARIO FROM USUARIO WHERE COD_EMPRESA = {id} AND USU_ESACTIVO = 1 )) AS 'USUARIO',(SELECT COUNT(COD_PERFIL) FROM PERFIL WHERE PERF_NOMBRE = 'MASTER' AND COD_USUARIO IN ( SELECT COD_USUARIO FROM USUARIO WHERE COD_EMPRESA = {id} AND USU_ESACTIVO = 1 )) AS 'MASTER' ,(SELECT COUNT(COD_PERFIL) FROM PERFIL WHERE PERF_NOMBRE = 'AUTORIZADOR' AND COD_USUARIO IN ( SELECT COD_USUARIO FROM USUARIO WHERE COD_EMPRESA = {id} AND USU_ESACTIVO = 1 )) AS 'AUTORIZADOR' FROM PERFIL", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        rol_cantidad_administrador = reader[0].ToString(),
                        rol_cantidad_usuario = reader[1].ToString(),
                        rol_cantidad_master = reader[2].ToString(),
                        rol_cantidad_autorizador = reader[3].ToString()
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

        [HttpPut("{value}/{id}")]
        public ActionResult PutRol(string value,int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE PERFIL SET PERF_NOMBRE = '{value}' WHERE COD_PERFIL = {id} ", connection);
                SqlDataReader reader = command.ExecuteReader();

            
                connection.Close();
                return Ok("Usuario modificado a "+value+"");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("sap/{value}/{id}")]
        public ActionResult PutSAPUSER(string value, int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE ASIGNACION_SOCIEDAD SET CODIGO_USUARIO_SAP = '{value}' WHERE COD_PERFIL = {id} ", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Usuario modificado a " + value + "");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("jefe/{value}/{id}")]
        public ActionResult PutjEFE(string value, int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE ASIGNACION_SOCIEDAD SET COD_JEFE = {value} WHERE COD_PERFIL = {id} ", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Usuario modificado a " + value + "");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
