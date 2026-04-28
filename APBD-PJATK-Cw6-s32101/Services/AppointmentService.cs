using APBD_PJATK_Cw6_s32101.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD_PJATK_Cw6_s32101.Services;


public class AppointmentService(IConfiguration configuration)
{
    private string ConnectionString => configuration.GetConnectionString("DefaultConnection") ??
                                       throw new InvalidDataException(
                                           "Nieprawidłowa konfiguracja, brak ciągu definiującego połączenie z bazą danych.");
    
    public async Task<IEnumerable<AppointmentListDto>> GetAppointmentsAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT * FROM Appointments a " +
                                                 "JOIN dbo.Patients p on p.IdPatient = a.IdPatient", connection);

        List<AppointmentListDto> result = [];
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new AppointmentListDto()
            {
                //IdAppointment = reader.GetInt32(reader.GetOrdinal("IdAppointment")),
                IdAppointment = (int) reader["IdAppointment"],
                //IdPatient = (int) reader["IdPatient"],
                AppointmentDate = (DateTime) reader["AppointmentDate"],
                Status = (string) reader["Status"],
                Reason = (string) reader["Reason"],
                PatientFullName = (string) reader["FirstName"] + " " + (string) reader["LastName"],
                PatientEmail = (string) reader["Email"]
            });
        }
        return result;
    }
}