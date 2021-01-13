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
    public class CentroCostoController : ControllerBase
    {

        private readonly BD context;
        private readonly string _connectionString;
        public CentroCostoController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet("{sociedad}")]
        public ActionResult CambiarEstado(string sociedad)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM CENTRO_DE_COSTO WHERE  cod_sociedad = '{sociedad}'", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_centro_de_costo = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        cdc_nombre = reader[2].ToString(),
                        cdc_estado = reader[3].ToString(),

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
        public ActionResult Post([FromBody] Centro_de_costo centro_De_Costo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO CENTRO_DE_COSTO (COD_CENTRO_DE_COSTO,COD_SOCIEDAD,CDC_NOMBRE,CDC_ESTADO) VALUES ( '{centro_De_Costo.cod_centro_de_costo}', '{centro_De_Costo.cod_sociedad}', '{centro_De_Costo.cdc_nombre}' , 1)", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Centro de costo agregado");
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
                SqlCommand command = new SqlCommand($"UPDATE CENTRO_DE_COSTO SET {value} = {estado} WHERE COD_CENTRO_DE_COSTO = '{cod}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Centro de costo modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpPut("{value}/{cod}")]
        public ActionResult CambiarNombre(string value, string cod)
        {

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"UPDATE CENTRO_DE_COSTO SET CDC_nombre = '{value}' WHERE COD_CENTRO_DE_COSTO = '{cod}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Centro de costo modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
