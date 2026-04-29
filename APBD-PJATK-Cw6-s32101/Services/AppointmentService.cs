using APBD_PJATK_Cw6_s32101.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD_PJATK_Cw6_s32101.Services;


public class AppointmentService(IConfiguration configuration)
{
    private string ConnectionString => configuration.GetConnectionString("DefaultConnection") ??
                                       throw new InvalidDataException(
                                           "Nieprawidłowa konfiguracja, brak ciągu definiującego połączenie z bazą danych.");
    
    public async Task<IEnumerable<AppointmentListDto>> GetAppointmentsAsync(int? id = null, string? status = null, string? patientLastName = null)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = new SqlCommand("SELECT * FROM Appointments a " +
                                                 "JOIN Patients p on p.IdPatient = a.IdPatient", connection);

        List<string> conditions = [];
        if (id != null)
        {
            conditions.Add("a.IdAppointment = @id");
            command.Parameters.AddWithValue("@id", id);
        }

        if (status != null)
        {
            conditions.Add("a.Status = @status");
            command.Parameters.AddWithValue("@status", status);
        }

        if (patientLastName != null)
        {
            conditions.Add("p.LastName = @patientLastName");
            command.Parameters.AddWithValue("@patientLastName", patientLastName);
        }

        if (conditions.Count > 0)
            command.CommandText += " WHERE " + string.Join(" AND ", conditions);
        
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

    
    public async Task InsertAppointment(CreateAppointmentRequestDto appointmentDTO)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("INSERT INTO Appointments (IdPatient, IdDoctor, AppointmentDate, Status, Reason, InternalNotes, CreatedAt) " +
                           "VALUES (@idPat, @idDoc, @appDate, 'Scheduled', @reason, null, GETUTCDATE())", connection);
        
        command.Parameters.AddWithValue("@idPat", appointmentDTO.IdPatient);
        command.Parameters.AddWithValue("@idDoc", appointmentDTO.IdDoctor);
        command.Parameters.AddWithValue("@appDate",  appointmentDTO.AppointmentDate);
        command.Parameters.AddWithValue("@reason",  appointmentDTO.Reason);
        
        await command.ExecuteNonQueryAsync();
    }
    
    
    public async Task DeleteAppointment(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("DELETE FROM Appointments WHERE IdAppointment = @co", connection);
        command.Parameters.AddWithValue("@co", id);    
        if (await command.ExecuteNonQueryAsync() != 1)
            throw new Exception("Obiekt nie istnieje lub wystąpił inny problem");
    }

    public async Task UpdateAppointment(int id, UpdateAppointmentRequestDto appointmentDTO)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command =
            new SqlCommand(
                "UPDATE appointments SET " +
                "AppointmentDate = @appDate," +
                "IdDoctor = @idDoc," +
                "Reason = @reason," +
                "Status = @status," +
                "IdPatient = @idPat," +
                "InternalNotes = @inNo" +
                " WHERE IdAppointment = @id",
                connection);
        
        command.Parameters.AddWithValue("@appDate", appointmentDTO.AppointmentDate);
        command.Parameters.AddWithValue("@idDoc", appointmentDTO.IdDoctor);
        command.Parameters.AddWithValue("@reason", appointmentDTO.Reason);
        command.Parameters.AddWithValue("@status", appointmentDTO.Status);
        command.Parameters.AddWithValue("@idPat", appointmentDTO.IdPatient);
        command.Parameters.AddWithValue("@inNo", appointmentDTO.InternalNotes);
        command.Parameters.AddWithValue("@id", id);

        if (await command.ExecuteNonQueryAsync() != 1)
            throw new Exception("Nie udało się wstawić jednego wiersza!");
    }

    public async Task<bool> IsPatientExistingAndActive(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("SELECT COUNT(1) FROM Patients where IdPatient = @id AND IsActive = 1", connection);
        command.Parameters.AddWithValue("@id", id);
        return (int)(await command.ExecuteScalarAsync() ?? 0) == 1;
    }
    
    public async Task<bool> IsDoctorExistingAndActive(int id)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("SELECT COUNT(1) FROM Doctors where IdDoctor = @id AND IsActive = 1", connection);
        command.Parameters.AddWithValue("@id", id);
        return (int)(await command.ExecuteScalarAsync() ?? 0) == 1;
    }

    public async Task<bool> IsDoctorFreeAt(int idDoctor, DateTime when)
    {
        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();
        await using var command = 
            new SqlCommand("SELECT COUNT(1) FROM Appointments where AppointmentDate = @when and IdDoctor = @idDoc", connection);
        command.Parameters.AddWithValue("@when", when);
        command.Parameters.AddWithValue("@idDoc", idDoctor);
        return (int)(await command.ExecuteScalarAsync() ?? 1) == 0;
    }
}