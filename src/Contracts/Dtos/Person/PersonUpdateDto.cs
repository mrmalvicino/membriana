using Contracts.Interfaces;

namespace Contracts.Dtos.Person;

public class PersonUpdateDto : IIdentifiable
{
    public int Id { get; set; }
    public bool Active { get; set; } = true;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Dni { get; set; }
    public DateTime BirthDate { get; set; }
    public int? ProfileImageId { get; set; }
}
