using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentApp.Entities
{
    public class RecruitmentContext : DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<OpenRole> OpenRoles { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<RecruitmentProcess> RecruitmentProcesses { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine("D:\\Work\\RecruitmentApp API\\recruitmentapi\\RecruitmentAppAPI\\", "RecruitmentAppDB.db");
            string connection = $"Filename={path}";
            optionsBuilder.UseSqlite(connection);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.HasKey(c => c.CandidateId).HasName("PrimaryKey");

                entity.Property(c => c.FirstName).HasMaxLength(50);
                entity.Property(c => c.LastName).HasMaxLength(50);
                entity.Property(c=>c.Status).HasMaxLength(50);
                entity.Property(c=> c.DateOfBirth).HasColumnType("datetime");

                entity.HasOne(o => o.OpenRole)
                .WithMany(c => c.Candidates)
                .HasForeignKey(o => o.RoleId);
            });

            modelBuilder.Entity<OpenRole>(entity =>
            {
                entity.HasKey(o => o.RoleId);

                entity.Property(o => o.RoleTitle).HasMaxLength(50);
                entity.Property(o => o.Seniority).HasMaxLength(30);
                entity.Property(o => o.SalaryRange).HasMaxLength(20);
            });

            modelBuilder.Entity<Recruiter>(entity =>
            {
                entity.HasKey(r => r.RecruiterId);

                entity.Property(r => r.FirstName).HasMaxLength(50);
                entity.Property(r => r.LastName).HasMaxLength(50);
                entity.Property(o => o.Seniority).HasMaxLength(30);
            });

            modelBuilder.Entity<RecruitmentProcess>(entity =>
            {
                entity.HasKey(p => p.ProcessId);

                entity.Property(p => p.Stage).HasMaxLength(50);

                entity.HasOne(c => c.Candidate)
                .WithMany(r => r.RecruitmentProcesses)
                .HasForeignKey(c => c.CandidateId);

                entity.HasOne(r => r.Recruiter)
                .WithMany(p => p.RecruitmentProcesses)
                .HasForeignKey(r => r.RecruiterId);
            });
        }
    }
}
