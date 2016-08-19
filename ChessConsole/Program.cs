using ChessConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessConsole
{
	public class Program
	{

		public static void Main(string[] args)
		{
			Control c = new Control();
			c.PlayerTurn();
			//if (args.Length > 0)
			//{
			//	CommandReader cr = new CommandReader(c);
			//	cr.Run(args[0]);
			//}
		}
	}
}
