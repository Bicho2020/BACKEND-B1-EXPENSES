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
    public class ArticuloController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public ArticuloController(BD context , IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        [HttpGet("{sociedad}")]
        public ActionResult CambiarEstado( string sociedad)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            var sql = $"SELECT * FROM ARTICULO WHERE  cod_sociedad = '{sociedad}'";
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
                        cod_articulo = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        cod_cuenta = reader[2].ToString(),
                        art_nombre = reader[3].ToString(),
                        art_estado = reader[4].ToString(),
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
        public ActionResult Post([FromBody] Articulo Articulo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"INSERT INTO ARTICULO(COD_ARTICULO, COD_SOCIEDAD, COD_CUENTA, ART_NOMBRE, ART_ESTADO) VALUES('{Articulo.cod_articulo}', '{Articulo.cod_sociedad}', '{Articulo.cod_cuenta}', '{Articulo.art_nombre}', 1)";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return Ok("Articulo agregado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        
         }

        [HttpPut("{value}/{estado}/{cod}")]
        public ActionResult CambiarEstado(string value, int estado,  string cod)
        {

            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"UPDATE ARTICULO SET {value} = {estado} WHERE COD_ARTICULO = '{cod}'";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Usuario modificado");
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
            var sql = $"UPDATE ARTICULO SET art_nombre = '{value}' WHERE COD_ARTICULO = '{cod}'";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Usuario modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var data = context.Articulo.FirstOrDefault(p => p.cod_articulo == id);
            if (data != null)
            {
                context.Articulo.Remove(data);
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
