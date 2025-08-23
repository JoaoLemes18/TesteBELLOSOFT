using API.Data;
using API.Models;

namespace API.Repositories
{
    public class ClimateRepository
    {
        private readonly ApplicationDbContext _ctx;

        public ClimateRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(ClimateRecord record)
        {
            _ctx.ClimateRecords.Add(record);
            await _ctx.SaveChangesAsync();
        }

        public Task<List<ClimateRecord>> GetAllAsync()
            => _ctx.ClimateRecords
                   .OrderByDescending(c => c.CapturedAtUtc)
                   .Take(200)
                   .ToListAsync();
    }
}
