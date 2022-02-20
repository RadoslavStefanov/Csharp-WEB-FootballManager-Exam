using BasicWebServer.Server.Attributes;
using BasicWebServer.Server.Controllers;
using BasicWebServer.Server.HTTP;
using FootballManager.Contracts;
using FootballManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Controllers
{
    public class PlayersController : Controller
    {
        private readonly IPlayerService playerService;
        public PlayersController(Request request, IPlayerService _playerService)
            : base(request)
        {
            playerService = _playerService;
        }

        [Authorize]
        public Response All()
        {
            List<AllPlayersViewModel> players = playerService.GetAll().ToList();
            return View( new { players=players,IsAuthenticated = true});
        }

        [Authorize]
        public Response Add()
        {return View(new { IsAuthenticated = true });}

        [Authorize]
        [HttpPost]
        public Response Add(AddPlayerViewModel model)
        {
            var (isCreated, errors) = playerService.CreatePlayer(model);

            if (isCreated)
            {return Redirect("/Players/All");}

            return View(errors, "/Users/Error");
        }

        [Authorize]
        public Response AddToCollection(int playerId)
        {
            (bool isAdded, IEnumerable<ErrorViewModel> errors) =  playerService.AddPlayerToUserCollection(playerId,User.Id);
            if (isAdded)
            {return Redirect("/Players/Collection");}

            return View(errors, "/Users/Error");
        }

        [Authorize]
        public Response Collection()
        {
            List<CollectionsViewModel> players = playerService.GetUserCollection(User.Id).ToList();
            return View(new { players = players, IsAuthenticated = true });
        }

        [Authorize]
        public Response RemoveFromCollection(int playerId)
        {
            (bool isAdded, IEnumerable<ErrorViewModel> errors) = playerService.RemovePlayerFromUserCollection(playerId, User.Id);
            if (isAdded)
            { return Redirect("/Players/Collection"); }

            return View(errors, "/Users/Error");
        }

    }
}
