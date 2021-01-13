using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Proceso_aprobacion
    { 
        [Key]
        public int cod_proceso_aprobacion { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public DateTime proa_fecha { get; set; }
        [Required]
        public int proa_esvalido { get; set; }
        [Required]
        public string proa_comentario { get; set; }
    }
}
