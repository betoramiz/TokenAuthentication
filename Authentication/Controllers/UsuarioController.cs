using Authentication.Models;
using Authentication.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private ApplicationContext applicationContext;

        public UsuarioController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<List<UserListVM>> GetAllUsers()
        {
            return await applicationContext.Users.Select(users => new UserListVM() {
                Id = users.Id,
                UserName = users.UserName,
                Email = users.Email,
                Phone = users.PhoneNumber
            }).ToListAsync();
        }
    }
}