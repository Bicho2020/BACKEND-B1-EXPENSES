using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SociedadController : ControllerBase
    {

        private readonly BD context;
        private readonly string _connectionString;
        public SociedadController(BD context, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
            this.context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var data = context.Sociedad.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }
        [HttpPost]
        public ActionResult Post([FromBody] Sociedad Sociedad)
        {
            {
                try
                {
                    context.Sociedad.Add(Sociedad);
                    context.SaveChanges();
                    return Ok("Sociedad Creada");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }
        }
        [HttpGet("{id}")]
        public Sociedad Get(string id)
        {

            var soc = context.Sociedad.FirstOrDefault(p => p.cod_sociedad == id);
            return soc;

        }

        [HttpGet("estado/{ide}")]
        public IEnumerable<Sociedad> Get(int ide)
        {

            var soc = context.Sociedad.FromSqlRaw($"SELECT * FROM SOCIEDAD WHERE COD_EMPRESA = {ide}").ToList();

            return soc;

        }

        [HttpGet("activo/{ide}")]
        public IEnumerable<Sociedad> GetActivo(int ide)
        {

            var soc = context.Sociedad.FromSqlRaw($"SELECT * FROM SOCIEDAD WHERE COD_EMPRESA = {ide} and soc_esactiva = 1").ToList();

            return soc;

        }


        [HttpPut("estado/{id}/{estado}")]
        public ActionResult UpdateEstado(string id, int estado)
        {
            var DataSociedad = new Sociedad()
            {
                cod_sociedad = id,
                soc_esactiva = estado
            };

            try
            {
                context.Sociedad.Attach(DataSociedad).Property(x => x.soc_esactiva).IsModified = true;
                context.SaveChanges();
                if (estado == 0)
                {
                    return Ok("Sociedad Desactivado");
                }
                else
                {
                    return Ok("Sociedad Activado");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put( string id , [FromBody] Sociedad Sociedad)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();

                SqlCommand command2 = new SqlCommand($"UPDATE SOCIEDAD SET COD_SOCIEDAD = '{Sociedad.cod_sociedad}' , SOC_NOMBRE = '{Sociedad.soc_nombre}', SOC_SERVIDOR = '{Sociedad.soc_servidor}' , SOC_ENLACE_SL = '{Sociedad.soc_enlace_sl}' , SOC_USUARIO = '{Sociedad.soc_servidor}' , SOC_CONTRASENIA = '{Sociedad.soc_contrasenia}' , SOC_VERSION_SQL = '{Sociedad.soc_version_sql}' , SOC_CONTRASENIA_BD = '{Sociedad.soc_contrasenia_bd}' , SOC_USUARIO_BD = '{Sociedad.soc_usuario_bd}' ,  SOC_BD = '{Sociedad.soc_bd}' WHERE COD_SOCIEDAD = '{id}';", connection);
                SqlDataReader reader2 = command2.ExecuteReader();

                connection.Close();

                return Ok("Sociedad Actualizada");

            }
            catch (Exception ex)
            {
                return BadRequest("Error al borrar asignacion" + ex);
            }
    ;
        

        }
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var data = context.Sociedad.FirstOrDefault(p => p.cod_sociedad == id);
            if (data != null)
            {
                context.Sociedad.Remove(data);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet("query/{id}/{idE}")]
        public ActionResult GetQuery(int id , int idE)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
           
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT COD_SOCIEDAD , SOC_NOMBRE , CASE WHEN COD_SOCIEDAD IN (SELECT COD_SOCIEDAD FROM ASIGNACION_SOCIEDAD WHERE COD_USUARIO = {id}) THEN 'ASIGNADO' ELSE 'NO ASIGNADO' END 'ASIGNACION' FROM SOCIEDAD  WHERE  SOC_ESACTIVA = 1 AND COD_EMPRESA = {idE}",connection);
                SqlDataReader reader = command.ExecuteReader();

               

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_sociedad = reader[0].ToString(),
                        soc_nombre = reader[1].ToString(),
                        asignacion_sociedad = reader[2].ToString(),
                  
                    });
                }
                
                connection.Close();
                return Ok(objs);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }

            
        }
    }
}
