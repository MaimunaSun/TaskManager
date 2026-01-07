using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using TaskManagerAPI.Services;
using Microsoft.EntityFrameworkCore;

using Xunit;

namespace TaskManagerAPI.Tests
{
    public class TaskManagerTests
    {
        private TaskManager GetInMemoryTaskManager(string dbName)
        {
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new TaskContext(options);
            return new TaskManager(context);
        }

        [Fact]
        public void AddTask_ShouldAddNewTask_WithTags()
        {
            // Arrange
            var taskManager = GetInMemoryTaskManager("AddTaskDb");
            var task = new TaskItem
            {
                Title = "Test Task",
                Description = "This is a test",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                CreatedAt = DateTime.Now,
                Tags = new List<string> { "Work", "Urgent" }
            };

            // Act
            taskManager.AddTask(task);

            // Assert
            var tasks = taskManager.GetTasks().ToList();
            Assert.Single(tasks);
            Assert.Equal("Test Task", tasks[0].Title);
            Assert.Contains("Work", tasks[0].Tags);
            Assert.Contains("Urgent", tasks[0].Tags);
        }

        [Fact]
        public void MarkTaskCompleted_ShouldSetIsCompletedToTrue()
        {
            // Arrange
            var taskManager = GetInMemoryTaskManager("CompleteTaskDb");
            taskManager.AddTask(new TaskItem
            {
                Title = "Test Task",
                Description = "Test",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                CreatedAt = DateTime.Now
            });

            // Act
            taskManager.MarkTaskCompleted(1);

            // Assert
            var task = taskManager.FindTaskById(1);
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public void RemoveTask_ShouldRemoveTask()
        {
            // Arrange
            var taskManager = GetInMemoryTaskManager("RemoveTaskDb");
            taskManager.AddTask(new TaskItem
            {
                Title = "Test Task",
                Description = "Test",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                CreatedAt = DateTime.Now
            });

            // Act
            taskManager.RemoveTask(1);

            // Assert
            Assert.Empty(taskManager.GetTasks());
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskDetails_AndTags()
        {
            // Arrange
            var taskManager = GetInMemoryTaskManager("UpdateTaskDb");
            taskManager.AddTask(new TaskItem
            {
                Title = "Old Title",
                Description = "Old Description",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                CreatedAt = DateTime.Now,
                Tags = new List<string> { "Work" }
            });

            // Act
            taskManager.UpdateTask(new TaskItem
            {
                Id = 1,
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(2),
                Priority = 2,
                Tags = new List<string> { "Personal", "Urgent" }
            });

            // Assert
            var task = taskManager.FindTaskById(1);
            Assert.Equal("New Title", task.Title);
            Assert.Equal("New Description", task.Description);
            Assert.Equal(2, task.Priority);
            Assert.Contains("Personal", task.Tags);
            Assert.Contains("Urgent", task.Tags);
        }

        [Fact]
        public void GetTasks_ShouldFilterCompletedTasks()
        {
            // Arrange
            var taskManager = GetInMemoryTaskManager("FilterTasksDb");
            taskManager.AddTask(new TaskItem
            {
                Title = "Task 1",
                Description = "Incomplete",
                DueDate = DateTime.Now.AddDays(1),
                Priority = 1,
                CreatedAt = DateTime.Now
            });
            taskManager.AddTask(new TaskItem
            {
                Title = "Task 2",
                Description = "Completed",
                DueDate = DateTime.Now.AddDays(2),
                Priority = 2,
                CreatedAt = DateTime.Now
            });
            taskManager.MarkTaskCompleted(2);

            // Act
            var allTasks = taskManager.GetTasks(includeCompleted: true).ToList();
            var pendingTasks = taskManager.GetTasks(includeCompleted: false).ToList();

            // Assert
            Assert.Equal(2, allTasks.Count);
            Assert.Single(pendingTasks);
            Assert.Equal("Task 1", pendingTasks[0].Title);
        }
    }
}
