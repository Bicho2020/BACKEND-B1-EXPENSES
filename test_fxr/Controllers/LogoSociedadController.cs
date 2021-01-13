using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Configuration;

namespace B1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoSociedadController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        private readonly string _connectionString;
        public LogoSociedadController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _connectionString = configuration.GetConnectionString("ConnectionString");

        }

        public class FileUpload
        {
            public IFormFile files
            {
                get;
                set;
            }
        }


        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT TOP 1 LOG_URL FROM LOGO_SOCIEDAD WHERE COD_SOCIEDAD = '{id}' ", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    objs.Add(new
                    {
                        url_foto = reader[0].ToString(),

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


        [HttpDelete("{id}")]
        public ActionResult Delte(string id)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"DELETE FROM LOGO_SOCIEDAD WHERE COD_SOCIEDAD = '{id}' ", connection);
                SqlDataReader reader = command.ExecuteReader();

              
                connection.Close();

                return Ok("Eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        } 



        [HttpPost("{ID}")]
        public async Task<string> Post( string ID , [FromForm] FileUpload objfile  )
        {
            var CurrentDirectory = Directory.GetCurrentDirectory();

            if (objfile.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + " \\uploads\\ "))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + " \\uploads\\ ");
                    }
                    using (FileStream fileStream = System.IO.File.Create(CurrentDirectory.ToString() + "\\img\\" + objfile.files.FileName ))
                    {

                     
                            SqlConnection connection = new SqlConnection(_connectionString);

                            try
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand($"INSERT INTO LOGO_SOCIEDAD VALUES ('{objfile.files.FileName }' , '{ID}') ", connection);
                                SqlDataReader reader = command.ExecuteReader();
                                connection.Close();

                            objfile.files.CopyTo(fileStream);
                            fileStream.Flush();
                            return "Guardado";
                        }
                            catch (Exception ex)
                            {
                                return ex.ToString();
                            }
                        
                    }
                }

                catch (Exception ex)
                {

                    return ex.ToString();
                }
            }
            else
            {
                return "Unsuccessful";
            }
        }

    }
}
