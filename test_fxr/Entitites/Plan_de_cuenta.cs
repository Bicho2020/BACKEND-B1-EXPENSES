using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Plan_de_cuenta
    {
        [Key]
        public string cod_plan_de_cuenta { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public string pdc_nombre { get; set; }
     
    }
}
