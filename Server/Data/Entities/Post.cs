using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorBlogWorkshop.Server.Data.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class PostConfig : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Slug).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Body).IsRequired();
        }
    }
}
