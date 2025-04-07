using API.DTOs;
using API.Extensions;
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController(IUnitOfWork unit, IBookService bookService) : BaseApiController
{
    [Cache(600)]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Book>>> GetBooks([FromQuery] BookSpecParams specParams)
    {
        var spec = new BookSpecification(specParams);

        return await CreatePagedResult(unit.Repository<Book>(), spec,
            specParams.PageIndex, specParams.PageSize);
    }

    [Cache(600)]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await unit.Repository<Book>().GetByIdAsync(id);

        if (book == null) return NotFound();

        return book;
    }

    [InvalidateCache("api/books|")]
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook([FromForm] CreateBookDto bookDto, IFormFile coverImage)
    {
        var book = bookDto.ToEntity();

        var createdBook = await bookService.CreateBookAsync(book, coverImage);

        if (createdBook == null) return BadRequest("Problem creating book");

        return CreatedAtAction("GetBook", new { id = createdBook.Id }, createdBook);
    }

    [InvalidateCache("api/books|")]
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBook(int id, [FromForm] UpdateBookDto bookDto, IFormFile? coverImage)
    {
        var bookToUpdate = bookDto.ToEntity();

        var result = await bookService.UpdateBookAsync(id, bookToUpdate, coverImage);

        if (result) return NoContent();

        return NotFound();
    }

    [InvalidateCache("api/books|")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await bookService.DeleteBookAsync(id);

        if (result) return NoContent();

        return NotFound();
    }

    [Cache(10000)]
    [HttpGet("genres")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetGenres()
    {
        var spec = new GenreListSpecification();

        return Ok(await unit.Repository<Book>().ListAsync(spec));
    }

    [Cache(10000)]
    [HttpGet("publishers")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetPublishers()
    {
        var spec = new PublisherListSpecification();

        return Ok(await unit.Repository<Book>().ListAsync(spec));
    }
}
