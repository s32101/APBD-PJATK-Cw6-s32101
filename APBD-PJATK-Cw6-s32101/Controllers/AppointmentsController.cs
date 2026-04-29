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
    
    /*
       Pacjent musi istnieć i być aktywny.
       Lekarz musi istnieć i być aktywny.
       Termin wizyty nie może być w przeszłości.
       Opis wizyty nie może być pusty i powinien mieć maksymalnie 250 znaków.
       Lekarz nie może mieć innej zaplanowanej wizyty dokładnie w tym samym terminie.
     */
    [HttpPost]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentRequestDto obj)
    {
        if (obj.AppointmentDate < DateTime.UtcNow)
            return BadRequest("Termin wizyty nie może być w przeszłości");
        
        await appointmentService.InsertAppointment(obj);
        return Created();
    }

    /*
       Jeśli wizyta nie istnieje, zwróć 404 Not Found.
       Pacjent i lekarz muszą istnieć oraz być aktywni.
       Status musi być jednym z: Scheduled, Completed, Cancelled. - weryfikowane na poziomie bazy
       Jeśli aktualizowana wizyta ma status Completed, nie pozwól zmienić jej terminu.
       Przy zmianie terminu sprawdź konflikt z innymi wizytami tego lekarza.
     */
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAppointmentAsync(int id, [FromBody] UpdateAppointmentRequestDto obj)
    {
        // if (obj.AppointmentDate < DateTime.UtcNow) // dopuszczamy aktualizację starych wizyt
        //     return BadRequest("Termin wizyty nie może być w przeszłości");
        
        if ((await appointmentService.GetAppointmentsAsync(id)).FirstOrDefault() == null)
            return NotFound();
        
        await appointmentService.UpdateAppointment(id, obj);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointmentAsync(int id)
    {
        try
        {
            var existing = (await appointmentService.GetAppointmentsAsync(id)).FirstOrDefault();
            if (existing == null)
                return NotFound();

            if (existing.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                return Conflict("Nie można usunąć zrealizowanej wizyty");
            
            await appointmentService.DeleteAppointment(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}