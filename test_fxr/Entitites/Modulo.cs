using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Modulo
    {
        [Key]
        public int cod_modulo { get; set; }
        [Required]
        public string mod_nombre { get; set; }
        [Required]
        public int cod_permiso { get; set; }
    }
}
