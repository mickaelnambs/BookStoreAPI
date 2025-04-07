namespace Core.Interfaces;

public interface IBookImportService
{
    Task ImportBooksFromJsonAsync(string filePath);
}
