using Core.Entities;

namespace Core.Specifications;

public class PublisherListSpecification : BaseSpecification<Book, string>
{
    public PublisherListSpecification()
    {
        AddSelect(x => x.Publisher);
        ApplyDistinct();
    }
}
