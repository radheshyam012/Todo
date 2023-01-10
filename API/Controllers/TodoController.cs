using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class TodoController: BaseApiController
    {
        private readonly DataContext _context;
        public TodoController(DataContext context)
        {
            this._context = context;
            
        }

        [HttpGet]  
        public async Task<ActionResult<IEnumerable<TodoUser>>> GetRegisterUsers()
        {
            var user = await _context.Todos.ToListAsync();
            return user;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoUser>> GetRegisterUsersById(int id)
        {
            return await _context.Todos.FindAsync(id);
        }

        [HttpPut("{id}")]
      
        public async Task<bool> UpdateRegisterUser(TodoUser todo)
        {
            var users = await _context.Todos.FirstOrDefaultAsync(x=>x.Id == todo.Id);

            if(users == null)
            {
                return false;
            }
            users.UserName = todo.UserName;
            // users.PasswordHash = todo.PasswordHash;
            // users.PasswordSalt = todo.PasswordSalt;           
           return await _context.SaveChangesAsync() > 0;
        }

        [HttpDelete("{id}")]
       
        public async Task<ActionResult<TodoUser>> DeleteRegisterUser(int id)
        {
            var user = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

    }
}