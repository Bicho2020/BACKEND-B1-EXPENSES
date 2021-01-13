using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using Newtonsoft.Json;
using System.Dynamic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendicionDetalleController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;
        public RendicionDetalleController(BD context, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
            this.context = context;
        }



        [HttpGet]
        public IEnumerable<Rendicion_detalle> Get()
        {

            var data = context.Rendicion_detalle.ToList();
            return data;

        }

        [HttpPost]
        public ActionResult Post(Object obj)
        {
            var insert = "INSERT INTO RENDICION_DETALLE (COD_RENDICION,REND_LINEA,REND_COMENTARIO,REND_TIPO_DOC,REND_MONTO,COD_ARTICULO,COD_PLAN_DE_CUENTA,COD_CENTRO_DE_COSTO,COD_PROYECTO,REND_NUMERO_DOC,COD_ADJUNTO) VALUES";
            var data = "";
            var n = 0;
            var ART = JsonConvert.DeserializeObject<List<ExpandoObject>>(obj.ToString());

            foreach (dynamic x in ART)
            {
                n = n + 1;

                if ( x.COD_CENTRO_DE_COSTO == "" && x.COD_PROYECTO == "")
                {
                    data += $"({x.COD_RENDICION},{x.REND_LINEA},'{x.REND_COMENTARIO}',{x.REND_TIPO_DOC},{x.REND_MONTO},'{x.COD_ARTICULO}','{x.COD_COD_PLAN_CUENTA}',null,null,'{x.REND_NUMERO_DOC}','{x.COD_ADJUNTO}'),";

                } else
                {
                    if (x.COD_CENTRO_DE_COSTO == "" && x.COD_PROYECTO != "")
                    {
                        data += $"({x.COD_RENDICION},{x.REND_LINEA},'{x.REND_COMENTARIO}',{x.REND_TIPO_DOC},{x.REND_MONTO},'{x.COD_ARTICULO}','{x.COD_COD_PLAN_CUENTA}',null,'{x.COD_PROYECTO}','{x.REND_NUMERO_DOC}','{x.COD_ADJUNTO}'),";

                    }
                    else
                    {
                        if (x.COD_CENTRO_DE_COSTO != "" && x.COD_PROYECTO == "")
                        {
                            data += $"({x.COD_RENDICION},{x.REND_LINEA},'{x.REND_COMENTARIO}',{x.REND_TIPO_DOC},{x.REND_MONTO},'{x.COD_ARTICULO}','{x.COD_COD_PLAN_CUENTA}','{x.COD_CENTRO_DE_COSTO}',null,'{x.REND_NUMERO_DOC}','{x.COD_ADJUNTO}'),";

                        }
                        else
                        {
                            data += $"({x.COD_RENDICION},{x.REND_LINEA},'{x.REND_COMENTARIO}',{x.REND_TIPO_DOC},{x.REND_MONTO},'{x.COD_ARTICULO}','{x.COD_COD_PLAN_CUENTA}','{x.COD_CENTRO_DE_COSTO}','{x.COD_PROYECTO}','{x.REND_NUMERO_DOC}','{x.COD_ADJUNTO}'),";
                        }
                    }
                }

            }

            data = data.TrimEnd(',');

            var query = insert + data;

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();


                connection.Close();
                return Ok("Carga Masiva Exitosa");
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

            var query = $"SELECT RD.COD_RENDICION_DETALLE , RD.REND_COMENTARIO , RD.REND_MONTO , RD.REND_LINEA , RD.REND_COMENTARIO_JEFE , ART.ART_NOMBRE , ART.COD_ARTICULO , RD.COD_CENTRO_DE_COSTO , PRO.PROY_DESCRIPCION , CC.CDC_NOMBRE , RD.COD_PLAN_DE_CUENTA , RD.COD_ADJUNTO " +
                        "FROM RENDICION_DETALLE AS RD JOIN PROYECTO AS PRO ON PRO.COD_PROYECTO = RD.COD_PROYECTO " +
                        "JOIN CENTRO_dE_COSTO AS CC " +
                        "ON CC.COD_CENTRO_DE_COSTO = RD.COD_CENTRO_DE_COSTO " +
                        "JOIN ARTICULO AS ART " +
                        "ON ART.COD_ARTICULO = RD.COD_ARTICULO " +
                        $"WHERE RD.COD_RENDICION = {cod_detalle} " +
                        "ORDER BY RD.COD_RENDICION asc";

            try
            {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_rendicion_detalle = reader[0].ToString(),
                        rd_comentario = reader[1].ToString(),
                        rd_monto = reader[2].ToString(),
                        rd_estado = reader[3].ToString(),
                        rd_comentario_jefe = reader[4].ToString(),
                        art_nombre = reader[5].ToString(),
                        cod_articulo = reader[6].ToString(),
                        cod_cc = reader[7].ToString(),
                        proyecto = reader[8].ToString(),
                        cc = reader[9].ToString(),
                        cod_pdc = reader[10].ToString(),
                        cod_adjunto = reader[11].ToString(),



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

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Rendicion_detalle Rendicion_detalle)
        {
            if (Rendicion_detalle.cod_rendicion_detalle == id)
            {
                context.Entry(Rendicion_detalle).State = EntityState.Modified;
                context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var data = context.Rendicion_detalle.FirstOrDefault(p => p.cod_rendicion_detalle == id);
            if (data != null)
            {
                context.Rendicion_detalle.Remove(data);
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
