using Microsoft.AspNetCore.Mvc;
using ConTech.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConTech.Web.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet()
    {
        // Redirect to another page in the Pages folder
        return RedirectToPage("Project/Create");
    }
}