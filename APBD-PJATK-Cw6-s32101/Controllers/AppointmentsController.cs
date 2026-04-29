using APBD_PJATK_Cw6_s32101.DTOs;
using APBD_PJATK_Cw6_s32101.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_PJATK_Cw6_s32101.Controllers;

[ApiController]
[Route("api/[controller]")] //api/appointments
public class AppointmentsController(AppointmentService appointmentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAppointmentsAsync
        (string? status = null, string? patientLastName = null)
    {
        return Ok(await appointmentService.GetAppointmentsAsync(null, status, patientLastName));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetAppointmentByIdAsync(int id)
    {
        var appointmentListDto = (await appointmentService.GetAppointmentsAsync(id))
            .FirstOrDefault();
        if (appointmentListDto == null)
            return NotFound();
        return Ok(appointmentListDto);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentRequestDto obj)
    {
        await appointmentService.InsertAppointment(obj);
        return Created();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAppointmentAsync(int id, [FromBody] UpdateAppointmentRequestDto obj)
    {
        await appointmentService.UpdateAppointment(id, obj);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointmentAsync(int id)
    {
        try
        {
            await appointmentService.DeleteAppointment(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}