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
    public class PermisoController : ControllerBase
    {
        private readonly BD context;
        public PermisoController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Permiso> Get()
        {

            var data = context.Permiso.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Permiso Permiso)
        {
            {
                try
                {
                    context.Permiso.Add(Permiso);
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
        public Permiso Get(int id)
        {

            var art = context.Permiso.FirstOrDefault(p => p.cod_permiso == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Permiso Permiso)
        {
            if (Permiso.cod_permiso == id)
            {
                context.Entry(Permiso).State = EntityState.Modified;
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
            var data = context.Permiso.FirstOrDefault(p => p.cod_permiso == id);
            if (data != null)
            {
                context.Permiso.Remove(data);
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
