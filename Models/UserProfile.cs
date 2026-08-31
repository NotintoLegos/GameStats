using System.Text.Json.Serialization;

public class UserProfile
{
    public string UniqueID { get; }= Guid.NewGuid().ToString();
    public string Email { get; set; }= "";
    public string UserName { get; set; }= "";


}