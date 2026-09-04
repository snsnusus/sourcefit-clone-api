using System.ComponentModel.DataAnnotations;

namespace SourcefitClone.Api.DTOs;

public class RefreshRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}