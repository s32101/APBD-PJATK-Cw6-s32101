using Microsoft.AspNetCore.Mvc;

namespace APBD_PJATK_Cw6_s32101.Controllers;

[ApiController]
[Route("[controller]")]
public class AppointmentController : ControllerBase
{
    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Index()
    {
        return Ok();
    }
}