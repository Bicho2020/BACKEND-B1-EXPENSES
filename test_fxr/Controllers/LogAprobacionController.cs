using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using B1E.Contexts;
using B1EXPENSES.Entitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAprobacionController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public LogAprobacionController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        [HttpPost]
        public ActionResult Post([FromBody] LOGS_FONDO_POR_RENDIR_APROBACION LFPD)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO LOGS_FONDO_POR_RENDIR_APROBACION " +
                       "(LFXR_HORA, LFXR_FECHA, LFXR_CODIGO_FONDO_POR_RENDIR_DETALLE, LFXR_ETAPA, LFXR_ESTADO, LFXR_LINEA, LFXR_COMENTARIO, LFXR_MONTO, LFXR_COMENTARIO_JEFE ,LFXR_JEFE ,LFXR_CODIGO_FONDO_POR_RENDIR )  " +
            $"VALUES('{LFPD.lfxr_hora}', '{LFPD.lfxr_fecha}', {LFPD.lfxr_codigo_fondo_por_rendir_detalle}, {LFPD.lfxr_etapa}, {LFPD.lfxr_estado}, {LFPD.lfxr_linea}, '{LFPD.lfxr_comentario}', {LFPD.lfxr_monto}, '{LFPD.lfxr_comentario_jefe}','{LFPD.lfxr_jefe}',{LFPD.LFXR_CODIGO_FONDO_POR_RENDIR}); ";
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

        [HttpGet("{COD_FXR}")]
        public ActionResult Post(int COD_FXR)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = "SELECT LFXR_CODIGO_FONDO_POR_RENDIR_DETALLE , LFXR_ETAPA , LFXR_JEFE , LFXR_COMENTARIO, SUBSTRING(CONVERT(varchar(10),LFXR_HORA),0,9) , CONVERT(VARCHAR(10),LFXR_FECHA,23) , LFXR_COMENTARIO_JEFE , LFXR_ESTADO " +
                       $"FROM LOGS_FONDO_POR_RENDIR_APROBACION WHERE LFXR_CODIGO_FONDO_POR_RENDIR = {COD_FXR} ORDER BY LFXR_ETAPA ASC; ";

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

    }
}
