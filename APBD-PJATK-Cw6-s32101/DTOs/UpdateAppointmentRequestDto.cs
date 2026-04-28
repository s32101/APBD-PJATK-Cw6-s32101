namespace APBD_PJATK_Cw6_s32101.DTOs;

record UpdateAppointmentRequestDto(
    int IdPatient,
    int IdDoctor,
    DateTime AppointmentDate,
    string Status,
    string Reason,
    string InternalNotes
);