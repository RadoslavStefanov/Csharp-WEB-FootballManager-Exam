using FootballManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Contracts
{
    public interface IPlayerService
    {
        public IEnumerable<AllPlayersViewModel> GetAll();
        public IEnumerable<CollectionsViewModel> GetUserCollection(string userId);
        (bool created, IEnumerable<ErrorViewModel> errors) CreatePlayer(AddPlayerViewModel model);
        (bool created, IEnumerable<ErrorViewModel> errors) IsCreateModelValid(AddPlayerViewModel model);
        (bool added, IEnumerable<ErrorViewModel> errors) AddPlayerToUserCollection(int playerId,string userId);
        (bool deleted, IEnumerable<ErrorViewModel> errors) RemovePlayerFromUserCollection(int playerId, string userId);
    }
}
