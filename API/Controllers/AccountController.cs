using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
  
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        
        public AccountController(DataContext context, ITokenService tokenService)
        {
            this._tokenService = tokenService;
           
            this._context = context;
            
        }

        [HttpPost("register")]
        
        public async Task<ActionResult<TodoUser>> Register(RegisterDto registerDto)
        {
            if(await UserExist(registerDto.Username)) return BadRequest("Username Taken"); 
            using var hmac = new HMACSHA512();

            var user = new TodoUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Todos.Add(user);
            await _context.SaveChangesAsync();
            return user;
            // return new TodoDto{
            //     Username = user.UserName,
            //     Token = _tokenService.CreateToken(user)
            // };

        }
        
        [HttpPost("login")]
        
        public async Task<ActionResult<TodoDto>> Login(string username,string password)
        {
            var user = await _context.Todos.SingleOrDefaultAsync(x=>x.UserName == username);
            if(user == null ) return Unauthorized("Username Empty");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var ComputeHashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for(int i=0;i<ComputeHashed.Length;i++){
                if(ComputeHashed[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            //return user;
            return new TodoDto{
                Username = user.UserName.ToLower(),
                Token = _tokenService.CreateToken(user)
            };
        }
        
        private async Task<bool> UserExist(string username)
        {
            return await _context.Todos.AnyAsync(x=> x.UserName == username);
        } 

    }
    
}