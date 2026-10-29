
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;

namespace RaceDayMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _api;

        public EventsController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var eventsList = await _api.GetAsync<Event>("api/Events");

            var categoriesByEvent = new Dictionary<int, List<Category>>();

            foreach (var item in eventsList)
            {
                var categories = await _api.GetAsync<Category>(
                    "api/Events/" + item.EventID + "/categories");

                categoriesByEvent[item.EventID] = categories;
            }

            ViewBag.CategoriesByEvent = categoriesByEvent;

            return View(eventsList);
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            var response = await _api.PostAsync(
                "api/Events/" + eventId + "/enrolments",
                new { categoryID = categoryId });

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] =
                    "You have successfully enrolled in the event.";
            }
            else
            {
                var errorMessage =
                    await response.Content.ReadAsStringAsync();

                TempData["ErrorMessage"] =
                    "Enrolment failed. API response: " + errorMessage;
            }

            return RedirectToAction("Index");
        }
    }
}
