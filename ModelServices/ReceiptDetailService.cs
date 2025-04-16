using Microsoft.EntityFrameworkCore;
using school_major_project.DataAccess;
using school_major_project.Interfaces;
using school_major_project.Models;

namespace school_major_project.ModelServices
{
    public class ReceiptDetailService : IReceiptDetailsRepository
    {
        private readonly ApplicationDbContext _context;
        public ReceiptDetailService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ReceiptDetail>> GetAllAsync()
        {
            return await _context.ReceiptDetails.Include(p => p.Receipt).ToListAsync();
        }
        public async Task<ReceiptDetail> GetByIdAsync(int id)
        {
            return await _context.ReceiptDetails.Include(p => p.Receipt).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<ReceiptDetail>> FindByScheduleId(int scheduleId)
        {
            return await _context.ReceiptDetails.Include(p => p.Receipt).Where(p => p.ScheduleId == scheduleId).ToListAsync();
        }
        public async Task AddAsync(ReceiptDetail receiptDetail)
        {
            await _context.ReceiptDetails.AddAsync(receiptDetail);
            await _context.SaveChangesAsync();
        }
    }
}
