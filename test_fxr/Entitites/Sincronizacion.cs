using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Sincronizacion
    {
        [Key]   
        public int cod_sincronizacion { get; set; }
        [Required]
        public int sinc_esactivo { get; set; }
        [Required]
        public int sinc_frecuencia { get; set; }
        [Required]
        public int cod_tipo_sincronizacion { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
    }
}
