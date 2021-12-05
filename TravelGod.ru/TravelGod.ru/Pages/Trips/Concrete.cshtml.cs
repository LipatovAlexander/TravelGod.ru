using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.Models;
using TravelGod.ru.Services;

namespace TravelGod.ru.Pages.Trips
{
    public class Concrete : MyPageModel
    {
        private readonly TripService _tripService;
        private readonly CommentService _commentService;
        private readonly ChatService _chatService;
        private readonly RatingService _ratingService;

        public Concrete(TripService tripService, CommentService commentService, ChatService chatService, RatingService ratingService)
        {
            _tripService = tripService;
            _commentService = commentService;
            _chatService = chatService;
            _ratingService = ratingService;
        }

        public Trip Trip { get; set; }

        [FromForm]
        public Comment NewComment { get; set; }
        [BindProperty]
        public Rating NewRating { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
            if (Trip is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetJoin(int id)
        {
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
            if (User is null || Trip is null || Trip.Users.Contains(User))
            {
                return BadRequest();
            }

            await _tripService.AddUserToTrip(Trip, User);
            return RedirectToPage("/Trips/Concrete", new {Id = id});
        }

        public async Task<IActionResult> OnGetCreateChat(int id)
        {
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
            if (User is null || Trip is null || Trip.InitiatorId != User.Id || Trip.Chat is not null)
            {
                return BadRequest();
            }

            await _chatService.CreateChatForTripAsync(Trip);
            return RedirectToPage("/Trips/Concrete", new {Id = id});
        }

        public async Task<IActionResult> OnPostAddComment(int id)
        {
            ModelState.Clear();
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
            if (User is null || Trip is null || NewComment is null)
            {
                return BadRequest();
            }

            if (!TryValidateModel(NewComment, nameof(NewComment)))
            {
                return Page();
            }

            NewComment.Trip = Trip;
            NewComment.User = User;
            NewComment.Date = DateTime.Now;
            await _commentService.AddCommentAsync(NewComment);
            return ViewComponent("Comment", new {Comment = NewComment});
        }

        public async Task<IActionResult> OnPostAddRating(int id)
        {
            ModelState.Clear();
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
            if (User is null || Trip is null || !Trip.Users.Exists(u => u.Id == User.Id) || Trip.Ratings.Exists(r => r.User.Id == User.Id) || NewRating is null)
            {
                return BadRequest();
            }
            if (!TryValidateModel(NewRating, nameof(NewRating)))
            {
                return Page();
            }

            NewRating.Trip = Trip;
            NewRating.User = User;
            NewRating.Date = DateTime.Now;
            await _ratingService.AddRatingAsync(NewRating);
            return RedirectToPage("/Trips/Concrete", new {id = Trip.Id});
        }
    }
}