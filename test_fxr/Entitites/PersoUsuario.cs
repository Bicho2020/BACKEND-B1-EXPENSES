using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class PersoUsuario
    {

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nombre debe tener como maximo 50 palabras y como minimo 2")]
        public string usu_nombre { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Apellido debe tener como maximo 50 palabras y como minimo 2")]
        public string usu_apellido { get; set; }
        [Required]
        public int  usu_telefono { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Direccion debe tener como maximo 100 caracteres y como minmo 5")]
        public string usu_direccion { get; set; }
    }
}
