using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Common.Primitives;

namespace SpendingTracker.WebApp.Controllers;

public class BaseController : Controller
{
    protected UserKey GetCurrentUserId()
    {
        var userIdAsString = User.Claims.First(c => c.Type == "Id").Value;
        var result = UserKey.Parse(userIdAsString);

        return result;
    }
}