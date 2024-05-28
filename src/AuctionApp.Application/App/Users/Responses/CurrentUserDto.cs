namespace Application.App.Users.Responses;
public class CurrentUserDto
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public required string Email { get; set; }

    public required string PhoneNumber { get; set; }

    public decimal Balance { get; set; }
}
