

using ConTech.Web.Pages.View;

namespace ConTech.Web;

public static class MappingEndpoints
{
    public static void Map(IEndpointRouteBuilder app)
    {
        ViewEndpoints.Map(app);
    }
}
