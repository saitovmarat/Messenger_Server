namespace MessangerBack.Schemas;

public class RegisterUserSchema
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RepeatPassword { get; set; }
}