namespace APBD_PJATK_Cw6_s32101.DTOs;

public record CreateAppointmentRequestDto(
    int IdPatient,
    int IdDoctor,
    string Reason,
    DateTime AppointmentDate
);