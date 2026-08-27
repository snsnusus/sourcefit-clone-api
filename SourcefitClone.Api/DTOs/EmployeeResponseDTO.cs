namespace SourcefitClone.Api.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string MaritalStatus { get; set; } = string.Empty;
    public string? OfficeLocation { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? DepartmentName { get; set; } // flattened from the related Department, not a raw FK
}