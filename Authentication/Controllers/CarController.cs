using Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ApplicationContext applicationContext;

        public CarController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllCars()
        {
            var carList = await applicationContext.Cars.ToListAsync();

            return Ok(carList);
        }
    }
}