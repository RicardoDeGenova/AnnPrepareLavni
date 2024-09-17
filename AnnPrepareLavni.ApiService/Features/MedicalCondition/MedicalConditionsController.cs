using Microsoft.AspNetCore.Mvc;

namespace AnnPrepareLavni.ApiService.Features.MedicalCondition;
public class MedicalConditionsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
