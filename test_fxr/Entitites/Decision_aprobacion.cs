using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Decision_aprobacion
    {
        [Key]
        public int cod_decision_aprobacion { get; set; }
        [Required]
        public string disa_resultado { get; set; }
        [Required]
        public int disa_cod_documento { get; set; }
        [Required]
        public int disa_linea { get; set; }
        [Required]
        public string disa_tipo_documento { get; set; }
    }
}
