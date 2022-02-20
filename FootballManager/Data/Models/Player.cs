using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Data.Models
{
    public class Player
    {

        [Key]
        [StringLength(36)]
        public int Id { get; set; }


        [Required]
        [StringLength(80, MinimumLength = 5)]
        public string FullName { get; set; }


        [Required]
        public string ImageUrl { get; set; }


        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Position { get; set; }


        [Required]
        [Range(0,10)]
        public byte Speed { get; set; }


        [Required]
        [Range(0, 10)]
        public byte Endurance { get; set; }


        [Required]
        [StringLength(200)]
        public string Description { get; set; }


        public ICollection<UserPlayer> UserPlayers { get; set; }

        public Player()
        {
            UserPlayers = new List<UserPlayer>();
        }

    }
}
