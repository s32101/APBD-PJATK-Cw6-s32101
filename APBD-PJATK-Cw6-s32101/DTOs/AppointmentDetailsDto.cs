namespace APBD_PJATK_Cw6_s32101.DTOs;

class AppointmentDetailsDto : AppointmentListDto
{
    public required string InternalNotes { get; set; }
    public DateTime CreatedAt { get; set; }
}