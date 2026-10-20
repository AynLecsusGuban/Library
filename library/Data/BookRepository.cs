using library.Models;
using Npgsql;

namespace library.Data;

// Every SQL statement about books lives in this one class.
public class BookRepository
{
    private readonly NpgsqlDataSource _dataSource;

    // ASP.NET Core hands in the data source that Program.cs registered.
    public BookRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    // Copies the current row of the reader into a Book.
    // The positions 0 to 3 match the column order in both SELECTs above.
    private static Book ReadBook(NpgsqlDataReader reader)
    {
        return new Book
        {
            BookId = reader.GetInt64(0),
            Title = reader.GetString(1),
            Category = reader.IsDBNull(2) ? null : reader.GetString(2),   // NULL becomes null
            Price = reader.IsDBNull(3) ? null : reader.GetDecimal(3)
        };
    }

    // READ: every book, sorted by title.
    public async Task<List<Book>> GetAllAsync()

    {
        const string sql = "SELECT book_id, title, category, price FROM lending.book ORDER BY title;";

        var books = new List<Book>();

        // The command borrows a connection; 'await using' gives it back when the method ends.
        await using var command = _dataSource.CreateCommand(sql);
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())        // once for each row
        {
            books.Add(ReadBook(reader));
        }

        return books;
    }

    // CREATE: insert a new book. The database chooses its book_id.
    public async Task AddAsync(Book book)
    {
        const string sql = "INSERT INTO lending.book (title, category, price) " +
                           "VALUES (@title, @category, @price);";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("title", book.Title);
        // A C# null must be sent as DBNull.Value, which is how a database NULL is written.
        command.Parameters.AddWithValue("category", (object?)book.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("price", (object?)book.Price ?? DBNull.Value);

        await command.ExecuteNonQueryAsync();           // runs SQL that returns no rows
    }

    // READ: one book, or null when no book has that id.
    public async Task<Book?> GetByIdAsync(long id)
    {
        const string sql = "SELECT book_id, title, category, price FROM lending.book WHERE book_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("id", id);      // @id in the SQL receives this value

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadBook(reader) : null;
    }

    // UPDATE: change every column of one book.
    public async Task UpdateAsync(Book book)
    {
        const string sql = "UPDATE lending.book " +
                           "SET title = @title, category = @category, price = @price " +
                           "WHERE book_id = @id;";

        await using var command = _dataSource.CreateCommand(sql);
        command.Parameters.AddWithValue("title", book.Title);
        command.Parameters.AddWithValue("category", (object?)book.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("price", (object?)book.Price ?? DBNull.Value);
        command.Parameters.AddWithValue("id", book.BookId);

        await command.ExecuteNonQueryAsync();
    }

}
