using ChessConsole.Enums;
using ChessConsole.Models;
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
		public Board ChessBoard { get; set; }

		public CommandReader(Board b)
		{
			ChessBoard = b;
		}

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
				PlacePieces(matchPlacement);
			}
			else if (matchMovement.Success)
			{
				MovePieces(matchMovement);
			}
			else if (matchCapture.Success)
			{
				CapturePiece(matchCapture);
			}
			else if (matchTwoMovement.Success)
			{
				MoveTwoPieces(matchTwoMovement);
			}
			else
			{
				Console.WriteLine("The command '" + command + "' cannot be recognized.");
			}
		}

		public void MoveTwoPieces(Match matchTwoMovement)
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

		public void CapturePiece(Match matchCapture)
		{
			string position1 = matchCapture.Groups[1].Value;
			position1 += matchCapture.Groups[2].Value;

			string position2 = matchCapture.Groups[3].Value;
			position2 += matchCapture.Groups[4].Value;
			Console.WriteLine("The piece at " + position1 + " moved to and captured the piece at " + position2 + ".");
		}

		public void MovePieces(Match matchMovement)
		{
			char row1 = matchMovement.Groups[1].Value[0];
			byte col1 = 0;
			byte.TryParse(matchMovement.Groups[2].Value, out col1);
			string position1 = row1.ToString();
			position1 += col1;

			char row2 = matchMovement.Groups[3].Value[0];
			byte col2 = 0;
			byte.TryParse(matchMovement.Groups[4].Value, out col2);

			string position2 = row2.ToString();
			position2 += col2;

			Console.WriteLine("The piece at " + position1 + " was moved to " + position2 + ".");
		}

		public void PlacePieces(Match matchPlacement)
		{
			string piece = "PIECE ERROR";
			Pieces cp = Pieces.PAWN;
			ChessColors cc = ChessColors.BLACK;
			string color = "COLOR ERROR";
			string position = "POSITION ERROR";
			switch (matchPlacement.Groups[1].Value)
			{
				case "K":
					cp = Pieces.KING;
					piece = "King";
					break;
				case "Q":
					cp = Pieces.QUEEN;
					piece = "Queen";
					break;
				case "B":
					cp = Pieces.BISHOP;
					piece = "Bishop";
					break;
				case "N":
					cp = Pieces.KNIGHT;
					piece = "Knight";
					break;
				case "R":
					cp = Pieces.ROOK;
					piece = "Rook";
					break;
				case "P":
					cp = Pieces.PAWN;
					piece = "Pawn";
					break;
			}
			switch (matchPlacement.Groups[2].Value)
			{
				case "L":
					cc = ChessColors.WHITE;
					color = "white";
					break;
				case "D":
					cc = ChessColors.BLACK;
					color = "black";
					break;
			}
			position = matchPlacement.Groups[3].Value;
			position += matchPlacement.Groups[4].Value;
			position = position.ToUpper();

			char pieceRow = position[0];
			byte pieceColumn = 0;

			byte.TryParse(matchPlacement.Groups[4].Value, out pieceColumn);

			ChessPiece chessPiece = new ChessPiece(cp, cc, pieceRow, pieceColumn);
			if (cc == ChessColors.WHITE)
			{
				ChessBoard.WhitePieces.Add(chessPiece);
			}
			else
			{
				ChessBoard.BlackPieces.Add(chessPiece);
			}

			Console.WriteLine("Place the " + color + " " + piece + " on " + position + ".");
		}

	}
}
