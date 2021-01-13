using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Usuario
    {
        [Key]
        public int cod_usuario { get; set; }
        [Required]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "El rut debe tener tener 8 o 9 digitos")]
        public string usu_rut { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Correo debe tener como maximo 50 palabras y como minimo 7")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string usu_correo { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nombre debe tener como maximo 50 palabras y como minimo 2")]
        public string usu_nombre { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Apellido debe tener como maximo 50 palabras y como minimo 2")]
        public string usu_apellido { get; set; }
        [Required]
        public int usu_telefono { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Direccion debe tener como maximo 100 caracteres y como minmo 3")]
        public string usu_direccion { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Contraseña debe tener como minimo 3 caracteres")]
        public string usu_contrasenia { get; set; }
        [Required]
        public int usu_esactivo { get; set; }
        [Required]
        public int cod_empresa { get; set; }

    }
}
