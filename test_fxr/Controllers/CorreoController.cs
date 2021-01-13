using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace B1EXPENSES.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorreoController : ControllerBase
    {
      
        private readonly string _connectionString;
        public CorreoController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }


        [HttpPost("{correo}/{codigo}")]
        public async Task<ActionResult> Post(string correo , int codigo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var rs = "";
            var query = $"SELECT COUNT(COD_USUARIO) FROM USUARIO WHERE USU_CORREO = '{correo}' ";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rs = reader[0].ToString();
                }

                if (rs == "1") {
                    var fromAddress = new MailAddress("smoust321@gmail.com", "B1 EXPENSES");
                    var toAddress = new MailAddress(correo, "USUARIO");
                    const string fromPassword = "vichoqazxsw2134e.";
                    const string subject = "Recuperación de contraseña B1EXPENSES";
                    var body = $" Tu código de verificación es  {codigo} ";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,

                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        
                        smtp.Send(message);
                        connection.Close();
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest("Correo invalido");
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

            

        }

        [HttpPost("actualizar/{correo}")]
        public ActionResult UpdatePass(string correo)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            var rs = "";
            var query = $"UPDATE USUARIO SET USU_CONTRASENIA = 'ef797c8118f02dfb649607dd5d3f8c7623048c9c063d532cc95c5ed7a898a64f' WHERE USU_CORREO = '{correo}' ";
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
