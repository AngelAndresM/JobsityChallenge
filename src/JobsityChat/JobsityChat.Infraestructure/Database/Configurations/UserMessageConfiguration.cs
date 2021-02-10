using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using JobsityChat.Core.Models;

namespace JobsityChat.Infraestructure.Database.Configurations
{
    public class UserMessageConfiguration : IEntityTypeConfiguration<UserMessage>
    {
        public UserMessageConfiguration()
        {
        }

        public void Configure(EntityTypeBuilder<UserMessage> typeBuilder)
        {
            typeBuilder.ToTable("UserMessages");

            typeBuilder.HasKey(t => t.MessageId);

            typeBuilder.Property(t => t.MessageId)
                .HasDefaultValueSql("NewId()");

            typeBuilder.HasOne(t => t.User)
                .WithMany(t => t.Messages)
                .HasForeignKey(t => t.UserId);
        }
    }
}
