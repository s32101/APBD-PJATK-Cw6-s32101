using System.ComponentModel.DataAnnotations;

namespace APBD_PJATK_Cw6_s32101.DTOs;

public record CreateAppointmentRequestDto(
    int IdPatient,
    int IdDoctor,
    [Required, MaxLength(250)]
    string Reason,
    DateTime AppointmentDate
);