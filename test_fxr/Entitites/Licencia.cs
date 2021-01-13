using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Licencia
    {
        [Key]
        public int cod_licencia { get; set; }
        [Required]
        public string lic_descripcion { get; set; }
        [Required]
        public DateTime lic_anio_caducacion{ get; set; }
        [Required]
        public int lic_total { get; set; }

    }
}
