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
    public class PermisoDocumentoController : ControllerBase
    {
        private readonly BD context;
        private readonly string _connectionString;

        public PermisoDocumentoController(BD context, IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

     
        [HttpGet("{cod_sociedad}")]
        public ActionResult Get(string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            var sql = $"SELECT COD_PERMISO_DOCUMENTO , COD_SOCIEDAD , TIPO_DOC , PD_DESCRIPCION  FROM PERMISO_DOCUMENTO WHERE  cod_sociedad = '{cod_sociedad}' ";

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
                        COD = reader[0].ToString(),
                        cod_sociedad = reader[1].ToString(),
                        TIPO_DOC = reader[2].ToString(),
                        PD_DESCRIPCION = reader[3].ToString(),
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
        public ActionResult Post([FromBody] Permiso_Documento PD)
        {
            {
                try
                {
                    context.Permiso_Documento.Add(PD);
                    context.SaveChanges();
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }
        }

        [HttpDelete("{cod_sociedad}/{tipo}/{DESC}")]
        public ActionResult Delete(string cod_sociedad , int tipo ,string DESC)
        {
            var data = context.Permiso_Documento.FirstOrDefault(p => p.COD_SOCIEDAD == cod_sociedad && p.TIPO_DOC == tipo && p.PD_DESCRIPCION == DESC  );
            if (data != null)
            {
                context.Permiso_Documento.Remove(data);
                context.SaveChanges();
                return Ok("Eliminado");
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
