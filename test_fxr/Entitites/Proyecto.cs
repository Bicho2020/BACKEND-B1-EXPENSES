using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Proyecto
    {
        [Key]
        public string cod_proyecto { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public string proy_descripcion { get; set; }
        [Required]
        public string proy_fecha_inicio { get; set; }
        [Required]
        public string proy_fecha_fin { get; set; }
        [Required]
        public int proy_estado { get; set; }
    }
}
