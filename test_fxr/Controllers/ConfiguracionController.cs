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
    public class ConfiguracionController : ControllerBase
    {
        private readonly BD context;
        public ConfiguracionController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Configuracion> Get()
        {

            var data = context.Configuracion.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Configuracion Configuracion)
        {
            {
                try
                {
                    context.Configuracion.Add(Configuracion);
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
        public Configuracion Get(int id)
        {

            var art = context.Configuracion.FirstOrDefault(p => p.cod_configuracion == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Configuracion Configuracion)
        {
            if (Configuracion.cod_configuracion == id)
            {
                context.Entry(Configuracion).State = EntityState.Modified;
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
            var data = context.Configuracion.FirstOrDefault(p => p.cod_configuracion == id);
            if (data != null)
            {
                context.Configuracion.Remove(data);
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
