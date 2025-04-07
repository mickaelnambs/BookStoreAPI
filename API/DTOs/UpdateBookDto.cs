using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class UpdateBookDto : BaseBookDto
{
    [Required]
    public int Id { get; set; }
}
