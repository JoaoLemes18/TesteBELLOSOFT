using API.Data;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ClimateRepository : IClimateRepository
    {
        private readonly ApplicationDbContext _ctx;

        public ClimateRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(ClimateRecord record)
        {
            await _ctx.ClimateRecords.AddAsync(record);
            await _ctx.SaveChangesAsync();
        }

        public async Task<List<ClimateRecord>> GetAllAsync()
        {
            return await _ctx.ClimateRecords
                .AsNoTracking() 
                .OrderByDescending(c => c.CapturedAtUtc)
                .Take(200)
                .ToListAsync();
        }
    }
}
