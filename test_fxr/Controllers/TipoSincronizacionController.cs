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
    public class TipoSincronizacionController : ControllerBase
    {
        private readonly BD context;
        public TipoSincronizacionController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Tipo_sincronizacion> Get()
        {

            var data = context.Tipo_sincronizacion.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Tipo_sincronizacion Tipo_sincronizacion)
        {
            {
                try
                {
                    context.Tipo_sincronizacion.Add(Tipo_sincronizacion);
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
        public Tipo_sincronizacion Get(int id)
        {

            var art = context.Tipo_sincronizacion.FirstOrDefault(p => p.cod_tipo_sincronizacion == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Tipo_sincronizacion Tipo_sincronizacion)
        {
            if (Tipo_sincronizacion.cod_tipo_sincronizacion== id)
            {
                context.Entry(Tipo_sincronizacion).State = EntityState.Modified;
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
            var data = context.Tipo_sincronizacion.FirstOrDefault(p => p.cod_tipo_sincronizacion == id);
            if (data != null)
            {
                context.Tipo_sincronizacion.Remove(data);
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
