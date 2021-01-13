using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Globalization;


namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdjuntoController : ControllerBase
    {

        public static IWebHostEnvironment _environment;
        private readonly string _connectionString;
        public AdjuntoController(IWebHostEnvironment environment, IConfiguration configuration)
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


        [HttpGet("{COD}")]
        public ActionResult Post(string COD)
        {
            System.Collections.ArrayList objs = new System.Collections.ArrayList();

            SqlConnection connection = new SqlConnection(_connectionString);
            var sql = $"SELECT ADJ_RUTA FROM ADJUNTO WHERE COD_ADJUNTO = '{COD}' ";



            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    objs.Add(new
                    {
                        URL = reader[0].ToString(),
                       
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


        [HttpPost("{ID}")]
        public async Task<string> Post(string ID, [FromForm] FileUpload objfile)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            String hourMinute = DateTime.Now.ToString("HHmmssfff");

            var CurrentDirectory = Directory.GetCurrentDirectory();

            if (objfile.files.Length > 0)
            {
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + " \\adjunto\\ "))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + " \\adjunto\\ ");
                    }
                    using (FileStream fileStream = System.IO.File.Create(CurrentDirectory.ToString() + "\\adjunto\\" + hourMinute + objfile.files.FileName))

                    {
                        

                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand($"INSERT INTO ADJUNTO (COD_ADJUNTO,ADJ_RUTA) values ('{ID}','{ hourMinute + objfile.files.FileName }') ", connection);
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
