using Microsoft.AspNetCore.Mvc;

namespace APBD_PJATK_Cw6_s32101.Controllers;

public class AppointmentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}