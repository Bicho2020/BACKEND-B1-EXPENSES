using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class Asignacion_Modulo
    {
        [Key]
        public int cod_asignacion_modulo { get; set; }

        [Required]
        public int cod_modulo { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int cod_usuario { get; set; }
    }
}
