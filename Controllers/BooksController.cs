using LibraryManager.Communications.Requests;
using LibraryManager.Entities;
using LibraryManager.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly List<Book> _books;
    private int id = 1;

    public BooksController(List<Book> books)
    {
        _books = books;
    }


    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] RequestRegisterBookJson request)
    {
        var canRegister = true;

        foreach (var book in _books)
        {
            if (book.Title == request.Title)
            {
                canRegister = false;
                break;
            }
        }

        if (!canRegister)
            return BadRequest("There is already a book with this title");

        if (canRegister)
        {
            _books.Add(new Book
            {
                Id = id,
                Title = request.Title,
                Amount = request.Amount,
                Author = request.Author,
                Genre = Converter(request.Genre),
                Price = request.Price,
            });

            id = _books[^1].Id + 1;
        }

        return Created(string.Empty, _books[id - 2]);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
    public IActionResult GetAll()
    {
        if (_books.Count == 0)
            return NoContent();

        return Ok(_books);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult UpdateById([FromRoute] int id, [FromBody] RequestUpdateBookJson request)
    {
        var updated = false;

        foreach (var book in _books)
        {
            if (book.Id == id)
            {
                book.Author = request.Author;
                book.Genre = Converter(request.Genre);
                book.Price = request.Price;
                book.Amount = request.Amount;
                book.Title = request.Title;

                updated = true;
            }
        }

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult Delete([FromRoute] int id)
    {
        var deleted = false;

        foreach (var book in _books)
        {
            if (book.Id == id)
            {
                _books.Remove(book);
                deleted = true;
            }
        }

        if (!deleted)
            return NotFound();

        return NoContent();
    }

    private static Genre Converter(string genre)
    {
        var map = new Dictionary<string, Genre>
        {
            { "fiction", Genre.Fiction },
            { "ficcão", Genre.Fiction },
            { "romance", Genre.Romance },
            { "mistery", Genre.Mistery },
            { "mistério", Genre.Mistery }
        };

        if (!map.ContainsKey(genre.Trim().ToLower()))
            return Genre.Unknown;

        return map[genre.Trim().ToLower()];
    }
}
