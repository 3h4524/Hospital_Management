using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class SalaryRepository : BaseRepository<Salary>
{
    public SalaryRepository(HospitalManagementContext context) : base(context) {}

    public async Task<Salary?> GetSalaryAsync(int userId, int month, int year)
    {
        return await Task.Run(() =>
            _context.Salaries.FirstOrDefault(s => s.UserId == userId && s.Month == month && s.Year == year));
    }

    public async Task<List<Salary>> GetSalariesByMonthYearAsync(int month, int year)
    {
        var salaries = await _context.Salaries
            .Where(s => s.Month == month && s.Year == year)
            .ToListAsync();
        var userIds = salaries.Select(s => s.UserId).Distinct().ToList();
        var users = _context.SystemUsers.Where(u => userIds.Contains(u.UserId)).ToList();
        var userDict = users.ToDictionary(u => u.UserId, u => u);
        foreach (var s in salaries)
        {
            if (userDict.TryGetValue(s.UserId, out var user))
                s.User = user;
        }
        return salaries;
    }

    public async Task<List<Salary>> GetSalariesByUserAsync(int userId)
    {
        return await Task.Run(() =>
            _context.Salaries.Where(s => s.UserId == userId).ToList());
    }

    public async Task AddOrUpdateSalaryAsync(Salary salary)
    {
        var existing = _context.Salaries.FirstOrDefault(s => s.UserId == salary.UserId && s.Month == salary.Month && s.Year == salary.Year);
        if (existing != null)
        {
            // Chỉ update các trường dữ liệu, KHÔNG update SalaryId
            existing.BaseSalary = salary.BaseSalary;
            existing.WorkingDays = salary.WorkingDays;
            existing.TotalReward = salary.TotalReward;
            existing.TotalPenalty = salary.TotalPenalty;
            existing.TaxRate = salary.TaxRate;
            existing.TaxAmount = salary.TaxAmount;
            existing.SocialInsurance = salary.SocialInsurance;
            existing.HealthInsurance = salary.HealthInsurance;
            existing.FinalSalary = salary.FinalSalary;
            existing.GeneratedAt = salary.GeneratedAt;
            // Nếu có thêm trường nào mới, update ở đây
        }
        else
        {
            _context.Salaries.Add(salary);
        }
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
} 