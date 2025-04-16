using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class ReceiptService : IReceiptRepository
    {
        private readonly ApplicationDbContext _context;
        public ReceiptService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Receipt>> GetAllAsync()
        {
            return await _context.Receipts.Include(p => p.GetUser).ToListAsync();
        }
        public async Task<Receipt> GetByIdAsync(int id)
        {
            return await _context.Receipts.Include(p => p.GetUser).FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddAsync(Receipt receipt)
        {
            _context.Receipts.Add(receipt);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Receipt receipt)
        {
            _context.Receipts.Update(receipt);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var receipt = await GetByIdAsync(id);
            if (receipt != null)
            {
                _context.Receipts.Remove(receipt);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Receipt>> GetByUserIdAsync(string userId)
        {
            return await _context.Receipts.Include(p => p.GetUser).Include(p=>p.ReceiptDetails).Where(p => p.UserId == userId).ToListAsync();
        }
    }
}
