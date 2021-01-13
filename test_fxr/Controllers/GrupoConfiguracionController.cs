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
    public class GrupoConfiguracionController : ControllerBase
    {
        private readonly BD context;
        public GrupoConfiguracionController(BD context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Grupo_Configuracion_Sociedad> Get()
        {

            var data = context.Grupo_Configuracion_Sociedad.ToList();
            return data;

        }
        [HttpPost]
        public ActionResult Post([FromBody] Grupo_Configuracion_Sociedad Grupo_Configuracion_Sociedad)
        {
            {
                try
                {
                    context.Grupo_Configuracion_Sociedad.Add(Grupo_Configuracion_Sociedad);
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
        public Grupo_Configuracion_Sociedad Get(int id)
        {

            var art = context.Grupo_Configuracion_Sociedad.FirstOrDefault(p => p.cod_grupo_configuracion_sociedad == id);
            return art;

        }
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Grupo_Configuracion_Sociedad Grupo_Configuracion_Sociedad)
        {
            if (Grupo_Configuracion_Sociedad.cod_grupo_configuracion_sociedad == id)
            {
                context.Entry(Grupo_Configuracion_Sociedad).State = EntityState.Modified;
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
            var data = context.Grupo_Configuracion_Sociedad.FirstOrDefault(p => p.cod_grupo_configuracion_sociedad == id);
            if (data != null)
            {
                context.Grupo_Configuracion_Sociedad.Remove(data);
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
