using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class ASIGNACION_CC
    {
        [Key]
        public int COD_ASIGNACION_CC { get; set; }
        [Required]
        public int ACC_TIPO_DOCUMENTO { get; set; }
        [Required]
        public string COD_SOCIEDAD { get; set; }
        [Required]
        public int COD_USUARIO { get; set; }
        [Required]
        public string COD_CENTRO_DE_COSTO { get; set; }
    }
}
