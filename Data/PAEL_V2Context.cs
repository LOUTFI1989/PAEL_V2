using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PAEL_V2.Models;

namespace PAEL_V2.Data
{
    public class PAEL_V2Context : DbContext
    {
        public PAEL_V2Context (DbContextOptions<PAEL_V2Context> options)
            : base(options)
        {
        }

        public DbSet<PAEL_V2.Models.Cours> Cours { get; set; } = default!;
        public DbSet<PAEL_V2.Models.Etudiant> Etudiant { get; set; } = default!;
        public DbSet<PAEL_V2.Models.Formation> Formation { get; set; } = default!;
        public DbSet<Inscription> Inscription { get; set; } = default!;

    }
}
