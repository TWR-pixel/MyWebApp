namespace MyWebApp.Services;

public class FileService
{
    public void DeleteFile(string path) => File.Delete(path);
}