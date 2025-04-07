using System.Security.Claims;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class WishlistsController(IWishlistService wishlistService) : BaseApiController
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<Book>>> GetWishlist()
    {
        var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized();

        var books = await wishlistService.GetWishlistBooksAsync(buyerEmail);

        return Ok(books);
    }

    [HttpPost("{bookId:int}")]
    public async Task<ActionResult> AddToWishlist(int bookId)
    {
        var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(buyerEmail))
        {
            return Ok(new { message = "You must be logged in to add books to your wishlist" });
        }

        var isAlreadyInWishlist = await wishlistService.IsBookInWishlistAsync(buyerEmail, bookId);

        if (isAlreadyInWishlist) return Ok(new { message = "This book is already in your wishlist" });

        var result = await wishlistService.AddToWishlistAsync(buyerEmail, bookId);

        if (!result) return BadRequest("Problem adding book to wishlist");

        return Ok(new { message = "Book added to wishlist successfully" });
    }

    [Authorize]
    [HttpDelete("{bookId}")]
    public async Task<ActionResult> RemoveFromWishlist(int bookId)
    {
        var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized();

        var result = await wishlistService.RemoveFromWishlistAsync(buyerEmail, bookId);

        if (!result) return BadRequest("Problem removing book from wishlist");

        return Ok(new { message = "Book removed from wishlist successfully" });
    }

    [HttpGet("contains/{bookId}")]
    public async Task<ActionResult<bool>> CheckBookInWishlist(int bookId)
    {
        var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized();

        var isInWishlist = await wishlistService.IsBookInWishlistAsync(buyerEmail, bookId);

        return Ok(isInWishlist);
    }
}
