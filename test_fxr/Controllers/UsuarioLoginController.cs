using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using B1E.Funciones;
using Microsoft.Data;
using Microsoft.Data.SqlClient;


namespace WebService_FxR.Controllers
{
    [Route("api/usuario/login")]
    [ApiController]
    public class UsuarioLoginController : ControllerBase
    {

        private readonly BD context;
        public UsuarioLoginController(BD context)
        {
            this.context = context;
        }


        [HttpPost]
        public ActionResult Post([FromBody] UsuarioLogin login)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            string master = "MASTER";
            var ePass = Encrypt.GETSHA256(login.usu_contrasenia);

            var user = context.Usuario.FirstOrDefault(p => p.usu_correo == login.usu_correo && p.usu_contrasenia == ePass && p.usu_esactivo == 1);
            if (user != null)

            {
                var ConsultarLicenciaAsignada = context.Asignacion_Licencia.FirstOrDefault(p => p.cod_usuario == user.cod_usuario);


                if (ConsultarLicenciaAsignada != null)
                {

                    var ConsultarEsMaster = context.Perfil.FirstOrDefault(p =>  p.perf_nombre == master &&  p.cod_usuario == user.cod_usuario);

                    if (ConsultarEsMaster != null)
                    {
                        objs.Add(new
                        {
                            cod_usuario = user.cod_usuario,
                            cod_empresa = user.cod_empresa,
                            perf = "MASTER",


                        });
                        return Ok(objs);
                    }
                    else {
                        var ConsultarSociedadASignadad = context.Asignacion_Sociedad.FirstOrDefault(p => p.cod_usuario == user.cod_usuario);
                        if (ConsultarSociedadASignadad != null)
                        {
                          
                            return Ok(user.cod_usuario);
                        }
                        else
                        {
                            return BadRequest("No tienes sociedad asignada");
                        }
                        }
             
                }
                else
                {
                    return BadRequest("No tiene Licencia Asignada");
                }
            }
            else
            {
                return BadRequest("Credenciales incorrectas");
            }

        }

    }

}
