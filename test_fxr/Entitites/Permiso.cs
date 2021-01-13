using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Permiso
    {
        [Key]
        public int cod_permiso { get; set; }
        [Required]
        public int per_esactivo { get; set; }
        [Required]
        public int cod_perfil { get; set; }
    }
}
