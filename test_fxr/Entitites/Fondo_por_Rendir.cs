using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Fondo_por_Rendir
    {
        [Key]
        public int cod_fondo_por_rendir { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int cod_usuario { get; set; }
        [Required]
        public int fxr_estado { get; set; }
        [Required]
        public DateTime fxr_fecha_creacion { get; set; }
        [Required]
        public int fxr_aprobacion_jefe { get; set; }
        [Required]
        public string fxr_comentarios { get; set; }
        [Required]
        public DateTime FXR_FECHA_NECESARIO { get; set; }
     
        public string cod_proyecto  { get; set; }
     
        public string cod_centro_de_costo { get; set; }
    }
}
