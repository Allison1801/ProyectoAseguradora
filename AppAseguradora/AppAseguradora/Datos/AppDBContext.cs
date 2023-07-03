using AppAseguradora.Modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace AppAseguradora.Datos
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }

        //mapeo de datos para realizar el crud
        public DbSet<Asegurado> Asegurados { get; set; }
        public DbSet<Seguro> Seguros { get; set; }



        //Muestra la relacción entre el login y el usuario mediante la obtención del Id
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Asegurado>()
                 .HasOne(c => c.Seguro)
                 .WithMany()
                  .HasForeignKey(c => c.idSeguro)
                 .HasConstraintName("FK_Seguro_Asegurados"); 


            

        }
         

    }

 }

