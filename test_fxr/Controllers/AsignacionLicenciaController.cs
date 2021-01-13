using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AsignacionLicenciaController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public AsignacionLicenciaController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        [HttpPost]
        public ActionResult Post([FromBody] Asignacion_Licencia AsigLicencia)
        {
            {
                try
                {
                    context.Asignacion_Licencia.Add(AsigLicencia);
                    context.SaveChanges();
                    return Ok("Licencia Creada");
                }
                catch (Exception)
                {
                    return BadRequest("Error al crear licencia");
                }

            }
        }

        [HttpGet("{id}")]
        public ActionResult get(int id)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT LIC.COD_LICENCIA , LIC.LIC_ANIO_CADUCACION , LIC.LIC_DESCRIPCION , CONCAT(COUNT(ASIGL.COD_LICENCIA),'/' , LIC.LIC_TOTAL ) FROM LICENCIA AS LIC LEFT JOIN ASIGNACION_LICENCIA AS ASIGL ON ASIGL.COD_LICENCIA = LIC.COD_LICENCIA WHERE COD_EMPRESA = {id} GROUP BY ASIGL.COD_LICENCIA, LIC.LIC_DESCRIPCION, LIC.LIC_TOTAL, LIC.COD_LICENCIA , LIC.LIC_ANIO_CADUCACION , LIC.LIC_DESCRIPCION  ", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_licencia = reader[0].ToString(),
                        lic_datos = reader[1].ToString(),
                        lic_descripcion = reader[2].ToString(),
                        lic_total = reader[3].ToString()


                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
                {
                    return BadRequest("Error al listar total licencias "+ ex);
                }

          
        }

        [HttpDelete("{codU}/{codL}")]
        public ActionResult Delete(int codU , int codL)
        {
            var data = context.Asignacion_Licencia.FirstOrDefault(p => p.cod_usuario == codU && p.cod_licencia == codL);
            if (data != null)
            {
                context.Asignacion_Licencia.Remove(data);
                context.SaveChanges();
                return Ok("Licencia eliminada");
            }
            else
            {
                return BadRequest("Error al eliminar ");
            }
        }
    }
}
