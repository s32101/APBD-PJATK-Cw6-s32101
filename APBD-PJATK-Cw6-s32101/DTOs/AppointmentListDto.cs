
namespace APBD_PJATK_Cw6_s32101.DTOs;

record AppointmentListDto(
    IEnumerable<AppointmentShortDto> Appointments
);

record AppointmentShortDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}