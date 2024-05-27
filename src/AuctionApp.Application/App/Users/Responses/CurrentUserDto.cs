namespace Application.App.Users.Responses;
public class CurrentUserDto
{
    public int Id { get; set; }

    public required string UserName { get; set; }

    public decimal Balance { get; set; }
}
