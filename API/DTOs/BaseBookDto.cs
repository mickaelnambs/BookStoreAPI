using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class BaseBookDto
{
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Author must be between 2 and 100 characters")]
    public string Author { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^(?:\d{10}|\d{13})$", ErrorMessage = "ISBN must be 10 or 13 digits")]
    public string Isbn { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 2000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 10000, ErrorMessage = "Price must be greater than 0 and less than 10000")]
    public decimal Price { get; set; }

    [Required]
    public string CoverImageUrl { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Genre must be between 2 and 50 characters")]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Publisher must be between 2 and 100 characters")]
    public string Publisher { get; set; } = string.Empty;

    [Required]
    [Range(0, 10000, ErrorMessage = "Quantity in stock must be between 0 and 10000")]
    public int QuantityInStock { get; set; }
}
