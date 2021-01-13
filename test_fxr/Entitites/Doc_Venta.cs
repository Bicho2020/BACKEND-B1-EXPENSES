using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Doc_Venta
    {
        [Key]
        public int cod_doc_venta { get; set; }
        [Required]
        public char docv_tipo { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int cod_usuario { get; set; }
        [Required]
        public int docv_estado { get; set; }
        [Required]
        public DateTime docv_fecha_creacion { get; set; }
        [Required]
        public DateTime docv_fecha_entrega { get; set; }
        [Required]
        public DateTime docv_fecha_cancelacion { get; set; }
        [Required]
        public int docv_aprobacion_jefe { get; set; }
        [Required]
        public string docv_comentario { get; set; }
    }
}
