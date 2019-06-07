using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SlipeAnalyticsServer
{
    class AnalyticsContext : DbContext
    {
        public DbSet<AnalyticsEntry> Entries { get; set; }

        public AnalyticsContext() : base()
        {
            Database.Migrate();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=analytics.db");
        }
    }
}
