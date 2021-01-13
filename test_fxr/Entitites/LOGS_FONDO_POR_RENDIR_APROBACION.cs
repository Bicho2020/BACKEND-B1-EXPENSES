using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace B1EXPENSES.Entitites
{
    public class LOGS_FONDO_POR_RENDIR_APROBACION
    {
        [Key]
        public int cod_logs_fondo_por_rendir_aprobacion { get; set; }
        [Required]
        public string lfxr_hora { get; set; }
        [Required]
        public string lfxr_fecha { get; set; }
        [Required]
        public int lfxr_codigo_fondo_por_rendir_detalle { get; set; }
        [Required]
        public int lfxr_etapa { get; set; }
        [Required]
        public int lfxr_estado { get; set; }
        [Required]
        public int lfxr_linea { get; set; }
        [Required]
        public string lfxr_comentario { get; set; }
        [Required]
        public int lfxr_monto { get; set; }

        public string lfxr_comentario_jefe { get; set; }
        [Required]
        public string lfxr_jefe { get; set; }
        public int LFXR_CODIGO_FONDO_POR_RENDIR { get; set; }
    }
}
