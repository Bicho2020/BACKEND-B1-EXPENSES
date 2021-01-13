using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using B1E.Entitites;
using B1EXPENSES.Entitites;

namespace B1E.Contexts
{
    public class BD : DbContext
    {
        public BD(DbContextOptions<BD> options) : base(options)
        {
            
        }


        public DbSet<LOGS_RENDICION_APROBACION> LOGS_RENDICION_APROBACION { get; set; }
        public DbSet<ASIGNACION_CC> ASIGNACION_CC { get; set; }
        public DbSet<LOGS_FONDO_POR_RENDIR_APROBACION> LOGS_FONDO_POR_RENDIR_APROBACION { get; set; }
        public DbSet<Tipo_Aprobacion> Tipo_Aprobacion { get; set; }
        public DbSet<Permiso_Documento> Permiso_Documento { get; set; }
        public DbSet<Asignacion_Modulo> Asignacion_Modulo { get; set; }
        public DbSet<Asignacion_Sociedad> Asignacion_Sociedad { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Sociedad> Sociedad { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Asignacion_Licencia> Asignacion_Licencia { get; set; }
        public DbSet<Centro_de_costo> Centro_de_costo { get; set; }
        public DbSet<Configuracion> Configuracion { get; set; }
        public DbSet<Decision_aprobacion> decision_Aprobacion { get; set; }
        public DbSet<Doc_Venta> Doc_Venta { get; set; }
        public DbSet<Doc_Venta_Detalle> Doc_Venta_Detalle { get; set; }
        public DbSet<Etapa> Etapa { get; set; }
        public DbSet<Fondo_por_Rendir> Fondo_Por_Rendir { get; set; }
        public DbSet<Fondo_Por_Rendir_Detalle> Fondo_Por_Rendir_Detalle { get; set; }
        public DbSet<Grupo_Configuracion_Sociedad> Grupo_Configuracion_Sociedad { get; set; }
        public DbSet<Licencia> Licencia { get; set; }
        public DbSet<Modulo> Modulo { get; set; }
        public DbSet<Perfil> Perfil{ get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<Plan_de_cuenta> plan_De_Cuenta { get; set; }
        public DbSet<Proceso_aprobacion> Proceso_aprobacion { get; set; }
        public DbSet<Proyecto> Proyecto { get; set; }
        public DbSet<Rendicion> Rendicion{ get; set; }
        public DbSet<Rendicion_detalle> Rendicion_detalle { get; set; }
        public DbSet<Sincronizacion> Sincronizacion { get; set; }
        public DbSet<Tipo_sincronizacion> Tipo_sincronizacion { get; set; }
     
    }
}
