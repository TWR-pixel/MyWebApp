using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Data.Entities;

namespace MyWebApp.Domain.Repositories;

public class GroupRepository : IRepository<Group>
{
    private readonly NorthwindContext _context;

    public GroupRepository(string connectionString)
    {
        _context = new NorthwindContext(connectionString);
    }

    public async ValueTask<Group> CreateAsync(Group entity)
    {
        var group = await _context.Groups.AddAsync(entity);
        await _context.SaveChangesAsync();

        return group.Entity;
    }

    public async ValueTask DeleteByIdAsync(ulong id)
    {
        var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == id);

        _context.Groups.Remove(group);
        
        await _context.SaveChangesAsync();
    }

    public async ValueTask<IList<Group>> GetAllAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    public async ValueTask<Group?> GetByIdAsync(ulong id)
    {
        return await _context.Groups
            .AsNoTracking()
            .Include(g => g.Images)
            .FirstOrDefaultAsync(g => g.Id == id);
    }

    public async ValueTask<IList<Group>> Take(int count)
    {
        var groups = await _context.Groups
            .AsNoTracking()
            .Take(count)
            .ToListAsync();

        return groups;
    }
}