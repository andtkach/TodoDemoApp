using System.Security.Claims;

namespace Tasks.Api.Services
{
    public class LoggedInUserService: ILoggedInUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public LoggedInUserService(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                var userId = _contextAccessor.HttpContext?.User?.FindFirstValue("userId");
                return userId ?? string.Empty;
            }
        }
    }
}
