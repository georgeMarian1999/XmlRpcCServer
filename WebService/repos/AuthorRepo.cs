using Npgsql;
using WebService.model;

namespace WebService.authorRepo;

public class AuthorRepo
{
    private JDBC.JDBC connectionUtils;
    private NpgsqlConnection _connection;

    public AuthorRepo()
    {
        connectionUtils = new JDBC.JDBC();
        _connection = connectionUtils.getConnection();
    }
    
    public async Task<int> getNextId()
    {
        int id = 0;
        String sql = "SELECT MAX(A.\"authorId\") as maxId FROM \"Author\" A ";
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

    public async Task<List<Author>> getAllAuthors()
    {
        List<Author> authors = new List<Author>();
        
        string sql = "SELECT * from \"Author\"";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Author author = new Author(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2));
                    authors.Add(author);
                }
        }

        return authors;
    }

    public async Task<Author> getAuthorById(int id)
    {
        Author author = new Author();
        
        string sql = "select * from \"Author\" where \"authorId\" = @authorId";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("authorId", id);

            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    author.AuthorId = reader.GetInt32(0);
                    author.Name = reader.GetString(1);
                    author.Age = reader.GetInt32(2);
                }
        }

        Console.WriteLine("Got author:" + author.Name);
        
        return author;
    }

    public async Task<List<Author>> getAuthorsBySearch(String keyword)
    {
        List<Author> authors = new List<Author>();
        string sql = "select * from \"Author\" where name like @keyword";
        await using (NpgsqlCommand cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("keyword", "%" + keyword + "%" );
            await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Author author = new Author(reader.GetInt32(0),reader.GetString(1),reader.GetInt32(2));
                    authors.Add(author);
                }
        }

        return authors;
    }

    public async void addAuthor(String name, int age)
    {
        int id = await getNextId();
        string sql = "INSERT INTO \"Author\" VALUES (@authorId,@name,@age)";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("authorId", id);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("age", age);

            await cmd.ExecuteNonQueryAsync();
        }
        Console.WriteLine("Author added with name " + name);
    }

    public async void deleteAuthor(int id)
    {
        string sql = "DELETE FROM \"Author\" where \"authorId\" = @authorId";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("authorId", id);
            await cmd.ExecuteNonQueryAsync();
        }
        
    }

    public async void editAuthor(int id, String name, int age)
    {
        string sql = "UPDATE \"Author\" set \"name\" = @name, \"age\" = @age where \"authorId\" = @authorId";
        await using (var cmd = new NpgsqlCommand(sql, _connection))
        {
            cmd.Parameters.AddWithValue("authorId", id);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("age", age);
            
            await cmd.ExecuteNonQueryAsync();
        }
    }
}