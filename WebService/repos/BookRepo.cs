using Npgsql;
using WebService.model;

namespace WebService.repos;

public class BookRepo
{
    private JDBC.JDBC connectionUtils;
    private NpgsqlConnection _connection;

    public BookRepo()
    {
        connectionUtils = new JDBC.JDBC();
        _connection = connectionUtils.getConnection();
    }
    
    public async Task<int> getNextId()
    {
        int id = 0;
        String sql = "SELECT MAX(A.\"bookId\") as maxId FROM \"Book\" A ";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    id = reader.GetInt32(0) + 1;
                    return id;
                }
        }
        
        return id ;
    }

    public List<Book> getAllBooks()
    {
        List<Book> books = new List<Book>();
        
        string sql = "SELECT * from \"Book\"";
        using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
             using (NpgsqlDataReader reader =  cmd.ExecuteReader())
                while ( reader.Read())
                {
                    Book book = new Book(reader.GetInt32(0),reader.GetString(1),reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                    books.Add(book);
                    
                }
        }

        return books;
    }

    public async Task<Book> getBookById(int id)
    {
        Book book = new Book();
        
        string sql = "select * from \"Book\" where \"bookId\" = @bookId";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("bookId", id);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    book.BookId = reader.GetInt32(0);
                    book.Title = reader.GetString(1);
                    book.Description = reader.GetString(2);
                    book.Year = reader.GetInt32(3);
                    book.AuthorId = reader.GetInt32(4);
                }
        }

        Console.WriteLine("Got book:" + book.Title);
        
        return book;
    }

    public async Task<List<Book>> getBooksBySearch(String keyword)
    {
        List<Book> books = new List<Book>();
        string sql = "select * from \"Book\" where title like @keyword";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("keyword", "%" + keyword + "%" );
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Book book = new Book(reader.GetInt32(0),reader.GetString(1),reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4));
                    books.Add(book);
                }
        }

        return books;
    }

    public async void addBook(String title,String description, int year, int authorId)
    {
        int id = await getNextId();
        string sql = "INSERT INTO \"Book\" VALUES (@bookId,@title,@description,@year,@authorId)";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("bookId", id);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("year", year);
            cmd.Parameters.AddWithValue("authorId", authorId);

            await cmd.ExecuteNonQueryAsync();
        }
        Console.WriteLine("Book added with title " + title);
    }

    public async void deleteBook(int id)
    {
        string sql = "DELETE FROM \"Book\" where \"bookId\" = @bookId";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("bookId", id);
            await cmd.ExecuteNonQueryAsync();
        }
        
    }

    public async void editBook(int id, String title,String description, int year, int authorId)
    {
        string sql = "UPDATE \"Book\" set \"title\" = @title, \"description\" = @description, \"year\" = @year, \"authorId\" = @authorId where \"bookId\" = @bookId";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("bookId", id);
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("year", year);
            cmd.Parameters.AddWithValue("authorId", authorId);
            
            await cmd.ExecuteNonQueryAsync();
        }
    }
}