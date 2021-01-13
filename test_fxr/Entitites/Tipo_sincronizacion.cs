using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Tipo_sincronizacion
    {
        [Key]
        public int cod_tipo_sincronizacion { get; set; }
        [Required]
        public string tsin_concepto { get; set; }
    }
}
