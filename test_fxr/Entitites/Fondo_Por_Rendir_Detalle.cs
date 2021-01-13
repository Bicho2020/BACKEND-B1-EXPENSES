using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Fondo_Por_Rendir_Detalle
    {
        [Key]
        public int cod_fondo_por_rendir_detalle { get; set; }
        [Required]
        public int cod_fondo_por_rendir  { get; set; }
   
        public string cod_proyecto { get; set; }
        [Required]
        public string cod_articulo { get; set; }
       
        public string cod_plan_de_cuenta { get; set; }
  
        public string cod_centro_de_costo { get; set; }
        [Required]
        public int fxrd_linea { get; set; }
        [Required]
        public string fxrd_comentario { get; set; }
        [Required]
        public string fxrd_tipo_doc { get; set; }
        [Required]
        public int fxrd_monto { get; set; }

        public string FXRD_COMENTARIO_JEFE { get; set; }

    }
}
