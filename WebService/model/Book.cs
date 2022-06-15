namespace WebService.model;

public class Book
{
    private int bookId;
    private String title;
    private String description;
    private int year;
    private int authorId;

    public Book(int bookId, string title, string description, int year, int authorId)
    {
        this.bookId = bookId;
        this.title = title;
        this.description = description;
        this.year = year;
        this.authorId = authorId;
    }

    public Book()
    {
        
    }

    public int BookId
    {
        get => bookId;
        set => bookId = value;
    }

    public string Title
    {
        get => title;
        set => title = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Description
    {
        get => description;
        set => description = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Year
    {
        get => year;
        set => year = value;
    }

    public int AuthorId
    {
        get => authorId;
        set => authorId = value;
    }
}