using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nsiclass.Data.Models;
using System;

namespace Nsiclass.Data
{
    public class ClassDbContext : IdentityDbContext<User>
    {
        public DbSet<Department> Departments { get; set; }

        public DbSet<PartnersLinks> Links { get; set; }

        public DbSet<TC_Classifications> Classifications { get; set; }

        public DbSet<TC_Classif_Vers> ClassVersions { get; set; }

        public DbSet<TC_Classif_Items> ClassItems { get; set; }

        public DbSet<TC_Classif_Rels> ClassRelations { get; set; }

        public DbSet<TC_Classif_Rel_Types> ClassRelationsTypes { get; set; }

        public ClassDbContext(DbContextOptions<ClassDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Department>()
                .HasMany(tt => tt.Employies)
                .WithOne(t => t.Department)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasMany(u => u.UsersCreated)
                .WithOne(u => u.CreateUser)
                .HasForeignKey(t => t.CreateUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Vers>()
                .HasKey(pc => new { pc.Classif, pc.Version });

            builder.Entity<TC_Classifications>()
                .HasMany(c => c.Versions)
                .WithOne(cl => cl.Classification)
                .HasForeignKey(t => t.Classif)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Items>()
                .HasKey(pc => new { pc.Classif, pc.Version, pc.ItemCode });

            builder.Entity<TC_Classif_Vers>()
                .HasMany(v => v.Items)
                .WithOne(c => c.ClassifVersion)
                .HasForeignKey(ci => new { ci.Classif, ci.Version })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Items>()
                .HasOne(p => p.ParentItem)
                .WithMany(ch => ch.ChildItems)
                .HasForeignKey(f => new { f.Classif, f.Version, f.ParentItemCode })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rel_Types>()
                .HasOne(s => s.SourceClassification)
                .WithMany(sc => sc.AsSourceClassification)
                .HasForeignKey(f => f.SrcClassifId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rel_Types>()
                .HasOne(s => s.DestClassification)
                .WithMany(sc => sc.AsDestClassification)
                .HasForeignKey(f => f.DestClassifId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rel_Types>()
                .HasOne(s => s.SourceClassificationVersion)
                .WithMany(sc => sc.AsSourceClassificationVersion)
                .HasForeignKey(f => new { f.SrcClassifId, f.SrcVersionId })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rel_Types>()
                .HasOne(s => s.DestClassificationVersion)
                .WithMany(sc => sc.AsDestClassificationVersion)
                .HasForeignKey(f => new { f.DestClassifId, f.DestVersionId })
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<TC_Classif_Rels>()
                .HasKey(pc => new { pc.SrcClassif, pc.SrcVer, pc.SrcItemId, pc.DestClassif, pc.DestVer, pc.DestItemId, pc.RelationTypeId });

            builder.Entity<TC_Classif_Rels>()
                .HasOne(rt => rt.RelationType)
                .WithMany(rt => rt.Relations)
                .HasForeignKey(f => f.RelationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rels>()
                .HasOne(rs => rs.SrcItem)
                .WithMany(ri => ri.SrcRelations)
                .HasForeignKey(f => new { f.SrcClassif, f.SrcVer, f.SrcItemId })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rels>()
                .HasOne(rs => rs.DestItem)
                .WithMany(ri => ri.DestRelations)
                .HasForeignKey(f => new { f.DestClassif, f.DestVer, f.DestItemId })
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Rels>()
                .HasOne(tr => tr.CreateUser)
                .WithMany(tr => tr.RelsCreated)
                .HasForeignKey(tr => tr.EnteredByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Items>()
                .HasOne(tr => tr.CreateUser)
                .WithMany(tr => tr.ItemsCreated)
                .HasForeignKey(tr => tr.EnteredByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TC_Classif_Items>()
                .HasOne(tr => tr.ModifyUser)
                .WithMany(tr => tr.ItemsModified)
                .HasForeignKey(tr => tr.ModifiedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(builder);
        }
    }
}
