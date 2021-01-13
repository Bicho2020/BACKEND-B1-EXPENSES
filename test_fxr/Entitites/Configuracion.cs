using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Configuracion
    {
        [Key]
        public int cod_configuracion { get; set; }
        [Required]
        public int cod_grupo_configuracion_sociedad { get; set; }
        [Required]
        public string conf_concepto { get; set; }
        [Required]
        public string conf_nombre { get; set; }
        [Required]
        public string conf_valor { get; set; }
    }
}
