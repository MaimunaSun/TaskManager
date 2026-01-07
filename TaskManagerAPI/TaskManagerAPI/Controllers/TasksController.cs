using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.Models;
using TaskManagerAPI.Interfaces;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskManager _taskManager;

        public TasksController(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> GetTasks([FromQuery] bool includeCompleted = true)
        {
            var tasks = _taskManager.GetTasks(includeCompleted);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTask(int id)
        {
            try
            {
                var task = _taskManager.FindTaskById(id);
                return Ok(task);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] TaskItem task)
        {
            _taskManager.AddTask(task);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskItem updatedTask)
        {
            if (id != updatedTask.Id)
                return BadRequest("Task ID mismatch.");

            try
            {
                _taskManager.UpdateTask(updatedTask);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/complete")]
        public IActionResult MarkTaskCompleted(int id)
        {
            try
            {
                _taskManager.MarkTaskCompleted(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                _taskManager.RemoveTask(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
