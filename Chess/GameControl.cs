using Chess.Enums;
using Chess.Models;
using Chess.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chess
{
	public class GameControl
	{
		public GameControl()
		{
			VC = new ValidityControl();
			ChessBoard = new Board();
			IsWhitesTurn = true;


			GeneratePawns(ChessColor.BLACK);
			GeneratePawns(ChessColor.WHITE);

			GenerateOtherPieces(ChessColor.WHITE);
			GenerateOtherPieces(ChessColor.BLACK);
			
			UpdateAllPossibleMoves(ChessBoard);
		}

		public ValidityControl VC { get; set; }
		public Board ChessBoard { get; set; }
		public bool IsWhitesTurn { get; set; }

		public void PlayerTurn()
		{
			//string patternMovement = @"^([A-H])([1-8])\s*([A-H])([1-8])$";
			//string patternCapture = @"^([A-H])([1-8])\s*([A-H])([1-8])[/*]$";

			//PrintBoard();
			//List <ChessPiece> pieces = IsWhitesTurn ? ChessBoard.WhitePieces : ChessBoard.BlackPieces;
			//ChessPiece choice = PromptForPiece(pieces);
			//Move m = PromptForMove(choice);

			//string command = "" + choice.Column + choice.Row + " " + m.Column + m.Row;
			//if(ChessBoard.GetPieceByRowAndColumn(m.Row, m.Column) != null)
			//{
			//	command += "*";
			//	Match matchCapture = Regex.Match(command, patternCapture, RegexOptions.None);
			//	CapturePiece(matchCapture);
			//}
			//else
			//{
			//	Match matchMovement = Regex.Match(command, patternMovement, RegexOptions.None);
			//	MovePieces(matchMovement);
			//}
			
		}

		private void PromptForPawnPromotion(ChessPiece cp)
		{
			ChessPiece newPiece = new Pawn();
			bool isValid = false;
			int choice = 0;
			while (!isValid)
			{
				Console.WriteLine("Which piece would you like to promote your pawn to?");
				Console.WriteLine("1: Rook");
				Console.WriteLine("2: Knight");
				Console.WriteLine("3: Bishop");
				Console.WriteLine("4: Queen");
				Console.Write("Your Selection: ");
				string unparsedChoice = Console.ReadLine();
				isValid = int.TryParse(unparsedChoice, out choice);
				if(isValid)
				{
					if(choice < 1 || choice > 4)
					{
						isValid = false;
						Console.WriteLine("That is not a valid choice.");
					}
				}
			}

			switch(choice)
			{
				case 1:
					newPiece = new Rook(cp.Color, cp.Row, cp.Column);
					break;
				case 2:
					newPiece = new Knight(cp.Color, cp.Row, cp.Column);
					break;
				case 3:
					newPiece = new Bishop(cp.Color, cp.Row, cp.Column);
					break;
				case 4:
					newPiece = new Queen(cp.Color, cp.Row, cp.Column);
					break;
			}

			List<ChessPiece> pieces = cp.Color == ChessColor.WHITE ? ChessBoard.WhitePieces : ChessBoard.BlackPieces;
			pieces.Remove(cp);
			pieces.Add(newPiece);

		}

		private Move PromptForMove(ChessPiece cp)
		{
			int selection = 0;
			bool isValid = false;
			List<Move> ListOfMoves = new List<Move>();
			foreach(Move m in cp.PossibleMoves)
			{
				if(CheckValidityOfHypotheticalMove(cp, m, true) || CheckValidityOfHypotheticalMove(cp, m, false))
				{
					ListOfMoves.Add(m);
				}
			}
			while (!isValid)
			{
				Console.WriteLine("Please enter which move you would like to make:");
				for (int i = 0; i < ListOfMoves.Count; i++)
				{
					Console.WriteLine((i + 1) + ": " + ListOfMoves[i].Column + ListOfMoves[i].Row);
				}

				Console.Write("Your selection: ");
				string unparsedSelection = Console.ReadLine();

				isValid = int.TryParse(unparsedSelection, out selection);

				if (selection < 1 || selection > ListOfMoves.Count)
				{
					isValid = false;
				}

				if (!isValid)
				{
					Console.WriteLine("That's not a valid input!");
				}
			}
			return ListOfMoves[selection - 1];
		}

		private ChessPiece PromptForPiece(List<ChessPiece> pieces)
		{
			int selection = 0;
			List<ChessPiece> moveablePieces = new List<ChessPiece>();
			foreach (ChessPiece cp in pieces)
			{
				if (cp.PossibleMoves.Count > 0)
				{
					List<Move> ListOfMoves = new List<Move>();
					foreach (Move m in cp.PossibleMoves)
					{
						if (CheckValidityOfHypotheticalMove(cp, m, true) || CheckValidityOfHypotheticalMove(cp, m, false))
						{
							ListOfMoves.Add(m);
						}
					}
					if(ListOfMoves.Count > 0)
					{
						moveablePieces.Add(cp);
					}
				}
			}

			bool isValid = false;
			while(!isValid)
			{
				Console.WriteLine("Please enter which piece you would like to move:");
				for (int i = 0; i < moveablePieces.Count; i++)
				{
					Console.WriteLine((i + 1) + ": " + moveablePieces[i].GetType().Name + " at " + moveablePieces[i].Column + moveablePieces[i].Row);
				}

				Console.Write("Your selection: ");
				string unparsedSelection = Console.ReadLine();

				isValid = int.TryParse(unparsedSelection, out selection);

				if(selection < 1 || selection > moveablePieces.Count)
				{
					isValid = false;
				}

				if(!isValid)
				{
					Console.WriteLine("That's not a valid input!");
				}
			}
			return moveablePieces[selection - 1];
		}

		/// <summary>
		/// Creates Pawns of the specified color in the standard starting positions
		/// </summary>
		/// <param name="cc">The color of the pawns</param>
		public void GeneratePawns(ChessColor cc)
		{
			byte row = cc == ChessColor.WHITE ? (byte)2 : (byte)7;

			for (int i = 0; i < 8; i++)
			{
				Pawn pawn = new Pawn(cc, row, (char)(65 + i));
				if (cc == ChessColor.WHITE)
				{
					ChessBoard.WhitePieces.Add(pawn);
				}
				else
				{
					ChessBoard.BlackPieces.Add(pawn);
				}
			}
		}

		/// <summary>
		/// Creates every piece that isn't a Pawn in their standard starting positions
		/// </summary>
		/// <param name="cc">The color of the pieces</param>
		public void GenerateOtherPieces(ChessColor cc)
		{
			byte row = cc == ChessColor.WHITE ? (byte)1 : (byte)8;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece cp = new Pawn();
				switch ((char)(65 + i))
				{
					case 'A':
					case 'H':
						cp = new Rook(cc, row, (char)(65 + i));
						break;
					case 'B':
					case 'G':
						cp = new Knight(cc, row, (char)(65 + i));
						break;
					case 'C':
					case 'F':
						cp = new Bishop(cc, row, (char)(65 + i));
						break;
					case 'D':
						cp = new Queen(cc, row, (char)(65 + i));
						break;
					case 'E':
						cp = new King(cc, row, (char)(65 + i));
						break;
				}
				if (cc == ChessColor.WHITE)
				{
					ChessBoard.WhitePieces.Add(cp);
				}
				else
				{
					ChessBoard.BlackPieces.Add(cp);
				}
			}
		}

		/// <summary>
		/// Prints the controller's instance of board to the console.
		/// </summary>
		public void PrintBoard()
		{
			Console.WriteLine("#######################");
			for (int i = 8; i >= 1; i--)
			{
				for (char c = 'A'; c <= 'H'; c++)
				{
					string stringToPrint = "-- ";
					foreach (ChessPiece cp in ChessBoard.WhitePieces)
					{
						if (cp.Row == i && cp.Column == c)
						{
							stringToPrint = "" + cp.GetColorChar() + cp.GetPieceChar() + " ";
						}
					}

					foreach (ChessPiece cp in ChessBoard.BlackPieces)
					{
						if (cp.Row == i && cp.Column == c)
						{
							stringToPrint = "" + cp.GetColorChar() + cp.GetPieceChar() + " ";
						}
					}
					Console.Write(stringToPrint);
				}
				Console.WriteLine();
			}
			Console.WriteLine("#######################");
			
		}

		/// <summary>
		/// Prints the status of the king
		/// </summary>
		public void PrintResultOfTurn()
		{
			if (IsWhitesTurn)
			{
				if (GetKingByColor(ChessColor.BLACK, ChessBoard).IsInCheck)
				{
					if(CheckIfKingIsInCheckmate(ChessColor.BLACK, ChessBoard))
					{
						Console.WriteLine("CHECKMATE! Black loses!");
					}
					else
					{
						Console.WriteLine("The Black king is in check!");
					}
				}
				//else
				//{
				//	Console.WriteLine("The Black king is not in check.");
				//}
			}
			else
			{
				if (GetKingByColor(ChessColor.WHITE, ChessBoard).IsInCheck)
				{
					if(CheckIfKingIsInCheckmate(ChessColor.WHITE, ChessBoard))
					{
						Console.WriteLine("CHECKMATE! White loses!");
					}
					else
					{
						Console.WriteLine("The White king is in check!");
					}
				}
				//else
				//{
				//	Console.WriteLine("The White king is not in check.");
				//}
			}
		}

		/// <summary>
		/// Place a chess piece on the board based on a received Regex match and prints the result of the command to the console.
		/// </summary>
		/// <param name="matchPlacement">The match in the expected command format.</param>
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

			char pieceColumn = position[0];
			byte pieceRow = 0;
			byte.TryParse(matchPlacement.Groups[4].Value, out pieceRow);

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

			Console.WriteLine("Placed the " + cc + " " + piece + " on " + position + ".");
			//UpdateAllPossibleMoves(ChessBoard);
		}

		/// <summary>
		/// Moves a piece to another piece if the move is valid, then prints the result of the command.
		/// </summary>
		/// <param name="matchMovement">The match in the expected command format.</param>
		public void MovePieces(Match matchMovement)
		{
			char col1 = matchMovement.Groups[1].Value[0];
			byte row1 = 0;
			byte.TryParse(matchMovement.Groups[2].Value, out row1);
			string position1 = col1.ToString();
			position1 += row1;

			char col2 = matchMovement.Groups[3].Value[0];
			byte row2 = 0;
			byte.TryParse(matchMovement.Groups[4].Value, out row2);
			string position2 = col2.ToString();
			position2 += row2;

			bool firstPieceWasFound = CheckSpaceForPiece(col1, row1);
			bool secondPieceWasFound = CheckSpaceForPiece(col2, row2);
			if (firstPieceWasFound && ((ChessBoard.GetPieceByRowAndColumn(row1, col1).Color == ChessColor.WHITE && IsWhitesTurn) || (ChessBoard.GetPieceByRowAndColumn(row1, col1).Color == ChessColor.BLACK && !IsWhitesTurn)))
			{
				if (firstPieceWasFound && !secondPieceWasFound)
				{
					ChessPiece cp = ChessBoard.GetPieceByRowAndColumn(row1, col1);
					if (VC.CheckMoveValidity(cp, row2, col2, false, ChessBoard))
					{

						Move m = new Move();
						m.Row = row2;
						m.Column = col2;
						bool isValid = CheckValidityOfHypotheticalMove(cp, m, false);

						if(isValid)
						{
							cp.Row = row2;
							cp.Column = col2;
							cp.HasMoved = true;
							UpdateAllPossibleMoves(ChessBoard);
							UpdateKingCheckStatus(cp.Color == ChessColor.WHITE ? ChessColor.BLACK : ChessColor.WHITE, ChessBoard);
							//Console.WriteLine("The piece at " + position1 + " was moved to " + position2 + ".");
							//PrintResultOfTurn();
							if (cp.GetType() == typeof(Pawn))
							{
								if (cp.Color == ChessColor.WHITE)
								{
									if (cp.Row == 8)
									{
										PromptForPawnPromotion(cp);
									}
								}
								else
								{
									if (cp.Row == 1)
									{
										PromptForPawnPromotion(cp);
									}
								}
							}
							ChangeTurns();
						}
						else
						{
							Console.WriteLine("That move will leave your king in check!");
						}
						
					}
					else
					{
						Console.WriteLine("That move is not valid.");
					}
				}
				else if (secondPieceWasFound)
				{
					Console.WriteLine("There is a piece at " + position2 + ".");
				}
			}
			else if (!firstPieceWasFound)
			{
				Console.WriteLine("There isn't a piece at " + position1 + ".");
			}
			else
			{
				Console.WriteLine("It's not your turn!");
			}
			PlayerTurn();

		}

		/// <summary>
		/// Moves a piece and captures another piece if the move is valid, then prints the result of the command.
		/// </summary>
		/// <param name="matchCapture">The match in the expected command format.</param>
		public void CapturePiece(Match matchCapture)
		{
			char col1 = matchCapture.Groups[1].Value[0];
			byte row1 = 0;
			byte.TryParse(matchCapture.Groups[2].Value, out row1);
			string position1 = col1.ToString();
			position1 += row1;

			char col2 = matchCapture.Groups[3].Value[0];
			byte row2 = 0;
			byte.TryParse(matchCapture.Groups[4].Value, out row2);

			string position2 = col2.ToString();
			position2 += row2;

			bool firstPieceWasFound = CheckSpaceForPiece(col1, row1);
			bool secondPieceWasFound = CheckSpaceForPiece(col2, row2);

			if (firstPieceWasFound && ((ChessBoard.GetPieceByRowAndColumn(row1, col1).Color == ChessColor.WHITE && IsWhitesTurn) || (ChessBoard.GetPieceByRowAndColumn(row1, col1).Color == ChessColor.BLACK && !IsWhitesTurn)))
			{
				if (firstPieceWasFound && !secondPieceWasFound)
				{
					Console.WriteLine("There is not a piece to capture at " + position2 + ".");
				}
				else if (firstPieceWasFound && secondPieceWasFound)
				{
					ChessPiece cp1 = ChessBoard.GetPieceByRowAndColumn(row1, col1);
					ChessPiece cp2 = ChessBoard.GetPieceByRowAndColumn(row2, col2);



					if (cp1.Color == cp2.Color)
					{
						Console.WriteLine("You cannot capture a piece of your own color.");
					}
					else
					{
						if (VC.CheckMoveValidity(cp1, row2, col2, true, ChessBoard))
						{

							Move m = new Move();
							m.Row = row2;
							m.Column = col2;
							bool isValid = CheckValidityOfHypotheticalMove(cp1, m, true);

							if(isValid)
							{
								if (cp2.Color == ChessColor.BLACK)
								{
									ChessBoard.BlackPieces.Remove(cp2);
								}
								else
								{
									ChessBoard.WhitePieces.Remove(cp2);
								}
								cp1.Row = row2;
								cp1.Column = col2;
								cp1.HasMoved = true;
								UpdateAllPossibleMoves(ChessBoard);
								UpdateKingCheckStatus(cp1.Color == ChessColor.WHITE ? ChessColor.BLACK : ChessColor.WHITE, ChessBoard);
								//Console.WriteLine("The piece at " + position1 + " moved to and captured the piece at " + position2 + ".");
								//PrintResultOfTurn();
								if (cp1.GetType() == typeof(Pawn))
								{
									if (cp1.Color == ChessColor.WHITE)
									{
										if (cp1.Row == 8)
										{
											PromptForPawnPromotion(cp1);
										}
									}
									else
									{
										if (cp1.Row == 1)
										{
											PromptForPawnPromotion(cp1);
										}
									}
								}
								ChangeTurns();
							}
							else
							{
								Console.WriteLine("That move will leave your king in check!");
							}

							
						}
						else
						{
							Console.WriteLine("That move is not valid.");
						}
					}
				}
				else if (!firstPieceWasFound)
				{
					Console.WriteLine("There isn't a piece at " + position1 + ".");
				}

			}
			PlayerTurn();

		}

		/// <summary>
		/// Moves two pieces, aka "Castles" two pieces if valid, then prints the result of the command.
		/// </summary>
		/// <param name="matchTwoMovement">The match in the expected command format.</param>
		public void MoveTwoPieces(Match matchTwoMovement)
		{
			char piece1Col1 = matchTwoMovement.Groups[1].Value[0];
			byte piece1Row1 = 0;
			byte.TryParse(matchTwoMovement.Groups[2].Value, out piece1Row1);
			string piece1Position1 = piece1Col1.ToString() + piece1Row1;

			char piece1Col2 = matchTwoMovement.Groups[3].Value[0];
			byte piece1Row2 = 0;
			byte.TryParse(matchTwoMovement.Groups[4].Value, out piece1Row2);
			string piece1Position2 = piece1Col2.ToString() + piece1Row2;

			char piece2Col1 = matchTwoMovement.Groups[5].Value[0];
			byte piece2Row1 = 0;
			byte.TryParse(matchTwoMovement.Groups[6].Value, out piece2Row1);
			string piece2Position1 = piece2Col1.ToString() + piece2Row1;

			char piece2Col2 = matchTwoMovement.Groups[7].Value[0];
			byte piece2Row2 = 0;
			byte.TryParse(matchTwoMovement.Groups[8].Value, out piece2Row2);
			string piece2Position2 = piece2Col2.ToString() + piece2Row2;

			bool firstPieceWasFound = CheckSpaceForPiece(piece1Col1, piece1Row1);
			bool secondPieceWasFound = CheckSpaceForPiece(piece1Col2, piece1Row2);
			bool thirdPieceWasFound = CheckSpaceForPiece(piece2Col1, piece2Row1);
			bool fourthPieceWasFound = CheckSpaceForPiece(piece2Col2, piece2Row2);

			if (firstPieceWasFound && !secondPieceWasFound)
			{
				if (thirdPieceWasFound && !fourthPieceWasFound)
				{
					ChessPiece cp1 = ChessBoard.GetPieceByRowAndColumn(piece1Row1, piece1Col1);
					ChessPiece cp2 = ChessBoard.GetPieceByRowAndColumn(piece2Row1, piece2Col1);

					cp1.Row = piece1Row2;
					cp1.Column = piece1Col2;
					cp2.Row = piece2Row2;
					cp2.Column = piece2Col2;
					Console.WriteLine("The piece at " + piece1Position1 + " was moved to " + piece1Position2 + " and the piece at " + piece2Position1 + " was moved to " + piece2Position2 + ".");
					PrintBoard();
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

		/// <summary>
		/// Checks a given Alpha-Numeric space to see if it contains a piece.
		/// </summary>
		/// <param name="col">The character specifying the column</param>
		/// <param name="row">The byte specifying the row</param>
		/// <returns>True if a space contains a piece, false if the space doesn't contain the piece.</returns>
		public bool CheckSpaceForPiece(char col, byte row)
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

		/// <summary>
		/// Changes which player's turn it is.
		/// </summary>
		public void ChangeTurns()
		{
			if (IsWhitesTurn)
			{
				IsWhitesTurn = false;
			}
			else
			{
				IsWhitesTurn = true;
			}
			PrintTurn();
		}

		/// <summary>
		/// Prints which player's turn it is.
		/// </summary>
		public void PrintTurn()
		{
			if(IsWhitesTurn)
			{
				Console.WriteLine("It is White's turn.");
			}
			else
			{
				Console.WriteLine("It is Black's turn.");
			}
		}

		/// <summary>
		/// Updates the possible move list for every piece on the board.
		/// </summary>
		/// <param name="b">The instance of the board to check the validity of all possible moves.</param>
		public void UpdateAllPossibleMoves(Board b)
		{
			foreach(ChessPiece cp in b.WhitePieces)
			{
				UpdatePossibleMoves(cp, b);
			}
			foreach(ChessPiece cp in b.BlackPieces)
			{
				UpdatePossibleMoves(cp, b);
			}
			//UpdateKingCheckStatus(ChessColor.BLACK, b);
			//UpdateKingCheckStatus(ChessColor.WHITE, b);

		}

		/// <summary>
		/// Updates a ChessPiece's possible moves.
		/// </summary>
		/// <param name="cp">The ChessPiece to update.</param>
		/// <param name="b">The instance of the board to check the validity of all possible moves.</param>
		public void UpdatePossibleMoves(ChessPiece cp, Board b)
		{
			cp.PossibleMoves = new List<Move>();
			for(byte i = 1; i <= 8; i++)
			{
				for (char j = 'A'; j <= 'H'; j++)
				{
					if(VC.CheckMoveValidity(cp, i, j, true, b) || VC.CheckMoveValidity(cp, i, j, false, b))
					{
						
						Move m = new Move();
						m.Column = j;
						m.Row = i;
						
						cp.PossibleMoves.Add(m);
					}
				}
			}
		}

		/// <summary>
		/// Loops through all possible moves and updates the check status of a particular colored king.
		/// </summary>
		/// <param name="cc">The color of the king to check.</param>
		/// <param name="b">The instance of the board to get the status of the king.</param>
		public void UpdateKingCheckStatus(ChessColor cc, Board b)
		{
			King k = GetKingByColor(cc, b);
			bool isInCheck = false;
			if(cc == ChessColor.WHITE)
			{
				foreach(ChessPiece cp in b.BlackPieces)
				{
					foreach(Move m in cp.PossibleMoves)
					{
						if(m.Column == k.Column && m.Row == k.Row)
						{
							isInCheck = true;
						}
					}
				}
			}
			else
			{
				foreach (ChessPiece cp in b.WhitePieces)
				{
					foreach (Move m in cp.PossibleMoves)
					{
						if (m.Column == k.Column && m.Row == k.Row)
						{
							isInCheck = true;
						}
					}
				}
			}
			if(isInCheck)
			{
				k.IsInCheck = true;
			}
			else
			{
				k.IsInCheck = false;
			}
		}

		/// <summary>
		/// Takes in a ChessColor, and checks to see if that colored king is in check.
		/// </summary>
		/// <param name="cc">The color king to check.</param>
		/// <param name="b">The instance of the board to get the king's check status from.</param>
		/// <returns>Whether or not the king is in check.</returns>
		public bool CheckIfKingIsInCheck(ChessColor cc, Board b)
		{
			bool isInCheck = false;
			King king = GetKingByColor(cc, b);

			if(king.IsInCheck)
			{
				isInCheck = true;
			}

			return isInCheck;
		}

		/// <summary>
		/// Gets whether or not the king of the supplied color has any available moves.
		/// </summary>
		/// <param name="cc">The color of the king to check.</param>
		/// <param name="b">The instance of the board to get the king's checkmate status from.</param>
		/// <returns>Whether or not the king is in checkmate.</returns>
		public bool CheckIfKingIsInCheckmate(ChessColor cc, Board b)
		{
			bool isInCheckmate = true;

			List<ChessPiece> pieces = cc == ChessColor.WHITE ? b.WhitePieces : b.BlackPieces;

			foreach(ChessPiece cp in pieces)
			{
				foreach(Move m in cp.PossibleMoves)
				{
					if(CheckValidityOfHypotheticalMove(cp, m, true) || CheckValidityOfHypotheticalMove(cp, m, false))
					{
						isInCheckmate = false;
					}
				}
			}

			return isInCheckmate;
		}

		/// <summary>
		/// Checks if a hypothetical move will put a piece in check.
		/// </summary>
		/// <param name="cp">The piece that is moving</param>
		/// <param name="m">Where they are moving to.</param>
		/// <param name="isCapturing">Whether or not the move includes capturing or not.</param>
		/// <returns>Whether or not the piece will put the king in check.</returns>
		public bool CheckValidityOfHypotheticalMove(ChessPiece cp, Move m, bool isCapturing)
		{
			bool isValid = true;

			Board b = CloneBoard(ChessBoard);
			ChessPiece p = b.GetPieceByRowAndColumn(cp.Row, cp.Column);

			if (isCapturing)
			{
				switch(cp.Color)
				{
					case ChessColor.WHITE:
						b.BlackPieces.Remove(b.GetPieceByRowAndColumn(m.Row, m.Column));
						break;
					case ChessColor.BLACK:
						b.WhitePieces.Remove(b.GetPieceByRowAndColumn(m.Row, m.Column));
						break;
				}
				
			}

			p.Row = m.Row;
			p.Column = m.Column;

			UpdateAllPossibleMoves(b);
			UpdateKingCheckStatus(p.Color, b);



			if(CheckIfKingIsInCheck(p.Color, b))
			{
				isValid = false;
			}

			return isValid;
		}

		/// <summary>
		/// Creates a copy of a collection of pieces based on the passed in color.
		/// </summary>
		/// <param name="cc">The color of the piece collection to copy.</param>
		/// <param name="b">The instance of the board to copy the pieces from.</param>
		/// <returns>A new copy of the collection.</returns>
		public List<ChessPiece> CopyPieces(ChessColor cc, Board board)
		{

			List<ChessPiece> pieces = (cc == ChessColor.WHITE ? board.WhitePieces : board.BlackPieces);
			List<ChessPiece> newPieces = new List<ChessPiece>();

			foreach (ChessPiece cp in pieces)
			{
				ChessPiece newPiece = cp.CopyPiece();
				newPieces.Add(newPiece);
			}
			return newPieces;
		}

		/// <summary>
		/// Creates a clone of the current board.
		/// </summary>
		/// <param name="b">The board to return a copy of.</param>
		/// <returns>A new board which is a copy of the original board.</returns>
		public Board CloneBoard(Board b)
		{
			Board newBoard = new Board();
			List<ChessPiece> whitePieceCopy = CopyPieces(ChessColor.WHITE, b);
			List<ChessPiece> blackPieceCopy = CopyPieces(ChessColor.BLACK, b);

			newBoard.WhitePieces = whitePieceCopy;
			newBoard.BlackPieces = blackPieceCopy;

			return newBoard;
		}

		/// <summary>
		/// Gets the instance of the king of a particular chess color.
		/// </summary>
		/// <param name="cc">The color of king to get.</param>
		/// <param name="b">The instance of the board to get the king from.</param>
		/// <returns>The king of the provided color.</returns>
		public King GetKingByColor(ChessColor cc, Board b)
		{
			King king = new King();
			if (cc == ChessColor.WHITE)
			{
				foreach (ChessPiece cp in b.WhitePieces)
				{
					if (cp.GetType() == typeof(King))
					{
						king = (King)cp;
					}
				}
			}
			else
			{
				foreach (ChessPiece cp in b.BlackPieces)
				{
					if (cp.GetType() == typeof(King))
					{
						king = (King)cp;
					}
				}
			}
			return king;
		}
	}
}
