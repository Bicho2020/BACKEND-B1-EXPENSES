using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Centro_de_costo
    {
        [Key]
        public string cod_centro_de_costo { get; set; }
        [Required]
        public  string cod_sociedad { get; set; }
        [Required]
        public string cdc_nombre { get; set; }
        [Required]
        public int cdc_estado { get; set; }

    }
}
