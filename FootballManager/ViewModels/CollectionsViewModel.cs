using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.ViewModels
{
    public class CollectionsViewModel
    {

        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public byte Speed { get; set; }
        public byte Endurance { get; set; }
        public int playerId { get; set; }

    }
}
