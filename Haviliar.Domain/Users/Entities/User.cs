using Haviliar.Domain.Users.Enums;

namespace Haviliar.Domain.Users.Entities;

public class User
{
    public int UserId { get; set; }
    public required string Email { get; set; }
    public required string Document { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required UserTypeEnum UserType { get; set; }
}
