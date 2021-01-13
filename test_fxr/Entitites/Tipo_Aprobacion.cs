using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class Tipo_Aprobacion
    {
        [Key]
        public int cod_tipo_aprobacion { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int ta_tipo_documento { get; set; }
        [Required]
        public int ta_opcion { get; set; }
    }
}
