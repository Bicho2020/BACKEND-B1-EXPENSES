using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocVentaDetalleController : ControllerBase
    {
        private readonly BD context;
        public DocVentaDetalleController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Doc_Venta_Detalle> Get()
        {

            var data = context.Doc_Venta_Detalle.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Doc_Venta_Detalle doc_Venta_Detalle)
        {
            {
                try
                {
                    context.Doc_Venta_Detalle.Add(doc_Venta_Detalle);
                    context.SaveChanges();
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }

            }
        }
        [HttpGet("{id}")]
        public Doc_Venta_Detalle Get(int id)
        {

            var art = context.Doc_Venta_Detalle.FirstOrDefault(p => p.cod_doc_venta_detalle == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Doc_Venta_Detalle doc_Venta_Detalle)
        {
            if (doc_Venta_Detalle.cod_doc_venta_detalle == id)
            {
                context.Entry(doc_Venta_Detalle).State = EntityState.Modified;
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
            var data = context.Doc_Venta_Detalle.FirstOrDefault(p => p.cod_doc_venta_detalle == id);
            if (data != null)
            {
                context.Doc_Venta_Detalle.Remove(data);
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
