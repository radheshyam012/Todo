using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class TaskController : BaseApiController
    {
        private readonly DataContext _context;
        public TaskController(DataContext context)
        {
            this._context = context;
            
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTask(int id){
            var task = await _context.Tasks.FindAsync(id);
            return task;
        }

        [HttpPost("Add")]
        public async Task<ActionResult<TodoTask>> AddTask(string taskname)
        {
            if(await TaskExist(taskname)) return BadRequest("Task Already Taken"); 
            var task = new TodoTask
            {
                TaskName = taskname
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        [HttpPut("{id}")]
        public async Task<bool> UpdateTask(TodoTask task)
        {
            var tasks = await _context.Tasks.FirstOrDefaultAsync(x=>x.Id == task.Id);
            if(tasks == null) return false;
            tasks.TaskName = task.TaskName;
            
            return await _context.SaveChangesAsync() > 0;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoTask>> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

             _context.Tasks.Remove(task);
             await _context.SaveChangesAsync();
            return task;
        }


        private async Task<bool> TaskExist(string taskname)
        {
            return await _context.Tasks.AnyAsync(x=> x.TaskName == taskname);
        }  
    }
}