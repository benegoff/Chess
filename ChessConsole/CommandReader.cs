using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole
{
	public class CommandReader
	{
		public void Run(string filePath)
		{
			string[] linesToParse = System.IO.File.ReadAllLines(filePath);
			foreach (string command in linesToParse)
			{
				PrintCommandResult(command);
			}
		}

		private void PrintCommandResult(string command)
		{
			command = command.Trim().ToLower();
			string patternPlacement = @"^([kqbnrp])([ld])([a-h])([1-8])$";
			string patternMovement = @"^([a-h])([1-8])\s*([a-h])([1-8])$";
			string patternCapture = @"^([a-h])([1-8])\s*([a-h])([1-8])[/*]$";
			string patternTwoMovement = @"^([a-h])([1-8])\s*([a-h])([1-8])\s*([a-h])([1-8])\s*([a-h])([1-8])$";

			Match matchPlacement = Regex.Match(command, patternPlacement, RegexOptions.None);
			Match matchMovement = Regex.Match(command, patternMovement, RegexOptions.None);
			Match matchCapture = Regex.Match(command, patternCapture, RegexOptions.None);
			Match matchTwoMovement = Regex.Match(command, patternTwoMovement, RegexOptions.None);
			if (matchPlacement.Success)
			{
				string piece = "PIECE ERROR";
				string color = "COLOR ERROR";
				string position = "POSITION ERROR";
				switch (matchPlacement.Groups[1].Value)
				{
					case "k":
						piece = "King";
						break;
					case "q":
						piece = "Queen";
						break;
					case "b":
						piece = "Bishop";
						break;
					case "n":
						piece = "Knight";
						break;
					case "r":
						piece = "Rook";
						break;
					case "p":
						piece = "Pawn";
						break;
				}
				switch (matchPlacement.Groups[2].Value)
				{
					case "l":
						color = "white";
						break;
					case "d":
						color = "black";
						break;
				}
				position = matchPlacement.Groups[3].Value;
				position += matchPlacement.Groups[4].Value;
				position = position.ToUpper();
				Console.WriteLine("Place the " + color + " " + piece + " on " + position + ".");
			}
			else if (matchMovement.Success)
			{
				string position1 = matchMovement.Groups[1].Value;
				position1 += matchMovement.Groups[2].Value;

				string position2 = matchMovement.Groups[3].Value;
				position2 += matchMovement.Groups[4].Value;
				Console.WriteLine("The piece at " + position1 + " was moved to " + position2 + ".");
			}
			else if (matchCapture.Success)
			{
				string position1 = matchCapture.Groups[1].Value;
				position1 += matchCapture.Groups[2].Value;

				string position2 = matchCapture.Groups[3].Value;
				position2 += matchCapture.Groups[4].Value;
				Console.WriteLine("The piece at " + position1 + " moved to and captured the piece at " + position2 + ".");
			}
			else if (matchTwoMovement.Success)
			{
				string piece1Position1 = matchTwoMovement.Groups[1].Value;
				piece1Position1 += matchTwoMovement.Groups[2].Value;

				string piece1Position2 = matchTwoMovement.Groups[3].Value;
				piece1Position2 += matchTwoMovement.Groups[4].Value;

				string piece2Position1 = matchTwoMovement.Groups[5].Value;
				piece2Position1 += matchTwoMovement.Groups[6].Value;

				string piece2Position2 = matchTwoMovement.Groups[7].Value;
				piece2Position2 += matchTwoMovement.Groups[8].Value;

				Console.WriteLine("The piece at " + piece1Position1 + " was moved to " + piece1Position2 + " and the piece at " + piece2Position1 + " was moved to " + piece2Position2 + ".");
			}
			else
			{
				Console.WriteLine("The command '" + command + "' cannot be recognized.");
			}
		}
	}
}
