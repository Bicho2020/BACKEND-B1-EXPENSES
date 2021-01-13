using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1E.Entitites
{
    public class Articulo
    {
        [Key]
        public string cod_articulo { get; set; }
        [Required]
        public string cod_sociedad { get; set; }
        [Required]
        public string cod_cuenta{ get; set; }
        [Required]
        public string art_nombre { get; set; }
        [Required]
        public int art_estado { get; set; }


    }
}
