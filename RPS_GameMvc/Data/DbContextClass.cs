using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RPS_GameMvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RPS_GameMvc.Data
{
	public class DbContextClass : DbContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<Round> Rounds { get; set; }
		public DbSet<Game> Games { get; set; }

		public DbContextClass()
		{		}

		public DbContextClass(DbContextOptions<DbContextClass> options) : base(options)
		{
			
		}

		//protected override void OnConfiguration(DbContextOptionsBuilder options)
		//{
		//	if (!options.IsConfigured)
		//	{
		//		options
		//	}
		//}

	}
}
