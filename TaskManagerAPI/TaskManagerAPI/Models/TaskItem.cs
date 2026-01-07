using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;  // Required, default empty string
        public string Description { get; set; } = string.Empty; // Required
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        // Store tags as comma-separated string in DB
        public string TagsString { get; set; } = string.Empty;

        // Not mapped to DB: list of tags
        [NotMapped]
        public List<string> Tags
        {
            get => string.IsNullOrEmpty(TagsString) ? new List<string>() : TagsString.Split(',').Select(t => t.Trim()).ToList();
            set => TagsString = value != null ? string.Join(",", value) : string.Empty;
        }

        // Business logic methods
        public void Update(string title, string description, DateTime dueDate, int priority, List<string>? tags = null)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Priority = priority;
            if (tags != null) Tags = tags;
        }

        public void MarkAsCompleted()
        {
            IsCompleted = true;
        }
    }

    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Description).IsRequired();
                entity.Property(t => t.Priority).IsRequired();
                entity.Property(t => t.IsCompleted).IsRequired();
                entity.Property(t => t.CreatedAt).IsRequired();
                entity.Property(t => t.TagsString).HasColumnName("Tags"); // map to DB column "Tags"
            });

            // Seed data with tags
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Setup project",
                    Description = "Initialize TaskManagerAPI",
                    IsCompleted = false,
                    DueDate = new DateTime(2026, 1, 7),
                    Priority = 1,
                    CreatedAt = new DateTime(2026, 1, 6),
                    TagsString = "Work"
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Design database",
                    Description = "Create EF Core models",
                    IsCompleted = false,
                    DueDate = new DateTime(2026, 1, 8),
                    Priority = 2,
                    CreatedAt = new DateTime(2026, 1, 6),
                    TagsString = "Work"
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Read book",
                    Description = "Finish reading personal development book",
                    IsCompleted = false,
                    DueDate = new DateTime(2026, 1, 9),
                    Priority = 3,
                    CreatedAt = new DateTime(2026, 1, 6),
                    TagsString = "Personal"
                }
            );
        }
    }
}
