using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Asignacion_Licencia
    {
        [Key]
        public int cod_asignacion_licencia { get; set; }
        [Required]
        public int cod_licencia { get; set; }
        [Required]
        public int cod_usuario { get; set; }
    }
}
