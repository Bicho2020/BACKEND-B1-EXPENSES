using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Grupo_Configuracion_Sociedad
    {
        [Key]
        public int cod_grupo_configuracion_sociedad { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public string grs_nombre { get; set; }
    }
}
