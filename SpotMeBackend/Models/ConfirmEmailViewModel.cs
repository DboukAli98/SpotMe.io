using System.ComponentModel.DataAnnotations;

namespace SpotMeBackend.Models;

public class ConfirmEmailViewModel
{
    [Required]
    public string? Token { get; set; }
    [Required]
    public string? UserId { get; set; }
}