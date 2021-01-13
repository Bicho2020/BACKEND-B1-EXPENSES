using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B1E.Contexts;
using B1E.Entitites;
using B1E.Funciones;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Dynamic;

namespace WebService_FxR.Controllers
{

    [Route("api/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly BD context;
        private readonly string _connectionString;
        public UsuarioController(BD context , IConfiguration configuration)
        {
            this.context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            var data = context.Usuario.FromSqlRaw("Select cod_empresa , cod_usuario,usu_rut,usu_correo,usu_apellido,usu_telefono,usu_direccion,usu_esactivo , usu_contrasenia='null' , usu_nombre from usuario;").ToList();
            return data;
        }

        [HttpGet("jefe/{cod_sociedad}")]
        public ActionResult Get(string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("" +
                    $"SELECT US.COD_USUARIO , US.USU_NOMBRE , US.USU_APELLIDO  FROM USUARIO US " +
                    "JOIN PERFIL PER " +
                    "ON PER.COD_USUARIO = US.COD_USUARIO " +
                    "JOIN ASIGNACION_SOCIEDAD ASI " +
                    "ON ASI.COD_PERFIL = PER.COD_PERFIL " +
                    "JOIN SOCIEDAD SO " +
                    "ON SO.COD_SOCIEDAD = ASI.COD_SOCIEDAD " +
                    $"WHERE ASI.COD_SOCIEDAD = '{cod_sociedad}' AND US.USU_ESACTIVO = 1 AND SO.SOC_ESACTIVA = 1  ", connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString()
                    });
                }
                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("cambiarContrasenia/{newPass}/{COD}")]
        public ActionResult CambiarContra(string newPass,int COD)
        {
        

            SqlConnection connection = new SqlConnection(_connectionString);

            
            var ePass = Encrypt.GETSHA256(newPass);

            var query = $"UPDATE USUARIO SET USU_CONTRASENIA = '{ePass}' WHERE COD_USUARIO = {COD} ";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();

               
                connection.Close();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Usuario Usuario)
        {
            var rut = Usuario.usu_rut;

            try
            {
                Boolean RespRuti = Rutificador.Ruti(rut);

                if (RespRuti == true)
                {
                    try
                    {
                        var queryRut = context.Usuario.Count(p => p.usu_rut == Usuario.usu_rut);
                        if (queryRut == 0)
                        {
                            var queryCorreo = context.Usuario.Count(p => p.usu_correo == Usuario.usu_correo);

                            if (queryCorreo == 0)
                            {
                                var pass = Usuario.usu_contrasenia;
                                var passE = Encrypt.GETSHA256(pass);

                                var DataUser = new Usuario()
                                {
                                    usu_rut = Usuario.usu_rut,
                                    usu_correo = Usuario.usu_correo,
                                    usu_nombre = Usuario.usu_nombre,
                                    usu_apellido = Usuario.usu_apellido,
                                    usu_telefono = Usuario.usu_telefono,
                                    usu_direccion = Usuario.usu_direccion,
                                    cod_empresa = Usuario.cod_empresa,
                                    usu_contrasenia = passE,
                                    usu_esactivo = 1
                                };
                                context.Usuario.Add(DataUser);
                                context.SaveChanges();
                                return Ok("Registrado");
                            }
                            else
                            {
                                return BadRequest("Correo ya registrado");
                            }
                        }
                        else
                        {
                            return BadRequest("Rut ya registrado");
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
                else
                {
                    return BadRequest("Rut incorrecto");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet("{value}/{dato}/{codEmp}")]
        public IEnumerable<Usuario> Get(string value, string dato, int codEmp)
        {

            var data = context.Usuario.FromSqlRaw($"Select cod_empresa , cod_usuario,usu_rut,usu_correo,usu_apellido,usu_telefono,usu_direccion,usu_esactivo , usu_contrasenia='null' , usu_nombre , 'asdsa' from usuario where {value} = '{dato}' and cod_empresa = {codEmp}");
            return data;

        }

        [HttpGet("autorizadores/{cod_empresa}/{cod_sociedad}")]
        public ActionResult GetQuery(int cod_empresa, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT US.COD_USUARIO,US.USU_NOMBRE,US.USU_APELLIDO,US.USU_RUT FROM PERFIL AS PL JOIN USUARIO US ON US.COD_USUARIO = PL.COD_USUARIO JOIN ASIGNACION_SOCIEDAD ASI ON ASI.COD_PERFIL = PL.COD_PERFIL WHERE PL.PERF_NOMBRE = 'AUTORIZADOR' AND US.COD_EMPRESA = {cod_empresa} AND ASI.COD_SOCIEDAD = '{cod_sociedad}' ;", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString(),
                        usu_rut = reader[3].ToString()
                    });
                }


                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("administradores/{cod_empresa}/{cod_sociedad}")]
        public ActionResult GetQueryAdmin(int cod_empresa, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT US.COD_USUARIO,US.USU_NOMBRE,US.USU_APELLIDO,US.USU_RUT FROM PERFIL AS PL JOIN USUARIO US ON US.COD_USUARIO = PL.COD_USUARIO JOIN ASIGNACION_SOCIEDAD ASI ON ASI.COD_PERFIL = PL.COD_PERFIL WHERE PL.PERF_NOMBRE = 'ADMINISTRADOR' AND US.COD_EMPRESA = {cod_empresa} AND ASI.COD_SOCIEDAD = '{cod_sociedad}' ;", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString(),
                        usu_rut = reader[3].ToString()
                    });
                }


                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("usuarios/{cod_empresa}/{cod_sociedad}")]
        public ActionResult GetQueryUsuario(int cod_empresa, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT US.COD_USUARIO,US.USU_NOMBRE,US.USU_APELLIDO,US.USU_RUT FROM PERFIL AS PL JOIN USUARIO US ON US.COD_USUARIO = PL.COD_USUARIO JOIN ASIGNACION_SOCIEDAD ASI ON ASI.COD_PERFIL = PL.COD_PERFIL WHERE PL.PERF_NOMBRE = 'USUARIO' AND US.COD_EMPRESA = {cod_empresa} AND ASI.COD_SOCIEDAD = '{cod_sociedad}' ;", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString(),
                        usu_rut = reader[3].ToString()
                    });
                }


                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpGet("Autoriza/{cod_empresa}/{cod_sociedad}")]
        public ActionResult GetQueryAutorizadores(int cod_empresa, string cod_sociedad)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT US.COD_USUARIO,US.USU_NOMBRE,US.USU_APELLIDO,US.USU_RUT FROM PERFIL AS PL JOIN USUARIO US ON US.COD_USUARIO = PL.COD_USUARIO JOIN ASIGNACION_SOCIEDAD ASI ON ASI.COD_PERFIL = PL.COD_PERFIL WHERE PL.PERF_NOMBRE = 'AUTORIZADOR' AND US.COD_EMPRESA = {cod_empresa} AND ASI.COD_SOCIEDAD = '{cod_sociedad}' ;", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString(),
                        usu_rut = reader[3].ToString()
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


    [HttpPut("estado/{id}/{estado}")]
    public ActionResult UpdateEstado(int id, int estado)
    {
        var DataUser = new Usuario()
        {
            cod_usuario = id,
            usu_esactivo = estado
        };

        try
        {
            context.Usuario.Attach(DataUser).Property(x => x.usu_esactivo).IsModified = true;
            context.SaveChanges();
            if (estado == 0)
            {
                return Ok("Usuario Desactivado");
            }
            else
            {
                return Ok("Usuario Activado");
            }

        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

        [HttpPut("actualizar/{COD_USUARIO}")]
        public ActionResult UpdateUsuario(int COD_USUARIO , Object obj)
        {
            var usr = JsonConvert.DeserializeObject<List<ExpandoObject>>(obj.ToString());

            var rut = "";
            var correo = "";
            var nombre = "";
            var apellido = "";
            var direccion = "";
            var telefono = "";

            foreach (dynamic x in usr)
            {
                 rut = x.usu_rut;
                 correo = x.usu_correo;
                nombre = x.usu_nombre;
                apellido = x.usu_apellido;
                 direccion = x.usu_direccion;
                 telefono = x.usu_telefono;

            }

          
            

            SqlConnection connection = new SqlConnection(_connectionString);

            var query = $"UPDATE USUARIO SET USU_RUT = '{rut}' , USU_CORREO = '{correo}' , USU_NOMBRE = '{nombre}' , USU_TELEFONO = '{telefono}' , USU_DIRECCION = '{direccion}' , USU_APELLIDO = '{apellido}' WHERE COD_USUARIO = {COD_USUARIO}";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                connection.Close();

                return Ok("Actualizado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }


        [HttpGet("query/{COD_EMPRESA}")]
        public ActionResult GetQuery(int COD_EMPRESA)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            var query = "SELECT * FROM (SELECT USU.COD_USUARIO ,  USU.USU_RUT , USU.USU_NOMBRE , USU.USU_APELLIDO , CASE "+
                        "WHEN USU.COD_USUARIO IN(SELECT COD_USUARIO FROM PERFIL WHERE COD_USUARIO = USU.COD_USUARIO AND PERF_NOMBRE = 'MASTER') THEN 'A' ELSE 'B' END 'AB' " +
                        $"FROM USUARIO AS USU WHERE USU.COD_EMPRESA = {COD_EMPRESA} AND USU_ESACTIVO = 1) a WHERE A.AB = 'B' ";


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_rut = reader[1].ToString(),
                        usu_nombre = reader[2].ToString(),
                        usu_apellido = reader[3].ToString()         
                    });
                }

             
                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }


        [HttpGet("sociedad/{COD_SOC}")]
        public ActionResult GetQuery(string COD_SOC)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = "SELECT US.COD_USUARIO , US.USU_NOMBRE , US.USU_APELLIDO , US.USU_RUT FROM ASIGNACION_SOCIEDAD AS ASS  " +
                       "JOIN USUARIO AS US " +
                       "ON US.COD_USUARIO = ASS.COD_USUARIO " +
                       $"WHERE ASS.COD_SOCIEDAD = '{COD_SOC}' AND US.USU_ESACTIVO = 1;";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        cod_usuario = reader[0].ToString(),
                        usu_rut = reader[3].ToString(),
                        usu_nombre = reader[1].ToString(),
                        usu_apellido = reader[2].ToString()
                    });
                }


                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpGet("query/jefe/{id}/{SOC}")]
        public ActionResult GetQueryAsignados(int id , string SOC)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();


            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM ASIGNACION_SOCIEDAD WHERE COD_JEFE = {id} AND COD_SOCIEDAD = '{SOC}' ", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        respuesta = reader[0].ToString(),
                    
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpGet("datos/{COD_USUARIO}")]
        public ActionResult GetQueryAsignados(int COD_USUARIO)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"SELECT usu_rut , usu_correo , usu_nombre , usu_apellido , usu_telefono , usu_direccion  FROM USUARIO WHERE COD_USUARIO = {COD_USUARIO}";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        usu_rut = reader[0].ToString(),
                        usu_correo = reader[1].ToString(),
                        usu_nombre = reader[2].ToString(),
                        usu_apellido = reader[3].ToString(),
                        usu_telefono = reader[4].ToString(),
                        usu_direccion = reader[5].ToString(),

                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("datos2/{COD_USUARIO}")]
        public ActionResult GetQueryAsignados2(int COD_USUARIO)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);
            var query = $"SELECT SO.SOC_NOMBRE , PE.PERF_NOMBRE ,  " +
                    "case " +
                    "when cod_jefe in (SELECT COD_USUARIO FROM USUARIO WHERE COD_USUARIO = AGS.COD_JEFE ) THEN(SELECT USU_NOMBRE + ' ' + USU_APELLIDO FROM USUARIO WHERE COD_USUARIO = AGS.COD_JEFE) ELSE " +
                    "'NO ASIGNADO' end 'Jefe' " +
                    "FROM ASIGNACION_SOCIEDAD AS AGS " +
                    "JOIN SOCIEDAD SO " +
                    "ON SO.COD_SOCIEDAD = AGS.COD_SOCIEDAD " +
                    "JOIN PERFIL AS PE " +
                    "ON PE.COD_PERFIL = AGS.COD_PERFIL " +
                    $"WHERE AGS.COD_USUARIO = {COD_USUARIO} AND SO.SOC_ESACTIVA = 1";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        sociedad = reader[0].ToString(),
                        perfil = reader[1].ToString(),
                        jefe = reader[2].ToString(),
                    });
                }

                connection.Close();
                return Ok(objs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("EstadisticasUsuario/{COD_USUARIO}/{COD_SOCIEDAD}")]
        public ActionResult EstadisticasUsuario(int COD_USUARIO , string COD_SOCIEDAD)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_1 = $"EXECUTE ESTADISTICA_RENDICION @COD_SOCIEDAD = {COD_SOCIEDAD} , @COD_USUARIO = {COD_USUARIO}";
          

            try
            {
                connection.Open();
  
               SqlCommand command = new SqlCommand(query_1, connection);
             SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        total = reader[0].ToString(),
                        rechazados = reader[1].ToString(),
                        contable = reader[2].ToString(),
                        aceptado = reader[3].ToString(),
                        proceso = reader[4].ToString(),
                    });
                }

                connection.Close();
                
                return Ok(objs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            

        }

        [HttpGet("EstadisticasUsuario_2/{COD_USUARIO}/{COD_SOCIEDAD}")]
        public ActionResult EstadisticasUsuario_2(int COD_USUARIO , string COD_SOCIEDAD)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE ESTADISTICA_FONDO @COD_SOCIEDAD = {COD_SOCIEDAD} , @COD_USUARIO = {COD_USUARIO}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        total = reader[0].ToString(),
                        rechazados = reader[1].ToString(),
                        contable = reader[2].ToString(),
                        aceptado = reader[3].ToString(),
                        proceso = reader[4].ToString(),
                    });
                }

                connection.Close();
                
                return Ok(objs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            

        }


        [HttpGet("EstadisticaMaster/{COD_EMPRESA}")]
        public ActionResult EstadisticaMaster(int COD_EMPRESA )
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE MASTER_TOTALES @COD_EMPRESA = {COD_EMPRESA}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        total_licencia = reader[0].ToString(),
                        sin_licencia = reader[1].ToString(),
                        usuarios = reader[2].ToString(),
                        sociedades = reader[3].ToString(),
                    });
                }

                connection.Close();
                
                return Ok(objs[0]);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("MASTER_ARTICULOS_FONDOS/{COD_EMPRESA}")]
        public ActionResult MASTER_ARTICULOS_FONDOS(int COD_EMPRESA )
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE MASTER_ARTICULOS_FONDOS @COD_EMPRESA = {COD_EMPRESA}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        total= reader[0].ToString(),
                        producto = reader[1].ToString(),
                     
                    });
                }

                connection.Close();
                
                return Ok(objs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("MASTER_ARTICULOS_RENDICION/{COD_EMPRESA}")]
        public ActionResult MASTER_ARTICULOS_RENDICION(int COD_EMPRESA )
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE MASTER_ARTICULOS_RENDICION @COD_EMPRESA = {COD_EMPRESA}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        total= reader[0].ToString(),
                        producto = reader[1].ToString(),
                     
                    });
                }

                connection.Close();
                
                return Ok(objs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpGet("MASTER_MES_RENDICION/{COD_EMPRESA}")]
        public ActionResult MASTER_MES_RENDICION(int COD_EMPRESA )
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE MASTER_MES_RENDICION @COD_EMPRESA = {COD_EMPRESA}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        enero = reader[0].ToString(),
                        febrero = reader[1].ToString(),
                        marzo = reader[2].ToString(),
                        abril = reader[3].ToString(),
                        mayo = reader[4].ToString(),
                        junio = reader[5].ToString(),
                        julio = reader[6].ToString(),
                        agosto = reader[7].ToString(),
                        septiembre = reader[8].ToString(),
                        octubre = reader[9].ToString(),
                        noviembre = reader[10].ToString(),
                        deciebre = reader[11].ToString(),

                     
                    });
                }

                connection.Close();
                
                return Ok(objs[0]);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        [HttpGet("MASTER_MES_FONDO/{COD_EMPRESA}")]
        public ActionResult MASTER_MES_FONDO(int COD_EMPRESA )
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();
            SqlConnection connection = new SqlConnection(_connectionString);

            var query_2 = $"EXECUTE MASTER_MES_FONDO  @COD_EMPRESA = {COD_EMPRESA}";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query_2, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    objs.Add(new
                    {
                        enero = reader[0].ToString(),
                        febrero = reader[1].ToString(),
                        marzo = reader[2].ToString(),
                        abril = reader[3].ToString(),
                        mayo = reader[4].ToString(),
                        junio = reader[5].ToString(),
                        julio = reader[6].ToString(),
                        agosto = reader[7].ToString(),
                        septiembre = reader[8].ToString(),
                        octubre = reader[9].ToString(),
                        noviembre = reader[10].ToString(),
                        deciebre = reader[11].ToString(),

                     
                    });
                }

                connection.Close();
                
                return Ok(objs[0]);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        
        [HttpGet("clear")]
        public ActionResult Clear2()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var query= "ALTER DATABASE B1S SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE";

            try
            {
                connection.Open();
  
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
              
                connection.Close();

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
   
}
