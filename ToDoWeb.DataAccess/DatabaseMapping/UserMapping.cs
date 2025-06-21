using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.DatabaseMapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Password).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            //builder.Property(x => x.Role).HasConversion(
                    
            //    )
        }
    }
}
