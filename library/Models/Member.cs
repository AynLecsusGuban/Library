
namespace library.Models;

public class Member
{
    public long MemberId { get; set; }
    public string FullName { get; set; } = "";
    public string? Email { get; set; }
    public string MemberType { get; set; } = "";
}
