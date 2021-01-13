using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class Permiso_Documento
    {
        [Key]
        public int COD_PERMISO_DOCUMENTO { get; set; }
        [Required]
        public string COD_SOCIEDAD { get; set; }
        [Required]
        public int TIPO_DOC { get; set; }
        [Required]
        public string PD_DESCRIPCION { get; set; }
    }
}
