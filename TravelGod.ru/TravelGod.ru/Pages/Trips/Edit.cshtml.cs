using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelGod.ru.DAL.Interfaces;
using TravelGod.ru.Models;
using TravelGod.ru.Services.Filters;

namespace TravelGod.ru.Pages.Trips
{
    [AuthenticationPageFilter]
    public class Edit : MyPageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public Edit(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty] public Trip Trip { get; set; }
        [BindProperty] public bool CreateChat { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (id == 0)
            {
                return Page();
            }

            Trip = await _unitOfWork.Trips.FirstOrDefaultAsync(
                t => t.Id == id,
                trips => trips
                    .Include(t => t.Chat));
            if (Trip.CreatedById != User.Id)
            {
                return BadRequest();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
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

        public async Task<IActionResult> OnPostUpdate(int id)
        {
            Trip = await _unitOfWork.Trips.FirstOrDefaultAsync(t => t.Id == id,
                trips => trips
                    .Include(t => t.CreatedBy)
                    .Include(t => t.Chat));

            ModelState.Clear();
            if (Trip is null
                || Trip.EndDate < DateTime.Now
                || !await TryUpdateModelAsync(Trip, nameof(Trip))
                || !TryValidateModel(Trip, nameof(Trip)))
            {
                return BadRequest();
            }

            try
            {
                await _unitOfWork.Trips.UpdateAsync(Trip, CreateChat);
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