using System.ComponentModel.DataAnnotations;

namespace SourcefitClone.Api.DTOs;

public class LogoutRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}