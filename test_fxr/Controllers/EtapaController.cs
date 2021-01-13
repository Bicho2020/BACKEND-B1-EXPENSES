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
    public class EtapaController : ControllerBase
    {
        private readonly BD context;
        public EtapaController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Etapa> Get()
        {

            var data = context.Etapa.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Etapa Etapa)
        {
            {
                try
                {
                    context.Etapa.Add(Etapa);
                    context.SaveChanges();
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }

            }
        }
        [HttpGet("{id}")]
        public Etapa Get(int id)
        {

            var art = context.Etapa.FirstOrDefault(p => p.cod_etapa == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Etapa Etapa)
        {
            if (Etapa.cod_etapa == id)
            {
                context.Entry(Etapa).State = EntityState.Modified;
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
            var data = context.Etapa.FirstOrDefault(p => p.cod_etapa == id);
            if (data != null)
            {
                context.Etapa.Remove(data);
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
