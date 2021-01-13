using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Doc_Venta_Detalle
    {
        [Key]
        public int cod_doc_venta_detalle{ get; set; }
        [Required]
        public string cod_plan_de_cuenta { get; set; }
        [Required]
        public string cod_proyecto { get; set; }
        [Required]
        public int cod_doc_venta { get; set; }
        [Required]
        public string cod_centro_de_costo { get; set; }
        [Required]
        public int dvd_linea { get; set; }
        [Required]
        public string dvd_comentario { get; set; }
        [Required]
        public int dvd_cantidad { get; set; }
        [Required]
        public int dvd_monto { get; set; }
        [Required]
        public string cod_articulo { get; set; }
    }
}
