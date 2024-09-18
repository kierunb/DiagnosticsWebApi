using DiagnosticsWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DiagnosticsWebApi.Controllers
{
    [Route("user-profile")]
    [ApiController]
    public class UserProfileController(
        ILogger<UserProfileController> logger,
        UserProfileDataGeneratorService userProfileDataGenerator,
        IUserProfileCache userProfileCache) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var userProfile = userProfileDataGenerator.GenerateUserProfile();
            userProfileCache.GetOrAdd(Guid.NewGuid(), _ => userProfile);

            return Ok(userProfile);
        }
    }
}
