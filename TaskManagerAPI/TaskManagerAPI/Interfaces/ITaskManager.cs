using TaskManagerAPI.Models;

namespace TaskManagerAPI.Interfaces
{
    public interface ITaskManager
    {
        void AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
        void RemoveTask(int id);
        void MarkTaskCompleted(int id);
        IEnumerable<TaskItem> GetTasks(bool includeCompleted = true);
        TaskItem FindTaskById(int id);
    }
}
