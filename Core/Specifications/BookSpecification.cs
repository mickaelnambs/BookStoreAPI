using Core.Entities;

namespace Core.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(BookSpecParams specParams) : base(x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Title.ToLower().Contains(specParams.Search)) &&
        (specParams.Genres.Count == 0 || specParams.Genres.Contains(x.Genre)) &&
        (specParams.Publishers.Count == 0 || specParams.Publishers.Contains(x.Publisher))
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderBy(x => x.Title);
                break;
        }
    }
}
