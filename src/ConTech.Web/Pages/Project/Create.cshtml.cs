using ConTech.Core;
using ConTech.Core.Features.Identity;
using ConTech.Core.Features.Project;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace ConTech.Web.Pages.Project;

public class CreateModel : PageModel
{
    private readonly IProjectRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public CreateModel(IProjectRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    [BindProperty]
    public ProjectNewInput Project { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var currentUser = new IdentityInfo(HttpContext);

        //if (Project != null)
            var result = await _repo.CreateProjectAsync(Project, currentUser);

        return RedirectToPage("./Index");
    }
}