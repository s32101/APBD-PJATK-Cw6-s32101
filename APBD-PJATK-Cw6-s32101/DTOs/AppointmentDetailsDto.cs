namespace APBD_PJATK_Cw6_s32101.DTOs;

record AppointmentDetailsDto : AppointmentShortDto
{
    public required string Status { get; set; }
    public required string Reason { get; set; }
    public required string InternalNotes { get; set; }
    public DateTime CreatedAt { get; set; }
}