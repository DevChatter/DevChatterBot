using System;
using DevChatter.Bot.Core.Messaging;
using Microsoft.EntityFrameworkCore;

namespace DevChatter.Bot.Infra.Ef
{
    public class AppDataContext : DbContext
    {
        public DbSet<IntervalTriggeredMessage> IntervalTriggeredMessages { get; set; }
        public DbSet<StaticCommandResponseMessage> StaticCommandResponseMessage { get; set; }

        public AppDataContext()
        { }

        public AppDataContext(DbContextOptions<AppDataContext> options)
            : base(options)
        { }

    }
}