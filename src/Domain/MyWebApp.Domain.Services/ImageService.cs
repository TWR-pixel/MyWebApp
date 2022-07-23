using MyWebApp.Data.Entities;
using MyWebApp.Domain.Exceptions;
using MyWebApp.Domain.Repositories;

namespace MyWebApp.Domain.Services;

public sealed class ImageService
{
    private readonly IRepository<Image> _repo;

    public ImageService(IRepository<Image> repo) => _repo = repo;

    public ImageService(string connectionString) => _repo = new ImageRepository(connectionString);

    public async Task<Image> CreateAsync(Image image)
    {
        if (image is null) throw new ArgumentNullException(nameof(image));

        var createdImage = await _repo.CreateAsync(image);

        return createdImage;
    }

    public async Task<Image> GetByIdAsync(ulong id)
    {
        var image = await _repo.GetByIdAsync(id);

        if (image is null) throw new EntityNotFoundException();

        return image;
    }

    public async Task<IList<Image>> GetAllAsync() => await _repo.GetAllAsync();



}
