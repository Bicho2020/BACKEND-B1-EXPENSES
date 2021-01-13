using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class UsuarioLogin
    {
        [Key]
        public int cod_usuario { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Correo debe tener como maximo 50 palabras y como minimo 7")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string usu_correo {get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Contraseña debe tener como minimo 8 caracteres")]
        public string usu_contrasenia { get; set; }
      
    }
}
