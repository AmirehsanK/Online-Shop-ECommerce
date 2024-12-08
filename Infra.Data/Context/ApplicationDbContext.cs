using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Account;
using Domain.Entities.ContactUs;
using Domain.Entities.Ticket;
using Infra.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Context
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        #region Dbsets

        public DbSet<User> Users { get; set; }

        #endregion

        #region Ticket

        public DbSet<TicketsMessage> TicketsMessages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        

        #endregion

        #region ContactUs

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }

        #endregion
        
        //For Conflict relation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ContactMessageConfiguration());
        }

    

    }
}
