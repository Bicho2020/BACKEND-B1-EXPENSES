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
    public class DocVentaController : ControllerBase
    {
        private readonly BD context;
        public DocVentaController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Doc_Venta> Get()
        {

            var data = context.Doc_Venta.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Doc_Venta Doc_Venta)
        {
            {
                try
                {
                    context.Doc_Venta.Add(Doc_Venta);
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
        public Doc_Venta Get(int id)
        {

            var art = context.Doc_Venta.FirstOrDefault(p => p.cod_doc_venta == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Doc_Venta Doc_Venta)
        {
            if (Doc_Venta.cod_doc_venta == id)
            {
                context.Entry(Doc_Venta).State = EntityState.Modified;
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
            var data = context.Doc_Venta.FirstOrDefault(p => p.cod_doc_venta == id);
            if (data != null)
            {
                context.Doc_Venta.Remove(data);
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
