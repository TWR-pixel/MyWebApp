using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Data.Entities;

namespace MyWebApp.Domain.Repositories;

public class ImageRepository : IRepository<Image>
{
    private readonly NorthwindContext _context;

    public ImageRepository(string connectionString)
    {
        _context = new NorthwindContext(connectionString);
    }

    public async ValueTask<Image> CreateAsync(Image entity)
    {
        var image = await _context.Images.AddAsync(entity);

        await _context.SaveChangesAsync();

        return image.Entity;
    }

    public async ValueTask DeleteByIdAsync(ulong id)
    {
        var image = await _context.Images
            .FirstOrDefaultAsync(i => i.Id == id);

        _context.Images.Remove(image);

        await _context.SaveChangesAsync();
    }

    public async ValueTask<IList<Image>> GetAllAsync()
    {
        var images = await _context.Images
            .AsNoTracking()
            .ToListAsync();

        return images;
    }

    public async ValueTask<Image> GetByIdAsync(ulong id)
    {
        var image = await _context.Images
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        return image;
    }

    public async ValueTask<IList<Image>> Take(int count)
    {
        return await _context.Images
            .AsNoTracking()
            .Take(count)
            .ToListAsync();
    }
}