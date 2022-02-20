using FootballManager.Contracts;
using FootballManager.Data.Common;
using FootballManager.Data.Models;
using FootballManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository repo;
        public PlayerService(IRepository _repo)
        { repo = _repo; }
              
        public IEnumerable<AllPlayersViewModel> GetAll()
        {
            return repo.All<Player>().Select(t => new AllPlayersViewModel()
            {
                ImageUrl = t.ImageUrl,
                Description = t.Description,
                FullName = t.FullName,
                Position = t.Position,
                Speed = t.Speed,
                Endurance = t.Endurance,
                playerId = t.Id
            }).ToList();
        }


        public (bool created, IEnumerable<ErrorViewModel> errors) CreatePlayer(AddPlayerViewModel model)
        {
            bool created = false; List<ErrorViewModel> errors = new List<ErrorViewModel>();

            var (isValid, validationError) = IsCreateModelValid(model);

            if (!isValid)
            {
                return (isValid, validationError);
            }

            Player player = new Player()
            {
                FullName = model.FullName,
                ImageUrl = model.ImageUrl,
                Position = model.Position,
                Speed = (byte)model.Speed,
                Endurance= (byte)model.Endurance,
                Description = model.Description
        };

            try
            {
                repo.Add(player);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                errors.Add(new ErrorViewModel("Couldn't save user in DB!"));
            }

            return (created, errors);
        }
        public (bool created, IEnumerable<ErrorViewModel> errors) IsCreateModelValid(AddPlayerViewModel model)
        {
            bool isValid = true;
            List<ErrorViewModel> errors = new List<ErrorViewModel>();
            List<string> validPositions = new List<string> {"Goalkeeper","Right Fullback","Left Fullback","Center Back","Defender","Striker","Winger"};

            if (model == null)
            {
                isValid = false;
                errors.Add(new ErrorViewModel("Register model is required!"));
            }

            if (String.IsNullOrWhiteSpace(model.FullName) || model.FullName.Length < 5 || model.FullName.Length > 80)
            {
                isValid = false;
                errors.Add(new ErrorViewModel($"FullName must be between 5 and 80 characters! you entered ${model.FullName}"));
            }

            if (String.IsNullOrWhiteSpace(model.ImageUrl) || model.ImageUrl.Length > 150 )
            {
                isValid = false;
                errors.Add(new ErrorViewModel("The provided ImageUrl is not valid!"));
            }
            if (!validPositions.Contains(model.Position))
            {
                isValid = false;
                errors.Add(new ErrorViewModel($"The provided position: '{model.Position}' is not valid!"));
            }
            if (model.Speed < 0 || model.Speed > 10|| model.Speed==null)
            {
                isValid = false;
                errors.Add(new ErrorViewModel($"The speed cannot be negative or exeed 10! You entered: {model.Speed}"));
            }
            if (model.Endurance < 0 || model.Endurance > 10 || model.Speed == null)
            {
                isValid = false;
                errors.Add(new ErrorViewModel($"The endurance cannot be negative or exeed 10!"));
            }
            if (String.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 200)
            {
                isValid = false;
                errors.Add(new ErrorViewModel($"FullName must be between 5 and 80 characters! you entered ${model.FullName}"));
            }

            return (isValid, errors);
        }

        public (bool added, IEnumerable<ErrorViewModel> errors) AddPlayerToUserCollection(int playerId,string userId)
        {
            bool added = true; 
            List<ErrorViewModel> errors = new List<ErrorViewModel>();
            var player = repo.All<Player>().FirstOrDefault(p => p.Id == playerId);
            var user = repo.All<User>().FirstOrDefault(u => u.Id == userId);

            if (player == null || user == null)
            {
                added = false;
                errors.Add(new ErrorViewModel("Player or User is null!"));
            }

            var userCollection = repo.All<UserPlayer>().Where(u => u.UserId == userId).ToList();
            foreach (var p in userCollection)
            {
                if (p.PlayerId == playerId)
                {
                    added = false;
                    errors.Add(new ErrorViewModel("This player is already in your collection!"));
                    return (added, errors);
                }
            }

            try
            {
                UserPlayer userPlayer = new UserPlayer()
                {
                    UserId = userId,
                    PlayerId = playerId,
                    User = user,
                    Player = player
                };

                repo.Add(userPlayer);
                repo.SaveChanges();

            }
            catch (Exception)
            {
                added = false;
                errors.Add(new ErrorViewModel("The DB refused to add this Player to your collection!"));
            }

            return (added, errors);
        }

        public IEnumerable<CollectionsViewModel> GetUserCollection(string userId)
        {
            var userCollection = 
                repo.All<UserPlayer>()
                .Where(u => u.UserId == userId)
                .Select(p=>p.PlayerId)
                .ToList();

            var playerCollection = new List<Player>();

             foreach (var playerId in userCollection)
             {
                playerCollection.Add(repo.All<Player>().FirstOrDefault(x => x.Id == playerId));
             }

             var result = new List<CollectionsViewModel>();

            foreach (var player in playerCollection)
            {
                result.Add(new CollectionsViewModel
                {
                    ImageUrl = player.ImageUrl,
                    Description = player.Description,
                    FullName = player.FullName,
                    Position = player.Position,
                    Speed = player.Speed,
                    Endurance = player.Endurance,
                    playerId = player.Id
                });
            }

            return result;
        }

        public (bool deleted, IEnumerable<ErrorViewModel> errors) RemovePlayerFromUserCollection(int playerid, string userId)
        {
            bool deleted = true; List<ErrorViewModel> errors = new List<ErrorViewModel>();

            bool userHasPlayer = repo.All<UserPlayer>()
                .Where(u => u.UserId == userId)
                .ToList()
                .Any(p=>p.PlayerId==playerid);

            if (userHasPlayer)
            {
                
                    var player = repo.All<Player>().FirstOrDefault(p=>p.Id==playerid);
                    var user = repo.All<User>().FirstOrDefault(u=>u.Id==userId);
                    var userPlayer = repo.All<UserPlayer>().FirstOrDefault(up=>up.UserId==userId&&up.PlayerId==playerid);

                    if (player == null || user == null || userPlayer == null)
                    {
                        deleted = false;
                        errors.Add(new ErrorViewModel("The DB query has failed!"));
                        return (deleted, errors);
                    }

                    user.UserPlayers.Remove(userPlayer);
                    player.UserPlayers.Remove(userPlayer);

                try
                {
                    repo.Remove<UserPlayer>(userPlayer);
                    repo.SaveChanges();
                }
                catch (Exception)
                {
                    deleted = false;
                    errors.Add(new ErrorViewModel("The DB query has failed!"));
                    return (deleted, errors);
                }
            }
            else
            {
                deleted = false;
                errors.Add(new ErrorViewModel("This player is not in your collection!"));
                return (deleted, errors);
            }

            return (deleted, errors);
        }
    }
}
