using Model;
using Repository;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hospital_Management;

public class SalaryService
{
    private readonly SalaryRepository _salaryRepo;
    private readonly HospitalManagementContext _context;
    public SalaryService(SalaryRepository salaryRepo, HospitalManagementContext context)
    {
        _salaryRepo = salaryRepo;
        _context = context;
    }

    public async Task<Salary> CalculateSalaryAsync(int userId, int month, int year)
    {
        var user = await _context.SystemUsers.Include(u => u.DoctorProfile).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null) throw new Exception("User not found");
        var doctorProfile = user.DoctorProfile;
        if (doctorProfile == null) throw new Exception("Doctor profile not found");
        decimal baseSalary = doctorProfile.BaseSalary ?? 0m;
        if (baseSalary <= 0) throw new Exception("Base salary not set for doctor");
        int standardWorkingDays = 22;
        var timekeepings = await _context.Timekeepings
            .Where(t => t.UserId == userId && t.WorkDate.Month == month && t.WorkDate.Year == year)
            .ToListAsync();
        int workingDays = timekeepings.Count(t => t.Status == "ON TIME" || t.Status == "Late" || t.Status == "Early");
        var rewards = await _context.RewardPenalties
            .Where(r => r.UserId == userId && r.Rpdate.Month == month && r.Rpdate.Year == year && r.Type == "Reward")
            .SumAsync(r => (decimal?)r.Amount) ?? 0m;
        var penalties = await _context.RewardPenalties
            .Where(r => r.UserId == userId && r.Rpdate.Month == month && r.Rpdate.Year == year && r.Type == "Penalty")
            .SumAsync(r => (decimal?)r.Amount) ?? 0m;
        decimal salaryByAttendance = baseSalary * workingDays / standardWorkingDays;
        decimal socialInsurance = salaryByAttendance * 0.08m;
        decimal healthInsurance = salaryByAttendance * 0.015m;
        decimal unemploymentInsurance = salaryByAttendance * 0.01m;
        decimal taxRate = 0.10m;
        decimal taxAmount = (salaryByAttendance + rewards - penalties) * taxRate;
        decimal finalSalary = salaryByAttendance + rewards - penalties - taxAmount - socialInsurance - healthInsurance - unemploymentInsurance;
        var salary = new Salary
        {
            UserId = userId,
            Month = month,
            Year = year,
            BaseSalary = baseSalary,
            WorkingDays = workingDays,
            TotalReward = rewards,
            TotalPenalty = penalties,
            TaxRate = taxRate,
            TaxAmount = taxAmount,
            SocialInsurance = socialInsurance,
            HealthInsurance = healthInsurance,
            FinalSalary = finalSalary,
            GeneratedAt = DateTime.Now,
            User = user // Gán luôn User để binding FullName
        };
        await _salaryRepo.AddOrUpdateSalaryAsync(salary);
        await _salaryRepo.SaveChangesAsync();
        return salary;
    }

    public async Task<List<Salary>> GenerateSalaryReportAsync(int month, int year)
    {
        var users = await _context.SystemUsers.Include(u => u.DoctorProfile)
            .Where(u => u.Role == "Doctor" && u.DoctorProfile != null)
            .ToListAsync();
        var result = new List<Salary>();
        foreach (var user in users)
        {
            var salary = await CalculateSalaryAsync(user.UserId, month, year);
            result.Add(salary);
        }
        return result;
    }

    public async Task<List<Salary>> GetSalariesByUserAsync(int userId)
    {
        return await _salaryRepo.GetSalariesByUserAsync(userId);
    }
} 