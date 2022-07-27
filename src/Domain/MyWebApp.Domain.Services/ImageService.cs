using MyWebApp.Data.Entities;
using MyWebApp.Domain.Exceptions;
using MyWebApp.Domain.Repositories;

namespace MyWebApp.Domain.Services;

public sealed class ImageService
{
    private readonly IRepository<Image> _repo;

    public ImageService(IRepository<Image> repo) => _repo = repo;

    public ImageService(string connectionString) => _repo = new ImageRepository(connectionString);

    /// <summary>
    /// создает и сохраняет фото 
    /// </summary>
    /// <param name="image">фото</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">если image является null</exception>
    public async ValueTask<Image> CreateAndSaveAsync(Image image)
    {
        if (image is null) throw new ArgumentNullException(nameof(image));

        var createdImage = await _repo.CreateAndSaveAsync(image);

        return createdImage;
    }

    /// <summary>
    /// возвращает изображение по id
    /// </summary>
    /// <param name="id">id изображения</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">если изображение не найдено по id</exception>
    public async ValueTask<Image> GetByIdAsync(ulong id)
    {
        var image = await _repo.GetByIdAsync(id);

        if (image is null) throw new EntityNotFoundException();

        return image;
    }

    /// <summary>
    /// Возвращает все изображения
    /// </summary>
    /// <returns></returns>
    public async ValueTask<IList<Image>> GetAllAsync() => await _repo.GetAllAsync();



}
