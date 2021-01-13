using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Perfil
    {
        [Key]
        public int cod_perfil { get; set; }
        [Required]
        public int cod_usuario { get; set; }
        [Required]
        public string perf_nombre { get; set; }
        [Required]
        public string perf_grupoerp { get; set; }
    }
}
