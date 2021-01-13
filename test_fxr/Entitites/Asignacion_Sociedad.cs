using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Asignacion_Sociedad
    {
        [Key]
        public int cod_asignacion_sociedad { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public int cod_usuario { get; set; }
  
        public int cod_perfil { get; set; }
        public string codigo_usuario_sap { get; set; }


    }
}
