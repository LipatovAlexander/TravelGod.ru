using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages
{
    public class TripPage : MyPageModel
    {
        private readonly TripService _tripService;
        private readonly FileService _fileService;
        private readonly CommentService _commentService;

        public TripPage(TripService tripService, FileService fileService, CommentService commentService)
        {
            _tripService = tripService;
            _fileService = fileService;
            _commentService = commentService;
        }

        public Trip Trip { get; set; }

        [FromForm]
        public Comment NewComment { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Trip = await _tripService.GetTripAsync(id);
            if (Trip is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetJoin(int id)
        {
            Trip = await _tripService.GetTripAsync(id);
            if (User is null || Trip is null || Trip.Users.Contains(User))
            {
                return BadRequest();
            }

            await _tripService.AddUserToTrip(Trip, User);
            return Page();
        }

        public async Task<IActionResult> OnPostAddComment(int id)
        {
            Trip = await _tripService.GetTripAsync(id);
            if (User is null || Trip is null || NewComment is null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            NewComment.Trip = Trip;
            NewComment.User = User;
            NewComment.Date = DateTime.Now;
            await _commentService.AddCommentAsync(NewComment);
            return ViewComponent("Comment", new {Comment = NewComment});
        }
    }
}