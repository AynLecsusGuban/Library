
using System.ComponentModel.DataAnnotations;

namespace library.Models;

public class Note
{
    public long NoteId { get; set; }

    [Required(ErrorMessage = "Please enter a title.")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    public string? Body { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime CreatedOn { get; set; } = DateTime.Today;
}
