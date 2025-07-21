using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePatientModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Patients__970EC3468134AAD1", x => x.PatientID);
                });

            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    Dependencies = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SystemUs__1788CCACC97E2D57", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientID = table.Column<int>(type: "int", nullable: false),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeSlot = table.Column<TimeOnly>(type: "time", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Scheduled")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Appointm__8ECDFCA2FA10DD48", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointment_Doctor",
                        column: x => x.DoctorID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK_Appointment_Patient",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID");
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    MessageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<int>(type: "int", nullable: false),
                    SenderRole = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceiverID = table.Column<int>(type: "int", nullable: false),
                    ReceiverRole = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatMess__C87C037CCCD5A511", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK__ChatMessa__Recei__5629CD9C",
                        column: x => x.ReceiverID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                    table.ForeignKey(
                        name: "FK__ChatMessa__Sende__5535A963",
                        column: x => x.SenderID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DoctorProfiles",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    JoinDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DoctorPr__1788CCAC39DD035A", x => x.UserID);
                    table.ForeignKey(
                        name: "FK__DoctorPro__UserI__300424B4",
                        column: x => x.UserID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                columns: table => new
                {
                    ScheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorID = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DoctorSc__9C8A5B69F79C45D3", x => x.ScheduleID);
                    table.ForeignKey(
                        name: "FK_Schedule_Doctor",
                        column: x => x.DoctorID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "EmailResetPassword",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ResetCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailResetPassword", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailResetPassword_User",
                        column: x => x.UserId,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RewardPenalty",
                columns: table => new
                {
                    RP_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    RPDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__RewardPe__850C77211E630E50", x => x.RP_ID);
                    table.ForeignKey(
                        name: "FK_RewardPenalty_User",
                        column: x => x.UserID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    SalaryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    TotalReward = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    TotalPenalty = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    TaxRate = table.Column<decimal>(type: "decimal(5,2)", nullable: true, defaultValue: 0m),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    SocialInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    HealthInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    FinalSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true, defaultValue: 0m),
                    GeneratedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Salary__4BE204B7752CFF05", x => x.SalaryID);
                    table.ForeignKey(
                        name: "FK_Salary_User",
                        column: x => x.UserID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Timekeeping",
                columns: table => new
                {
                    TimeKeepingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    CheckInTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    CheckOutTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Timekeep__1F0DBAF1C1858209", x => x.TimeKeepingID);
                    table.ForeignKey(
                        name: "FK_Timekeeping_User",
                        column: x => x.UserID,
                        principalTable: "SystemUsers",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    RecordID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentID = table.Column<int>(type: "int", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalR__FBDF78C987ED6C35", x => x.RecordID);
                    table.ForeignKey(
                        name: "FK_Record_Appointment",
                        column: x => x.AppointmentID,
                        principalTable: "Appointments",
                        principalColumn: "AppointmentID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "UQ_Doctor_AppointmentSlot",
                table: "Appointments",
                columns: new[] { "DoctorID", "AppointmentDate", "TimeSlot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ReceiverID",
                table: "ChatMessages",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderID",
                table: "ChatMessages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "UQ_Doctor_Schedule",
                table: "DoctorSchedules",
                columns: new[] { "DoctorID", "WorkDate", "StartTime", "EndTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailResetPassword_UserId",
                table: "EmailResetPassword",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_AppointmentID",
                table: "MedicalRecords",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_RewardPenalty_UserID",
                table: "RewardPenalty",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ_Salary_PerMonth",
                table: "Salary",
                columns: new[] { "UserID", "Month", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SystemUs__85FB4E388CE9BEE3",
                table: "SystemUsers",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__SystemUs__A9D105340BFD31BA",
                table: "SystemUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timekeeping_UserID",
                table: "Timekeeping",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "DoctorProfiles");

            migrationBuilder.DropTable(
                name: "DoctorSchedules");

            migrationBuilder.DropTable(
                name: "EmailResetPassword");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "RewardPenalty");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "Timekeeping");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "SystemUsers");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
