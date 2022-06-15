using System.Net;
using System.Text.Json;
using Nwc.XmlRpc;
using WebService.authorRepo;
using WebService.model;
using WebService.repos;

namespace WebService.service;

public class Server
{
    public const int port = 5050;

    public const String path = "books";

    private AuthorRepo _authorRepo;
    private BookRepo _bookRepo;
    
    public String ping() {
        Console.WriteLine("Method ping requested from proxy");
        String host = Dns.GetHostName();
        return "C# XML-RPC "+path+" "+host+" ("+Dns.GetHostAddresses(host)[0]+":"+port+") "+DateTime.Now;
    }

    public Server()
    {
        XmlRpcServer xmlRpcServer = new XmlRpcServer(port);
        _bookRepo = new BookRepo();
        _authorRepo = new AuthorRepo();
        xmlRpcServer.Add(path, this);
        Console.WriteLine("C# XML-RPC service "+path+" waiting at: "+"http://localhost:" + port + "/" + path);
        xmlRpcServer.Start();
    }

    public String getAllAuthors()
    {
        try
        {
            Console.WriteLine("Method get all authors requested from proxy");
            String jsonString = JsonSerializer.Serialize(_authorRepo.getAllAuthors().Result);
            return jsonString;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        return "";
    }

    public string getAuthorById(int id)
    {
        Console.WriteLine("Method get author by id requested from proxy" + id);
        try
        {
            Console.WriteLine(_authorRepo.getAuthorById(id).Result.Age);
            return JsonSerializer.Serialize(_authorRepo.getAuthorById(id).Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return null;
        }
        
    }

    public string getAuthorsBySearch(String keyword)
    {
        Console.WriteLine("Method getAuthorsBySearch requested from proxy with keyword " + keyword);
        try
        {
            Console.WriteLine(_authorRepo.getAuthorsBySearch(keyword).Result.Count);
            return JsonSerializer.Serialize(_authorRepo.getAuthorsBySearch(keyword).Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return null;
        }
        
        
    }

    public void addAuthor(String name, int age)
    {
        _authorRepo.addAuthor(name, age);
    }

    public void deleteAuthor(int id)
    {
        _authorRepo.deleteAuthor(id);
    }

    public void editAuthor(int id, String name, int age)
    {
        _authorRepo.editAuthor(id, name, age);
    }
    
    public string getAllBooks()
    {
        try
        {
            Console.WriteLine("Method get all books requested from proxy ");
            string jsonString = JsonSerializer.Serialize(_bookRepo.getAllBooks());
            return jsonString;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        return "";
    }

    public string getBookById(int id)
    {
        Console.WriteLine("Method get book by id requested from proxy" + id);
        try
        {
            Console.WriteLine(_bookRepo.getBookById(id).Result);
            return JsonSerializer.Serialize(_bookRepo.getBookById(id).Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return null;
        }
        
    }

    public string getBooksBySearch(String keyword)
    {
        Console.WriteLine("Method getBooksBySearch requested from proxy with keyword " + keyword);
        try
        {
            Console.WriteLine(_bookRepo.getBooksBySearch(keyword).Result.Count);
            return JsonSerializer.Serialize(_bookRepo.getBooksBySearch(keyword).Result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            return null;
        }
        
        
    }

    public void addBook(String title,String description, int year, int authorId)
    {
        _bookRepo.addBook(title,description,year,authorId);
    }

    public void deleteBook(int id)
    {
        _bookRepo.deleteBook(id);
    }

    public void editBook(int id, String title,String description, int year, int authorId)
    {
        _bookRepo.editBook(id, title,description,year,authorId);
    }
    
    
}