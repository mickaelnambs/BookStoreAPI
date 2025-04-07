namespace Core.Specifications;

public class BookSpecParams : PagingParams
{
    private List<string> _genres = [];
    public List<string> Genres
    {
        get => _genres;
        set
        {
            _genres = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    private List<string> _publishers = [];
    public List<string> Publishers
    {
        get => _publishers;
        set
        {
            _publishers = value.SelectMany(x => x.Split(',',
                StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value.ToLower();
    }
}
