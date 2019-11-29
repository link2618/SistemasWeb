using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemasWeb.Areas.Categorias.Models;
using SistemasWeb.Areas.Cursos.Models;

namespace SistemasWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TCategoria> _TCategoria { get; set; }
        public DbSet<TCursos> _TCursos { get; set; }
    }
}
