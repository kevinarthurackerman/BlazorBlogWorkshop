namespace BlazorBlogWorkshop.Server.Test.Tests.Data.Entities
{
    using BlazorBlogWorkshop.Server.Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Xunit;
    using BlazorBlogWorkshop.Server.Data.Entities;
    using System;
    using System.Threading.Tasks;
    using Shouldly;

    public class PostTests
    {
        [Fact]
        public async Task ShouldPersist()
        {
            var testProvider = TestServiceProvider
                .WithConfiguration(config => config.AddDefaultConnectionString())
                .WithServices((sp, config) => sp.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("App"))));

            var createdPost = new Post
            {
                Id = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Slug = "slug",
                Title = "title",
                Body = "body"
            };

            // create post
            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                db.Posts.Add(createdPost);

                await db.SaveChangesAsync();
            });
             
            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                var persistedPost = await db.Posts
                    .SingleOrDefaultAsync(x => x.Id == createdPost.Id);

                persistedPost.ShouldBeEquivalentTo(createdPost);
            });

            // modify post
            Post modifiedPost = null;
            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                var persistedPost = await db.Posts
                    .SingleOrDefaultAsync(x => x.Id == createdPost.Id);

                persistedPost.Title = $"{persistedPost.Title} 2";

                modifiedPost = persistedPost;

                await db.SaveChangesAsync();
            });

            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                var persistedPost = await db.Posts
                    .SingleOrDefaultAsync(x => x.Id == createdPost.Id);

                persistedPost.ShouldBeEquivalentTo(modifiedPost);
            });

            // delete post
            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                var persistedPost = await db.Posts
                    .SingleOrDefaultAsync(x => x.Id == createdPost.Id);

                db.Posts.Remove(persistedPost);

                await db.SaveChangesAsync();
            });

            await testProvider.ExecuteScopeAsync(async sp =>
            {
                var db = sp.GetRequiredService<AppDbContext>();

                var persistedPost = await db.Posts
                    .SingleOrDefaultAsync(x => x.Id == createdPost.Id);

                persistedPost.ShouldBeNull();
            });
        }
    }
}
