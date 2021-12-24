using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;
using TravelGod.ru.Services.Filters;

namespace TravelGod.ru.Pages.Trips
{
    [AuthenticationPageFilter]
    public class Add : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Add(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty] public Trip Trip { get; set; }
        [BindProperty] public bool CreateChat { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _unitOfWork.Trips.CreateAsync(Trip, User, CreateChat);
                await _unitOfWork.SaveAsync();
            }
            catch
            {
                return BadRequest();
            }

            return RedirectToPage("/Trips/Concrete", new {id = Trip.Id});
        }
    }
}