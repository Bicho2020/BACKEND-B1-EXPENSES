using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FondoRendirDetalleController : ControllerBase
    {

        private readonly BD context;
        private readonly string _connectionString;
        public FondoRendirDetalleController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }
        [HttpGet]
        public IEnumerable<Fondo_Por_Rendir_Detalle> Get()
        {

            var data = context.Fondo_Por_Rendir_Detalle.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Fondo_Por_Rendir_Detalle Fondo_Por_Rendir_Detalle)
        {

            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);


            try
            {
                connection.Open();
                context.Fondo_Por_Rendir_Detalle.Add(Fondo_Por_Rendir_Detalle);
                    context.SaveChanges();

                    SqlCommand command = new SqlCommand($"SELECT TOP 1 COD_FONDO_POR_RENDIR FROM FONDO_POR_RENDIR ORDER BY COD_FONDO_POR_RENDIR DESC", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        objs.Add(new
                        {
                            cod_fondo_por_rendir = reader[0].ToString()
                        });
                    }

                connection.Close();

                return Ok();


                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

        }

        [HttpGet("{cod_detalle}")]
        public ActionResult Get(int cod_detalle)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"SELECT FRD.COD_FONDO_POR_RENDIR_DETALLE , FRD.FXRD_COMENTARIO , FRD.FXRD_MONTO, COD_PROYECTO , FRD.fxrd_linea , FRD.FXRD_COMENTARIO_JEFE , ART.ART_NOMBRE ,ART.COD_ARTICULO , FRD.COD_CENTRO_DE_COSTO , FRD.COD_PLAN_DE_CUENTA FROM FONDO_POR_RENDIR_DETALLE AS FRD JOIN ARTICULO AS ART ON ART.COD_ARTICULO = FRD.COD_ARTICULO WHERE  FRD.COD_FONDO_POR_RENDIR = {cod_detalle} ORDER BY FRD.COD_FONDO_POR_RENDIR ASC  ";

            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_fondo_por_rendir_detalle = reader[0].ToString(),
                        fxrd_comentario = reader[1].ToString(),
                        fxrd_monto = reader[2].ToString(),
                        cod_proyecto = reader[3].ToString(),
                        fxrd_estado = reader[4].ToString(),
                        fxrd_comentario_jefe = reader[5].ToString(),
                        art_producto = reader[6].ToString(),
                        cod_producto = reader[7].ToString(),
                        cod_cc = reader[8].ToString(),
                        cod_pdc = reader[9].ToString(),


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


        [HttpGet("{cod_detalle}/activos")]
        public ActionResult GetActivos(int cod_detalle)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT FRD.COD_FONDO_POR_RENDIR_DETALLE , "+
                        "FRD.FXRD_COMENTARIO , " +
                        "FRD.FXRD_MONTO, " +
                        "FRD.COD_PROYECTO ,  " +
                        "FRD.fxrd_linea ,  " +
                        "FRD.FXRD_COMENTARIO_JEFE ,  " +
                        "ART.ART_NOMBRE, " +
                        "ART.COD_ARTICULO , " +
                        "FRD.COD_CENTRO_DE_COSTO , " +
                        "FRD.COD_PLAN_DE_CUENTA , " +
                        "PR.PROY_DESCRIPCION, " +
                        "CC.CDC_NOMBRE " +
                        "FROM FONDO_POR_RENDIR_DETALLE AS FRD " +
                        "JOIN ARTICULO AS ART " +
                        "ON ART.COD_ARTICULO = FRD.COD_ARTICULO " +
                        "JOIN CENTRO_DE_COSTO AS CC " +
                        "ON CC.COD_CENTRO_DE_COSTO = FRD.COD_CENTRO_DE_COSTO " +
                        "JOIN PROYECTO AS PR " +
                        "ON PR.COD_PROYECTO = FRD.COD_PROYECTO " +
                        $"WHERE FRD.COD_FONDO_POR_RENDIR = {cod_detalle} AND FRD.FXRD_LINEA = 101 " +
                        "ORDER BY FRD.COD_FONDO_POR_RENDIR ASC "; 

            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_fondo_por_rendir_detalle = reader[0].ToString(),
                        fxrd_comentario = reader[1].ToString(),
                        fxrd_monto = reader[2].ToString(),
                        cod_proyecto = reader[3].ToString(),
                        fxrd_estado = reader[4].ToString(),
                        fxrd_comentario_jefe = reader[5].ToString(),
                        art_producto = reader[6].ToString(),
                        cod_producto = reader[7].ToString(),
                        cod_cc = reader[8].ToString(),
                        cod_pdc = reader[9].ToString(),
                        proyecto = reader[10].ToString(),
                        centro_costo = reader[11].ToString(),


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

        [HttpPut("{estado}/{id}/{comentario}")]
        public ActionResult Put( int estado ,int id , string comentario)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"UPDATE FONDO_POR_RENDIR_DETALLE SET FXRD_LINEA = {estado} , FXRD_COMENTARIO_JEFE = '{comentario}'  WHERE COD_FONDO_POR_RENDIR_DETALLE = {id}";

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 connection.Close();
                return Ok("Listo");
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("rendicion/{estado}/{id}/{comentario}")]
        public ActionResult PutRendicion(int estado, int id, string comentario)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"UPDATE RENDICION_DETALLE SET REND_LINEA = {estado} , REND_COMENTARIO_JEFE = '{comentario}'  WHERE COD_RENDICION_DETALLE = {id}";

            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                 connection.Close();
                return Ok("Listo");
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var data = context.Fondo_Por_Rendir_Detalle.FirstOrDefault(p => p.cod_fondo_por_rendir_detalle == id);
            if (data != null)
            {
                context.Fondo_Por_Rendir_Detalle.Remove(data);
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
