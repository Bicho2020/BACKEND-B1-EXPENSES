using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Sociedad
    {
        [Key]
        public string cod_sociedad{ get; set; }
        [Required]
        public string soc_nombre { get; set; }
        [Required]
        public string soc_servidor { get; set; }
        [Required]
        public string soc_usuario { get; set; }
        [Required]
        public string soc_contrasenia { get; set; }
        [Required]
        public string soc_enlace_sl { get; set; }
        [Required]
        public int soc_esactiva { get; set; }
        [Required]
        public int cod_empresa { get; set; }
        [Required]
        public string soc_version_sql{ get; set; }
        [Required]
        public string soc_contrasenia_bd { get; set; }
        [Required]
        public string soc_usuario_bd { get; set; }
        [Required]
        public string soc_bd { get; set; }
    }
}
