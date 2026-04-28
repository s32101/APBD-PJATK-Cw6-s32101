using APBD_PJATK_Cw6_s32101.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PJATK_Cw6_s32101.Controllers;

[ApiController]
[Route("api/[controller]")] //api/appointments
public class AppointmentsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointmentsAsync
        (string status, string patientLastName)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetAppointmentByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentRequestDto obj)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAppointmentAsync(int id, [FromBody] UpdateAppointmentRequestDto obj)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointmentAsync(int id)
    {
        throw new NotImplementedException();
    }
}