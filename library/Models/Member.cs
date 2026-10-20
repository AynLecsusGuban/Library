
namespace library.Models;

public class Member
{
    public long member_id { get; set; }
    public string full_name { get; set; } = "";
    public string? email { get; set; }
    public string member_type { get; set; } = "Regular";
}
