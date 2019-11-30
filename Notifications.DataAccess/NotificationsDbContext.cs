using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess
{
    public class NotificationsDbContext : DbContext
    {
        public NotificationsDbContext(DbContextOptions<NotificationsDbContext> options)
            : base(options)
        { }

        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<TemplateEntity> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<NotificationEntity>().HasData();

            modelBuilder.Entity<TemplateEntity>().HasData(
                    new TemplateEntity()
                    {
                        Id = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                        EventType = "AppointmentCancelled",
                        Body = "Hi {Firstname}, your appointment with {OrganisationName} at {AppointmentDateTime} has been - cancelled for the following reason: {Reason}.",
                        Title = "Appointment Cancelled"
                    }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
