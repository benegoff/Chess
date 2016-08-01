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
			if(args.Length > 0)
			{
				CommandReader cr = new CommandReader();
				cr.Run(args[0]);
			}
		}

	}
}
