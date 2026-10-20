using library.Models;
using Npgsql;

namespace library.Data;

public class MemberRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public MemberRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    private static Member ReadMember(NpgsqlDataReader reader)
    {
        return new Member
        {
            member_id = reader.GetInt64(0),
            full_name = reader.GetString(1),
            email = reader.IsDBNull(2) ? null : reader.GetString(2),   // NULL becomes null
            member_type = reader.GetString(3)
        };
    }

    const string sql = "SELECT member_id, full_name, email, member_type FROM lending.member ORDER BY full_name;";
}