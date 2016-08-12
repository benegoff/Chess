using ChessConsole.Enums;
using ChessConsole.Models;
using ChessConsole.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessConsole
{
	public class CommandReader
	{
		public Control Controller { get; set; }

		public CommandReader(Control c)
		{
			Controller = c;
		}

		/// <summary>
		/// Runs the program by reading all the commands.
		/// </summary>
		/// <param name="filePath">The file with the list of chess commands.</param>
		public void Run(string filePath)
		{
			Console.WriteLine("PROGRAM START");
			Controller.PrintBoard();
			string[] linesToParse = System.IO.File.ReadAllLines(filePath);
			foreach (string command in linesToParse)
			{
				PrintCommandResult(command);
			}

		}

		/// <summary>
		/// Uses Regex to check the validity of a command, and then runs the proper code based on the command.
		/// </summary>
		/// <param name="command">The command to be checked.</param>
		private void PrintCommandResult(string command)
		{
			command = command.Trim().ToUpper();
			string patternPlacement = @"^([KQPNBR])([LD])([A-H])([1-8])$";
			string patternMovement = @"^([A-H])([1-8])\s*([A-H])([1-8])$";
			string patternCapture = @"^([A-H])([1-8])\s*([A-H])([1-8])[/*]$";
			string patternTwoMovement = @"^([A-H])([1-8])\s*([A-H])([1-8])\s*([A-H])([1-8])\s*([A-H])([1-8])$";

			Match matchPlacement = Regex.Match(command, patternPlacement, RegexOptions.None);
			Match matchMovement = Regex.Match(command, patternMovement, RegexOptions.None);
			Match matchCapture = Regex.Match(command, patternCapture, RegexOptions.None);
			Match matchTwoMovement = Regex.Match(command, patternTwoMovement, RegexOptions.None);
			if (matchPlacement.Success)
			{
				Controller.PlacePieces(matchPlacement);
			}
			else if (matchMovement.Success)
			{
				Controller.MovePieces(matchMovement);
			}
			else if (matchCapture.Success)
			{
				Controller.CapturePiece(matchCapture);
			}
			else if (matchTwoMovement.Success)
			{
				Controller.MoveTwoPieces(matchTwoMovement);
			}
			else
			{
				Console.WriteLine("The command '" + command + "' cannot be recognized.");
			}
		}

	}
}
