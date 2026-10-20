
using library.Data;
using library.Models;
using Microsoft.AspNetCore.Mvc;

namespace library.Controllers;

public class NotesController : Controller
{
    private readonly NoteRepository _notes;

    public NotesController(NoteRepository notes)
    {
        _notes = notes;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _notes.GetAllAsync());
    }

    public IActionResult Create()
    {
        return View(new Note { CreatedOn = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Note note)
    {
        if (!ModelState.IsValid)
            return View(note);

        await _notes.AddAsync(note);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(long id)
    {
        var note = await _notes.GetByIdAsync(id);

        if (note is null)
            return NotFound();

        return View(note);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(long id, Note note)
    {
        if (id != note.NoteId)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(note);

        if (await _notes.GetByIdAsync(id) is null)
            return NotFound();

        await _notes.UpdateAsync(note);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(long id)
    {
        var note = await _notes.GetByIdAsync(id);

        if (note is null)
            return NotFound();

        return View(note);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(long id)
    {
        if (await _notes.GetByIdAsync(id) is null)
            return NotFound();

        await _notes.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
