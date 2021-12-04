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

        public Concrete(TripService tripService, CommentService commentService, ChatService chatService)
        {
            _tripService = tripService;
            _commentService = commentService;
            _chatService = chatService;
        }

        public Trip Trip { get; set; }

        [FromForm]
        public Comment NewComment { get; set; }

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
            Trip = await _tripService.GetTripAsync(id, Status.Normal);
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