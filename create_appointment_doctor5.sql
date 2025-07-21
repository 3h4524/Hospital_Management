USE Hospital_Management;

-- Tạo patient mới
INSERT INTO Patients (FullName, PhoneNumber, Email, Gender, Address, DateOfBirth)
VALUES ('Bệnh nhân Test', '0123456789', 'patient@test.com', 'Male', '123 Test Street', '1990-01-01');

-- Lấy PatientId vừa tạo
DECLARE @NewPatientId INT = SCOPE_IDENTITY();

-- Tạo appointment cho doctor 5
INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, TimeSlot, Reason, Status)
VALUES (@NewPatientId, 5, '2025-01-27', '09:00:00', 'Khám tổng quát', 'Scheduled');

-- Kiểm tra kết quả
SELECT 'Patient created:' as Info, @NewPatientId as PatientId;
SELECT 'Appointment created for Doctor 5:' as Info, AppointmentId, PatientId, DoctorId, AppointmentDate, TimeSlot FROM Appointments WHERE DoctorId = 5; 