using Core.Entities;

namespace Core.Specifications;

public class GenreListSpecification : BaseSpecification<Book, string>
{
    public GenreListSpecification()
    {
        AddSelect(x => x.Genre);
        ApplyDistinct();
    }
}
