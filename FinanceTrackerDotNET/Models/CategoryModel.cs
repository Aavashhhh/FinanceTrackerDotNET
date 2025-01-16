using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerDotNET.Models;

public class CategoryModel
{
    public Guid CategoryId { get; set; } = Guid.NewGuid();

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
    public string CategoryName { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    public string Type { get; set; }

    public Guid CreatedBy { get; set; }
}