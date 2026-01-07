using TaskManagerAPI.Interfaces;
using TaskManagerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagerAPI.Services
{
    public class TaskManagerDb : ITaskManager
    {
        private readonly TaskContext _context;

        public TaskManagerDb(TaskContext context)
        {
            _context = context;
        }

        // Add a new task to the database
        public void AddTask(TaskItem task)
        {
            task.CreatedAt = DateTime.Now;

            // Initialize Tags if null
            if (task.Tags == null)
                task.Tags = new List<string>();

            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        // Update an existing task
        public void UpdateTask(TaskItem updatedTask)
        {
            var task = _context.Tasks.Find(updatedTask.Id)
                       ?? throw new KeyNotFoundException($"Task {updatedTask.Id} not found.");

            // Update fields including tags
            task.Update(
                updatedTask.Title,
                updatedTask.Description,
                updatedTask.DueDate,
                updatedTask.Priority,
                updatedTask.Tags // <-- update tags
            );

            _context.SaveChanges();
        }

        // Delete a task
        public void RemoveTask(int id)
        {
            var task = _context.Tasks.Find(id)
                       ?? throw new KeyNotFoundException($"Task {id} not found.");

            _context.Tasks.Remove(task);
            _context.SaveChanges();
        }

        // Mark a task as completed
        public void MarkTaskCompleted(int id)
        {
            var task = _context.Tasks.Find(id)
                       ?? throw new KeyNotFoundException($"Task {id} not found.");

            task.MarkAsCompleted();
            _context.SaveChanges();
        }

        // Get all tasks, optionally filtering out completed tasks
        public IEnumerable<TaskItem> GetTasks(bool includeCompleted = true)
        {
            if (includeCompleted)
                return _context.Tasks.ToList();
            else
                return _context.Tasks.Where(t => !t.IsCompleted).ToList();
        }

        // Find a task by ID
        public TaskItem FindTaskById(int id)
        {
            var task = _context.Tasks.Find(id)
                       ?? throw new KeyNotFoundException($"Task {id} not found.");
            return task;
        }
    }
}
