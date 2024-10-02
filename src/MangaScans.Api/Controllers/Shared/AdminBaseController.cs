using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Shared;

[Authorize(Roles = "admin")]
public class AdminBaseController : ControllerBase
{
    
}