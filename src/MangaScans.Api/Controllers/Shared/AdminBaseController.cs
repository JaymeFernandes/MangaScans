using MangaScans.Identity.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScans.Api.Controllers.Shared;

[ApiController]
[Authorize(Roles = Roles.Administrator)]
public class AdminBaseController : ControllerBase
{
    
}