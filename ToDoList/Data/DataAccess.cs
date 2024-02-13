using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class DataAccess
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        
        public DataAccess(ApplicationDbContext context, UserManager<User> userManager) {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<ToDoTask>> GetAllTasks(string userId) {

            var tasks = await _context.ToDoTask.Where(u => u.UserId == userId).ToListAsync();
            return tasks;
        }

        public async Task<ToDoTask> GetTask(string taskId)
        {
            var task = await _context.ToDoTask.FindAsync(taskId);
            return task;
        }

        public async Task<bool> DeleteTask(string taskId)
        {
            if(taskId.IsNullOrEmpty()) return false;
            if(GetTask(taskId) == null) return false;
            var task = GetTask(taskId);
            _context.Remove(task); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateTask(ToDoTask task)
        {
            if (task == null) return false;
            _context.Add(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditTask(ToDoTask task)
        {

            if (task == null) return false;
            var oldTask = await _context.ToDoTask.FindAsync(task.Id);
            if (oldTask == null) { return false; }
            else {
                oldTask.Name = task.Name;
                oldTask.updated = task.updated;
                oldTask.completed = task.completed;
                oldTask.PriorityId = task.PriorityId;
                oldTask.StatusId = task.StatusId;
                oldTask.UserId = task.StatusId;

                _context.Update(oldTask);
                await _context.SaveChangesAsync();

                return true;
            }
        }
    }

}
