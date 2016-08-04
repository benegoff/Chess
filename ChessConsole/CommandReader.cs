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
			char piece1Row1 = matchTwoMovement.Groups[1].Value[0];
			byte piece1Col1 = 0;
			byte.TryParse(matchTwoMovement.Groups[2].Value, out piece1Col1);
			string piece1Position1 = piece1Row1.ToString() + piece1Col1;

			char piece1Row2 = matchTwoMovement.Groups[3].Value[0];
			byte piece1Col2 = 0;
			byte.TryParse(matchTwoMovement.Groups[4].Value, out piece1Col2);
			string piece1Position2 = piece1Row2.ToString() + piece1Col2;

			char piece2Row1 = matchTwoMovement.Groups[5].Value[0];
			byte piece2Col1 = 0;
			byte.TryParse(matchTwoMovement.Groups[6].Value, out piece2Col1);
			string piece2Position1 = piece2Row1.ToString() + piece2Col1;

			char piece2Row2 = matchTwoMovement.Groups[7].Value[0];
			byte piece2Col2 = 0;
			byte.TryParse(matchTwoMovement.Groups[8].Value, out piece2Col2);
			string piece2Position2 = piece2Row2.ToString() + piece2Col2;

			bool firstPieceWasFound = CheckSpaceForPiece(piece1Row1, piece1Col1);
			bool secondPieceWasFound = CheckSpaceForPiece(piece1Row2, piece1Col2);
			bool thirdPieceWasFound = CheckSpaceForPiece(piece2Row1, piece2Col1);
			bool fourthPieceWasFound = CheckSpaceForPiece(piece2Row2, piece2Col2);

			if (firstPieceWasFound && !secondPieceWasFound)
			{
				if (thirdPieceWasFound && !fourthPieceWasFound)
				{
					ChessPiece cp1 = GetPieceByRowAndColumn(piece1Row1, piece1Col1);
					ChessPiece cp2 = GetPieceByRowAndColumn(piece2Row1, piece2Col1);

					cp1.Row = piece1Row2;
					cp1.Column = piece1Col2;
					cp2.Row = piece2Row2;
					cp2.Column = piece2Col2;
					Console.WriteLine("The piece at " + piece1Position1 + " was moved to " + piece1Position2 + " and the piece at " + piece2Position1 + " was moved to " + piece2Position2 + ".");
					ChessBoard.PrintBoard();
				}
				else if (!thirdPieceWasFound)
				{
					Console.WriteLine("There isn't a piece at " + piece2Position1 + ".");
				}
				else if (fourthPieceWasFound)
				{
					Console.WriteLine("There isn't space to castle.");
				}
			}
			else if (!firstPieceWasFound)
			{
				Console.WriteLine("There isn't a piece at " + piece1Position1 + ".");
			}
			else if (secondPieceWasFound)
			{
				Console.WriteLine("There isn't space to castle.");
			}
		}

		public void CapturePiece(Match matchCapture)
		{
			char row1 = matchCapture.Groups[1].Value[0];
			byte col1 = 0;
			byte.TryParse(matchCapture.Groups[2].Value, out col1);
			string position1 = row1.ToString();
			position1 += col1;

			char row2 = matchCapture.Groups[3].Value[0];
			byte col2 = 0;
			byte.TryParse(matchCapture.Groups[4].Value, out col2);

			string position2 = row2.ToString();
			position2 += col2;

			bool firstPieceWasFound = CheckSpaceForPiece(row1, col1);
			bool secondPieceWasFound = CheckSpaceForPiece(row2, col2);

			if (firstPieceWasFound && !secondPieceWasFound)
			{
				Console.WriteLine("There is not a piece to capture at " + position2 + ".");
			}
			else if (!firstPieceWasFound)
			{
				Console.WriteLine("There isn't a piece at " + position1 + ".");
			}
			else if (firstPieceWasFound && secondPieceWasFound)
			{
				ChessPiece cp1 = GetPieceByRowAndColumn(row1, col1);
				ChessPiece cp2 = GetPieceByRowAndColumn(row2, col2);
				if(cp1.Color == cp2.Color)
				{
					Console.WriteLine("You cannot capture a piece of your own color.");
				}
				else
				{
					if(cp2.Color == ChessColor.BLACK)
					{
						ChessBoard.BlackPieces.Remove(cp2);
					}
					else
					{
						ChessBoard.WhitePieces.Remove(cp2);
					}
					cp1.Row = row2;
					cp1.Column = col2;
					Console.WriteLine("The piece at " + position1 + " moved to and captured the piece at " + position2 + ".");
					ChessBoard.PrintBoard();
				}
				
			}

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

			bool firstPieceWasFound = CheckSpaceForPiece(row1, col1);
			bool secondPieceWasFound = CheckSpaceForPiece(row2, col2);

			if (firstPieceWasFound && !secondPieceWasFound)
			{
				ChessPiece cp = GetPieceByRowAndColumn(row1, col1);
				cp.Row = row2;
				cp.Column = col2;
				Console.WriteLine("The piece at " + position1 + " was moved to " + position2 + ".");
				ChessBoard.PrintBoard();
			}
			else if(!firstPieceWasFound)
			{
				Console.WriteLine("There isn't a piece at " + position1 + ".");
			}
			else if(secondPieceWasFound)
			{
				Console.WriteLine("There is a piece at " + position2 + ".");
			}

		}

		public ChessPiece GetPieceByRowAndColumn(char row, byte col)
		{
			ChessPiece cp = new Pawn();
			bool pieceWasFound = false;
			for (int i = 0; i < ChessBoard.WhitePieces.Count && !pieceWasFound; i++)
			{
				if (ChessBoard.WhitePieces[i].Row == row && ChessBoard.WhitePieces[i].Column == col)
				{
					cp = ChessBoard.WhitePieces[i];
					pieceWasFound = true;
				}
			}
			for (int i = 0; i < ChessBoard.BlackPieces.Count && !pieceWasFound; i++)
			{
				if (ChessBoard.BlackPieces[i].Row == row && ChessBoard.BlackPieces[i].Column == col)
				{
					cp = ChessBoard.BlackPieces[i];
					pieceWasFound = true;
				}
			}
			return cp;
		}

		public bool CheckSpaceForPiece(char row, byte col)
		{
			bool pieceWasFound = false;
			for (int i = 0; i < ChessBoard.WhitePieces.Count && !pieceWasFound; i++)
			{
				if (ChessBoard.WhitePieces[i].Row == row && ChessBoard.WhitePieces[i].Column == col)
				{
					pieceWasFound = true;
				}
			}
			for (int i = 0; i < ChessBoard.BlackPieces.Count && !pieceWasFound; i++)
			{
				if (ChessBoard.BlackPieces[i].Row == row && ChessBoard.BlackPieces[i].Column == col)
				{
					pieceWasFound = true;
				}
			}
			return pieceWasFound;
		}

		public void PlacePieces(Match matchPlacement)
		{
			ChessPiece chessPiece = new Pawn();
			string piece = "PIECE ERROR";
			ChessColor cc = ChessColor.BLACK;
			string position = "POSITION ERROR";
			switch (matchPlacement.Groups[2].Value)
			{
				case "L":
					cc = ChessColor.WHITE;
					break;
				case "D":
					cc = ChessColor.BLACK;
					break;
			}
			position = matchPlacement.Groups[3].Value;
			position += matchPlacement.Groups[4].Value;
			position = position.ToUpper();

			char pieceRow = position[0];
			byte pieceColumn = 0;
			byte.TryParse(matchPlacement.Groups[4].Value, out pieceColumn);

			switch (matchPlacement.Groups[1].Value)
			{
				case "K":
					chessPiece = new King(cc, pieceRow, pieceColumn);
					piece = "King";
					break;
				case "Q":
					chessPiece = new Queen(cc, pieceRow, pieceColumn);
					piece = "Queen";
					break;
				case "B":
					chessPiece = new Bishop(cc, pieceRow, pieceColumn);
					piece = "Bishop";
					break;
				case "N":
					chessPiece = new Knight(cc, pieceRow, pieceColumn);
					piece = "Knight";
					break;
				case "R":
					chessPiece = new Rook(cc, pieceRow, pieceColumn);
					piece = "Rook";
					break;
				case "P":
					chessPiece = new Pawn(cc, pieceRow, pieceColumn);
					piece = "Pawn";
					break;
			}
			if (cc == ChessColor.WHITE)
			{
				ChessBoard.WhitePieces.Add(chessPiece);
			}
			else
			{
				ChessBoard.BlackPieces.Add(chessPiece);
			}

			Console.WriteLine("Place the " + cc + " " + piece + " on " + position + ".");
		}

	}
}
