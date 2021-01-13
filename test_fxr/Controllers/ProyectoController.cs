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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectoController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public ProyectoController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }
        [HttpGet("{sociedad}")]
        public ActionResult CambiarEstado( string sociedad)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT COD_PROYECTO , COD_SOCIEDAD , PROY_DESCRIPCION , CONVERT(varchar,PROY_FECHA_INICIO,23) ,  CONVERT(varchar,PROY_FECHA_FIN,23) , PROY_ESTADO FROM PROYECTO WHERE  cod_sociedad = '{sociedad}'", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_proyecto = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        proy_descripcion = reader[2].ToString(),
                        proy_fecha_inicio = reader[3].ToString(),
                        proy_fecha_fin = reader[4].ToString(),
                        proy_estado = reader[5].ToString(),

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
        public ActionResult Post([FromBody] Proyecto Proyecto)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO PROYECTO (COD_PROYECTO,COD_SOCIEDAD,PROY_DESCRIPCION,PROY_FECHA_INICIO,PROY_FECHA_FIN,PROY_ESTADO) VALUES ('{Proyecto.cod_proyecto}' ,'{Proyecto.cod_sociedad}','{Proyecto.proy_descripcion}','{Proyecto.proy_fecha_inicio}','{Proyecto.proy_fecha_fin}',1)", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("proyecto agregado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

       

        [HttpPut("{value}/{estado}/{cod}")]
        public ActionResult CambiarEstado(string value, int estado, string cod)
        {

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE PROYECTO SET {value} = {estado} WHERE COD_PROYECTO = '{cod}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("proyecto modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpPut]
        public ActionResult Modificar([FromBody] Proyecto Proyecto)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE PROYECTO SET PROY_DESCRIPCION = '{Proyecto.proy_descripcion}' , PROY_FECHA_INICIO = '{Proyecto.proy_fecha_inicio}' , PROY_FECHA_FIN = '{Proyecto.proy_fecha_fin}' WHERE COD_PROYECTO = '{Proyecto.cod_proyecto}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("proyecto modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
