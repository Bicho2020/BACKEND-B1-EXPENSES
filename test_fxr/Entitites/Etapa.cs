using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Etapa
    {
        [Key]
        public int cod_etapa { get; set; }
    
        [Required]
        public int cod_usuario { get; set; }
        [Required]
        public int eta_numero { get; set; }
        [Required]
        public string eta_nombre { get; set; }
        [Required]
        public int eta_isvalido { get; set; }
        [Required]
        public int cod_proceso_aprobacion { get; set; }
    }
}
