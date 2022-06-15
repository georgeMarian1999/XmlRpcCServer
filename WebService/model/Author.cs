namespace WebService.model;

public class Author
{
    private int authorId;
    private String name;
    private int age;

    public Author(int authorId, string name, int age)
    {
        this.authorId = authorId;
        this.name = name;
        this.age = age;
    }

    public Author()
    {
    }

    public int AuthorId
    {
        get => authorId;
        set => authorId = value;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Age
    {
        get => age;
        set => age = value;
    }
}