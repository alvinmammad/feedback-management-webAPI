using Entity;
using FeedbackManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedbackManagement.Data.DAL
{
    public class FeedbackManagementDBContext :
        IdentityDbContext<
        AppUser,
        AppRole, int,
        IdentityUserClaim<int>,
        AppUserRole,
        IdentityUserLogin<int>,
        IdentityRoleClaim<int>,
        IdentityUserToken<int>
        >
    {
        #region Constructor
        public FeedbackManagementDBContext()
        {
        }
        #endregion

        #region Database context options
        public FeedbackManagementDBContext(DbContextOptions options) : base(options)
        {

        }
        #endregion

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    if (!options.IsConfigured)
        //    {
        //        options.UseSqlServer("Server=DESKTOP-2NJCST5\\SQLEXPRESS; Initial Catalog = FeedbackManagementDB; Integrated security = SSPI;");
        //    }
        //}

        #region On model creating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            #region Feedback and Feedback categories
            builder.Entity<Feedback>()
                .HasOne(f => f.FeedbackCategory)
                .WithMany(fc => fc.Feedbacks);

            builder.Entity<FeedbackCategories>()
                .HasMany(fc => fc.Feedbacks)
                .WithOne(f => f.FeedbackCategory);
            #endregion


            #region Feedback categories and Department
            builder.Entity<FeedbackCategories>()
             .HasOne(fc => fc.Department)
             .WithMany(d => d.FeedbackCategories);

            builder.Entity<Department>()
              .HasMany(d => d.FeedbackCategories)
              .WithOne(d => d.Department);
            #endregion


            #region User feedbacks and Feedback
            builder.Entity<UserFeedback>()
             .HasOne(fc => fc.Feedback)
             .WithMany(f => f.UserFeedbacks);

            builder.Entity<Feedback>()
              .HasMany(fc => fc.UserFeedbacks)
              .WithOne(f => f.Feedback);
            #endregion


            #region App ,User ,Role
            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId);

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId);
            });
            #endregion

        }
        #endregion

        #region Database sets
        public DbSet<Department> Departments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<FeedbackCategories> FeedbackCategories { get; set; }
        public DbSet<UserFeedback> UserFeedbacks { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        #endregion

    }
}
