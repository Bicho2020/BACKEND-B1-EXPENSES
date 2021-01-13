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
    public class LicenciaController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public LicenciaController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet]
        public IEnumerable<Licencia> Get()
        {

            var data = context.Licencia.ToList();
            return data;

        }


        [HttpGet("query/{id}/{idE}")]
        public ActionResult GetQuery(int id , int idE)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT LIC.COD_LICENCIA , LIC.LIC_DESCRIPCION , LIC.LIC_ANIO_CADUCACION ,CASE WHEN ASIGL.COD_ASIGNACION_LICENCIA IS NOT NULL  THEN 'ASIGNADO' ELSE 'NO ASIGNADO' END 'ASIGNACION' FROM LICENCIA AS LIC LEFT JOIN ASIGNACION_LICENCIA AS ASIGL ON ASIGL.COD_LICENCIA = LIC.COD_LICENCIA AND ASIGL.COD_USUARIO = {id} WHERE LIC.COD_EMPRESA = {idE} ", connection);
                SqlDataReader reader = command.ExecuteReader();




                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_licencia = reader[0].ToString(),
                        lic_descripcion = reader[1].ToString(),
                        lic_anio_expiracion = reader[2].ToString(),
                        asignacion = reader[3].ToString()

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

        [HttpGet("usuario/{idE}")]
        public ActionResult GetUsuarioLicencia(int idE)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"SELECT US.COD_USUARIO , US.USU_RUT , US.USU_NOMBRE , US.USU_APELLIDO , "+
                        "CASE " +
                        "WHEN ASL.COD_LICENCIA = 1 THEN 'Con licencia' else 'Sin licencia' " +
                        "END 'LICENCIA' " +
                        "FROM USUARIO AS US " +
                        "FULL JOIN ASIGNACION_LICENCIA ASL " +
                        "ON ASL.COD_USUARIO = US.COD_USUARIO " +
                        $"WHERE US.COD_EMPRESA = {idE} AND USU_ESACTIVO = 1; ";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();




                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(), 
                        usu_rut = reader[1].ToString(),
                        usu_nombre = reader[2].ToString(), 
                        usu_apellido = reader[3].ToString(), 
                        asignacion = reader[4].ToString(),

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

    }
}
