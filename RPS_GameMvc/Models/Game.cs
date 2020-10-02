using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPS_GameMvc.Models
{
	public class Game
	{
        public int GameId { get; set; }
        //[Required]
        public Player Player1 { get; set; }
        public Player Computer { get; set; }
        public ICollection<Round> rounds { get; set; } = new List<Round>();
        public Player winner = new Player();
    }
}
