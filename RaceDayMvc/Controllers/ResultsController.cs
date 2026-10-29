
using Microsoft.AspNetCore.Mvc;
using RaceDayMvc.Models;
using RaceDayMvc.Services;

namespace RaceDayMVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApiService _api;

        public ResultsController(ApiService api)
        {
            _api = api;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var results = await _api.GetAsync<ParticipantResult>(
                    "api/Results/my-results");

                return View(results);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage =
                    "API request failed: " + ex.Message;

                return View(new List<ParticipantResult>());
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage =
                    "Error loading results: " + ex.Message;

                return View(new List<ParticipantResult>());
            }
        }

    }
}


