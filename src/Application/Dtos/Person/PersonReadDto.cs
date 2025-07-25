using Application.Dtos.Image;
using Domain.Interfaces;

namespace Application.Dtos.Person
{
    public class PersonReadDto : IIdentifiable
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Dni { get; set; }
        public DateTime BirthDate { get; set; }
        public ImageReadDto? ProfileImage { get; set; }
    }
}
