using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using FootballManager.Contracts;
using FootballManager.ViewModels;

namespace FootballManager.Controllers
{
    public class UsersController : Controller
    {

        private readonly IUserService userService;
        public UsersController(Request request, IUserService _userService) 
            : base(request)
        { userService = _userService; }

        public Response Login()
        {
            if (User.IsAuthenticated)
            { return Redirect("/Players/All"); }
            return this.View(new { IsAuthenticated = false });
        }

        public Response Register()
        {
            if (User.IsAuthenticated)
            { return Redirect("/Players/All"); }
            return this.View(new { IsAuthenticated = false });
        }

        [HttpPost]
        public Response Login(LoginViewModel model)
        {
            Request.Session.Clear();

            (string userId, bool isCorrect) = userService.IsLoginCorrect(model);

            if (isCorrect)
            {
                SignIn(userId);

                CookieCollection cookies = new CookieCollection();
                cookies.Add(Session.SessionCookieName,
                    Request.Session.Id);

                return Redirect("/Players/All");
            }

            return View(new List<ErrorViewModel>() { new ErrorViewModel("Login incorrect") }, "/Users/Error");
        }

        [HttpPost]
        public Response Register(RegisterViewModel model)
        {
            var (isRegistered, errors) = userService.Register(model);

            if (isRegistered)
            {
                return Redirect("/Users/Login");
            }

            return View(errors, "/Users/Error");
        }

        [Authorize]
        public Response Logout()
        {
            SignOut();
            return Redirect("/");
        }

        

    }
}
