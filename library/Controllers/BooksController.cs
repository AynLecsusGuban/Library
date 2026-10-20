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

    // GET /Books/Create : an empty form.
    public IActionResult Create()
    {
        return View(new Book());
    }

    // POST /Books/Create : the form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Book book)
    {
        if (!ModelState.IsValid)                        // a required field was left empty
        {
            return View(book);
        }

        await _books.AddAsync(book);
        return RedirectToAction(nameof(Index));
    }

    // GET /Books/Edit/5 : the form, filled with book 5.
    public async Task<IActionResult> Edit(long id)
    {
        var book = await _books.GetByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return View(book);
    }

    // POST /Books/Edit/5 : the changed form was submitted.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Book book)
    {
        if (!ModelState.IsValid)
        {
            return View(book);
        }

        await _books.UpdateAsync(book);
        return RedirectToAction(nameof(Index));
    }


}
