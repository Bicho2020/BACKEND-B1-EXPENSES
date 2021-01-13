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
    public class PlanCuentaController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public PlanCuentaController(BD context, IConfiguration configuration) 
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
                SqlCommand command = new SqlCommand($"SELECT * FROM PLAN_DE_CUENTA WHERE  cod_sociedad = '{sociedad}'", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_plan_de_cuenta = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        pdc_nombre = reader[2].ToString(),
                        pdc_estado = reader[3].ToString(),

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
        public ActionResult Post([FromBody] Plan_de_cuenta plan_De_Cuenta)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            string cod = plan_De_Cuenta.cod_plan_de_cuenta.ToString();

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO PLAN_DE_CUENTA (COD_PLAN_DE_CUENTA,COD_SOCIEDAD,PDC_NOMBRE,PDC_ESTADO) VALUES ( '{cod}', '{plan_De_Cuenta.cod_sociedad}', '{plan_De_Cuenta.pdc_nombre}' , 1)", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Plan de cuenta agregado");
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
                SqlCommand command = new SqlCommand($"UPDATE PLAN_DE_CUENTA SET {value} = {estado} WHERE COD_PLAN_DE_CUENTA = '{cod}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("plan de cuenta modificado");
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
                SqlCommand command = new SqlCommand($"UPDATE  PLAN_DE_CUENTA SET PDC_nombre = '{value}' WHERE COD_PLAN_DE_CUENTA = '{cod}'", connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("plan de cuenta modificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
