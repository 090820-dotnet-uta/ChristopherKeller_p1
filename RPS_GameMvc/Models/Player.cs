using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RPS_GameMvc.Models
{
	public class Player
	{
        public int PlayerId { get; set; }
        public string Name { get; set; } = "null";
        
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Wins { get; set; } = 0;
        
        [Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int Losses { get; set; } = 0;
    }
}
