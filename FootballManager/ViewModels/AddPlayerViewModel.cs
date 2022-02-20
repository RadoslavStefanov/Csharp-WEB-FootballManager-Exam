using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.ViewModels
{
    public class AddPlayerViewModel
    {
        public string FullName { get; set; }
        public string ImageUrl { get; set; }
        public string Position { get; set; }
        public int Speed { get; set; }
        public int Endurance { get; set; }
        public string Description { get; set; }
    }
}
