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
    public class DecisionAprobacionController : ControllerBase
    {
        private readonly BD context;
        public DecisionAprobacionController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Decision_aprobacion> Get()
        {

            var data = context.decision_Aprobacion.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Decision_aprobacion Decision_aprobacion)
        {
            {
                try
                {
                    context.decision_Aprobacion.Add(Decision_aprobacion);
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
        public Decision_aprobacion Get(int id)
        {

            var art = context.decision_Aprobacion.FirstOrDefault(p => p.cod_decision_aprobacion == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Decision_aprobacion Decision_aprobacion)
        {
            if (Decision_aprobacion.cod_decision_aprobacion == id)
            {
                context.Entry(Decision_aprobacion).State = EntityState.Modified;
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
            var data = context.decision_Aprobacion.FirstOrDefault(p => p.cod_decision_aprobacion == id);
            if (data != null)
            {
                context.decision_Aprobacion.Remove(data);
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
