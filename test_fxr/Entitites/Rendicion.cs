using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Rendicion
    {
        [Key]
        public int cod_rendicion { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int cod_usuario { get; set; }
        [Required]
        public int rend_estado{ get; set; }
        [Required]
        public string rend_fecha_creacion { get; set; }
        [Required]
        public string REND_FECHA_REQUERIDA { get; set; }
        [Required]
        public int rend_aprobacion_jefe { get; set; }
        [Required]
        public string rend_comentario { get; set; }
        
        public string cod_centro_de_costo { get; set; }
    
    
        public string cod_proyecto { get; set; }
  
        public int cod_fondo_por_rendir { get; set; }
   
}
}
