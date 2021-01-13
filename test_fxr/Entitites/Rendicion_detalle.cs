using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Rendicion_detalle
    {
        [Key]
        public int cod_rendicion_detalle{ get; set; }
        [Required]
        public int cod_rendicion { get; set; }
        [Required]
        public string cod_plan_de_cuenta { get; set; }
        [Required]
        public string cod_articulo { get; set; }
        [Required]
        public string cod_proyecto{ get; set; }
        [Required]
        public string cod_centro_de_costo { get; set; }
        [Required]
        public string cod_adjunto { get; set; }
        [Required]
        public int rend_linea { get; set; }
        [Required]
        public string rend_comentario { get; set; }
        [Required]
        public string rend_tipo_doc{ get; set; }
        [Required]
        public int rend_monto { get; set; }
        [Required]
        public string REND_NUMERO_DOC { get; set; }
    }
}
