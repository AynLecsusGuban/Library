using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class BooksController : Controller
{
    private readonly BookRepository _books;

    // ASP.NET Core hands in the repository, because Program.cs registered it.
    public BooksController(BookRepository books)
    {
        _books = books;
    }

    // GET /Books : the list of books.
    public async Task<IActionResult> Index()
    {
        var books = await _books.GetAllAsync();
        return View(books);                             // Views/Books/Index.cshtml
    }

}
