using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class LOGS_RENDICION_APROBACION
    {
        [Key]
        public int COD_LOGS_RENDICION_APROPACION { get; set; }
        [Required]
        public string LRA_HORA { get; set; }
        [Required]
        public string LRA_FECHA { get; set; }
        [Required]
        public int LRA_CODIGO_RENDICION_DETALLE { get; set; }
        [Required]
        public int LRA_ETAPA { get; set; }
        [Required]
        public int LRA_ESTADO { get; set; }
        [Required]
        public int LRA_LINEA { get; set; }
        [Required]
        public string LRA_COMENTARIO { get; set; }
        [Required]
        public int LRA_MONTO { get; set; }

        public string LRA_COMENTARIO_JEFE { get; set; }
        [Required]
        public string LRA_JEFE { get; set; }
        [Required]
        public int LRA_CODIGO_RENDICION { get; set; }

        public int LRA_CODIGO_FONDO_POR_RENDIR { get; set; }
    }
}
