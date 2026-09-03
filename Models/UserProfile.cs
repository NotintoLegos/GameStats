using System.Text.Json.Serialization;

public class UserProfile
{
    public string UniqueID { get; set; } = "";
    public string Email { get; set; } = "";
    public string UserName { get; set; } = "";

    [JsonIgnore]
    public string PasswordHash { get; set; } = "";


    //need obj of stats

    //need obj of 


}