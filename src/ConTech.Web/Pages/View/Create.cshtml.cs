using ConTech.Core;
using ConTech.Core.Features.Identity;
using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace ConTech.Web.Pages.View;

public class CreateModel : PageModel
{
    private readonly IProjectViewRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public CreateModel(IProjectViewRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    [BindProperty]
    public ProjectViewNewInput View { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Check what values made it through binding
        var keys = ModelState.Keys.Where(k => ModelState[k].AttemptedValue != "");
        foreach (var key in keys)
        {
            Debug.WriteLine($"{key}: {ModelState[key].AttemptedValue}");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUser = new IdentityInfo(HttpContext);

        //if (Project != null)
            var result = await _repo.CreateProjectViewAsync(View, currentUser);

        return RedirectToPage("./Index");
    }
}