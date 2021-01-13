using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using B1E.Contexts;
using B1EXPENSES.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Logs_rendicion : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public Logs_rendicion(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        [HttpPost]
        public ActionResult Post([FromBody] LOGS_RENDICION_APROBACION LFPD)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO LOGS_RENDICION_APROBACION  "+
                        "(LRA_HORA, LRA_FECHA, LRA_CODIGO_RENDICION_DETALLE, LRA_ETAPA, LRA_ESTADO, LRA_LINEA, LRA_COMENTARIO, LRA_MONTO, LRA_COMENTARIO_JEFE, LRA_JEFE, LRA_CODIGO_RENDICION, LRA_CODIGO_FONDO_POR_RENDIR) " +
                        $"VALUES('{LFPD.LRA_HORA}', '{LFPD.LRA_FECHA}', {LFPD.LRA_CODIGO_RENDICION_DETALLE}, {LFPD.LRA_ETAPA}, {LFPD.LRA_ESTADO}, {LFPD.LRA_LINEA}, '{LFPD.LRA_COMENTARIO}', {LFPD.LRA_MONTO}, '{LFPD.LRA_COMENTARIO_JEFE}', '{LFPD.LRA_JEFE}', {LFPD.LRA_CODIGO_RENDICION}, {LFPD.LRA_CODIGO_FONDO_POR_RENDIR}); ";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();
                return Ok("Logs aprobacion FPR guardado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("{COD_RENDICION}")]
        public ActionResult get(int COD_RENDICION)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = "SELECT LRA_CODIGO_RENDICION ,LRA_ETAPA , LRA_JEFE ,LRA_COMENTARIO , SUBSTRING(CONVERT(varchar(10),LRA_HORA),0,9) , CONVERT(VARCHAR(10),LRA_FECHA,23) , LRA_COMENTARIO_JEFE, LRA_ESTADO "+
                      $"FROM LOGS_RENDICION_APROBACION WHERE LRA_CODIGO_RENDICION = {COD_RENDICION} ORDER BY LRA_ETAPA ASC";

          

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    objs.Add(new
                    {
                        COD_DETALLE = reader[0].ToString(),
                        ETAPA = reader[1].ToString(),
                        JEFE = reader[2].ToString(),
                        ARTICULO = reader[3].ToString(),
                        HORA = reader[4].ToString(),
                        FECHA = reader[5].ToString(),
                        COMENTARIO_JEFE = reader[6].ToString(),
                        ESTADO = reader[7].ToString(),
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

        [HttpGet("LG/{COD_USUARIO}")]
        public ActionResult Get2(int COD_USUARIO)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var sql = $"SELECT CONVERT(VARCHAR(10), FECHA_OPERACION, 23) , SUBSTRING(CONVERT(varchar(10), HORA_OPERACION), 0, 9) , OPERACION,COD_RENDICÍON FROM LOG_RENDICIÓN WHERE COD_USUARIO = {COD_USUARIO} ORDER BY COD_LOG_RENDICIÓN ASC";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    objs.Add(new
                    {
                        FECHA_OPERACION = reader[0].ToString(),
                        HORA_OPERACION = reader[1].ToString(),
                        OPERACION = reader[2].ToString(),
                        COD_RENDICÍON = reader[3].ToString(),

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

        [HttpPost("LG")]
        public ActionResult PostDos2(Object obj)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
            var ART = JsonConvert.DeserializeObject<List<ExpandoObject>>(obj.ToString());
            var sql = "";

            foreach (dynamic x in ART)
            {
                sql = "INSERT INTO LOG_RENDICIÓN (FECHA_OPERACION,HORA_OPERACION,OPERACION,COD_RENDICÍON,COD_USUARIO)" +
                       $"VALUES('{x.FECHA_OPERACION}', '{x.HORA_OPERACION}', '{x.OPERACION}', {x.COD_RENDICÍON}, {x.COD_USUARIO})";
            }

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                connection.Close();
                return Ok("Guardado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpGet("LG2/{COD_USUARIO}")]
        public ActionResult GetDos(int COD_USUARIO)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var sql = $"SELECT CONVERT(VARCHAR(10), FECHA_OPERACION, 23) , SUBSTRING(CONVERT(varchar(10), HORA_OPERACION), 0, 9) , OPERACION,COD_FONDO_X_RENDIR FROM LOGS_FONDO_x_RENDIR WHERE COD_USUARIO = {COD_USUARIO} ORDER BY COD_LOG_FONDO_POR_RENDIR  ASC";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    objs.Add(new
                    {
                        FECHA_OPERACION = reader[0].ToString(),
                        HORA_OPERACION = reader[1].ToString(),
                        OPERACION = reader[2].ToString(),
                        COD_FONDO_X_RENDIR = reader[3].ToString(),

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

        [HttpPost("LG2")]
        public ActionResult PostDos23(Object obj)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
            var ART = JsonConvert.DeserializeObject<List<ExpandoObject>>(obj.ToString());
            var sql = "";

            foreach (dynamic x in ART)
            {
                sql = "INSERT INTO LOGS_FONDO_x_RENDIR (FECHA_OPERACION,HORA_OPERACION,OPERACION,COD_FONDO_X_RENDIR,COD_USUARIO)" +
                       $"VALUES('{x.FECHA_OPERACION}', '{x.HORA_OPERACION}', '{x.OPERACION}', {x.COD_FONDO_X_RENDIR}, {x.COD_USUARIO})";
            }

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                connection.Close();
                return Ok("Guardado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
