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
    public class SincronizacionController : ControllerBase
    {
        private readonly BD context;
        public SincronizacionController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Sincronizacion> Get()
        {

            var data = context.Sincronizacion.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Sincronizacion Sincronizacion)
        {
            {
                try
                {
                    context.Sincronizacion.Add(Sincronizacion);
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
        public Sincronizacion Get(int id)
        {

            var art = context.Sincronizacion.FirstOrDefault(p => p.cod_sincronizacion == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Sincronizacion Sincronizacion)
        {
            if (Sincronizacion.cod_sincronizacion == id)
            {
                context.Entry(Sincronizacion).State = EntityState.Modified;
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
            var data = context.Sincronizacion.FirstOrDefault(p => p.cod_sincronizacion == id);
            if (data != null)
            {
                context.Sincronizacion.Remove(data);
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
