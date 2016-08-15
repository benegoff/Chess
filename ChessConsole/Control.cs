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
	public class Control
	{
		public Control()
		{
			ChessBoard = new Board();
			IsWhitesTurn = true;

			//GeneratePawns(ChessColor.BLACK);
			//GeneratePawns(ChessColor.WHITE);

			//GenerateOtherPieces(ChessColor.WHITE);
			//GenerateOtherPieces(ChessColor.BLACK);
		}

		public Board ChessBoard { get; set; }
		public bool IsWhitesTurn { get; set; }

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
				else
				{
					Console.WriteLine("The Black king is not in check.");
				}
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
				else
				{
					Console.WriteLine("The White king is not in check.");
				}
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
					if (CheckMoveValidity(cp, row2, col2, false, ChessBoard))
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
							Console.WriteLine("The piece at " + position1 + " was moved to " + position2 + ".");
							PrintBoard();
							PrintResultOfTurn();
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
						if (CheckMoveValidity(cp1, row2, col2, true, ChessBoard))
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
								Console.WriteLine("The piece at " + position1 + " moved to and captured the piece at " + position2 + ".");
								PrintBoard();
								PrintResultOfTurn();
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
		/// Runs validity check on a particular piece passed in.
		/// </summary>
		/// <param name="cp">The piece to be moved</param>
		/// <param name="row">The row the piece is moving to</param>
		/// <param name="col">The column the piece is moving to</param>
		/// <param name="isCapturing">Whether or not the piece is capturing another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the move off of.</param>
		/// <returns>Whether or not the specified move is valid.</returns>
		public bool CheckMoveValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			if(cp.GetType() == typeof(King))
			{
				isValid = CheckKingValidity(cp, row, col, isCapturing, b);
			}
			else if(cp.GetType() == typeof(Queen))
			{
				isValid = CheckQueenValidity(cp, row, col, isCapturing, b);
			}
			else if (cp.GetType() == typeof(Knight))
			{
				isValid = CheckKnightValidity(cp, row, col, isCapturing, b);
			}
			else if (cp.GetType() == typeof(Bishop))
			{
				isValid = CheckBishopValidity(cp, row, col, isCapturing, b);
			}
			else if (cp.GetType() == typeof(Rook))
			{
				isValid = CheckRookValidity(cp, row, col, isCapturing, b);
			}
			else if (cp.GetType() == typeof(Pawn))
			{
				isValid = CheckPawnValidity(cp, row, col, isCapturing, b);
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular Knight's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the Knight.</param>
		/// <param name="row">The row the Knight is moving to.</param>
		/// <param name="col">The column the Knight is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the Knight off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckKnightValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			
			if (Math.Abs(cp.Row - row) == 1)
			{
				if (Math.Abs(cp.Column - col) == 2)
				{
					isValid = true;
					if (isCapturing)
					{
						if (b.GetPieceByRowAndColumn(row, col) == null)
						{
							isValid = false;
						}
						else if (b.GetPieceByRowAndColumn(row, col).Color == cp.Color)
						{
							isValid = false;
						}
					}
					else if (b.GetPieceByRowAndColumn(row, col) != null)
					{
						isValid = false;
						if (isCapturing && b.GetPieceByRowAndColumn(row, col).Color != cp.Color)
						{
							isValid = true;
						}
					}
				}
			}
			else if (Math.Abs(cp.Column - col) == 1)
			{
				if (Math.Abs(cp.Row - row) == 2)
				{
					isValid = true;
					if (isCapturing)
					{
						if (b.GetPieceByRowAndColumn(row, col) == null)
						{
							isValid = false;
						}
						else if (b.GetPieceByRowAndColumn(row, col).Color == cp.Color)
						{
							isValid = false;
						}
					}
					else if (b.GetPieceByRowAndColumn(row, col) != null)
					{
						isValid = false;
						if (isCapturing && ChessBoard.GetPieceByRowAndColumn(row, col).Color != cp.Color)
						{
							isValid = true;
						}
					}
				}
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular Bishop's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the Bishop</param>
		/// <param name="row">The row the Bishop is moving to.</param>
		/// <param name="col">The column the Bishop is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the Bishop off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckBishopValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			if (Math.Abs(cp.Row - row) == Math.Abs(cp.Column - col) && cp.Row - row != 0)
			{
				isValid = true;
				if (isCapturing)
				{
					if (b.GetPieceByRowAndColumn(row, col) == null)
					{
						isValid = false;
					}
					else if (b.GetPieceByRowAndColumn(row, col).Color == cp.Color)
					{
						isValid = false;
					}
				}
				else
				{
					byte distance = (byte)Math.Abs((cp.Column - col));
					if (isCapturing)
					{
						distance -= 1;
					}
					bool movingUp = cp.Row - row < 0 ? true : false;
					bool movingRight = cp.Column - col < 0 ? true : false;

					for (int i = 1; i <= distance && isValid; i++)
					{
						if (movingUp)
						{
							if (movingRight)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + i), (char)(cp.Column + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + i), (char)(cp.Column + i))))
								{
									isValid = false;
								}
							}
							else
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - i), (char)(cp.Column + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - i), (char)(cp.Column + i))))
								{
									isValid = false;
								}
							}
						}
						else
						{
							if (movingRight)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + i), (char)(cp.Column - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + i), (char)(cp.Column - i))))
								{
									isValid = false;
								}
							}
							else
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - i), (char)(cp.Column - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - i), (char)(cp.Column - i))))
								{
									isValid = false;
								}
							}
						}

					}
				}
				
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular Rook's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the Rook</param>
		/// <param name="row">The row the Rook is moving to.</param>
		/// <param name="col">The column the Rook is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the Rook off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckRookValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			if (!(cp.Row == row && cp.Column == col))
			{
				
				if (cp.Row == row)
				{
					isValid = true;
					if(isCapturing)
					{
						if (b.GetPieceByRowAndColumn(row, col) == null)
						{
							isValid = false;
						}
						else if (b.GetPieceByRowAndColumn(row, col).Color == cp.Color)
						{
							isValid = false;
						}
					}
					else
					{
						byte distance = (byte)Math.Abs((cp.Column - col));
						if (isCapturing)
						{
							distance -= 1;
						}
						bool movingUp = cp.Column - col < 0 ? true : false;

						for (int i = 0; i < distance; i++)
						{
							if (movingUp)
							{
								if (ChessBoard.WhitePieces.Contains(ChessBoard.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column + 1 + i))) || ChessBoard.BlackPieces.Contains(ChessBoard.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column + 1 + i))))
								{
									isValid = false;
								}
							}
							else
							{
								if (ChessBoard.WhitePieces.Contains(ChessBoard.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column - 1 - i))) || ChessBoard.BlackPieces.Contains(ChessBoard.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column - 1 - i))))
								{
									isValid = false;
								}
							}

						}
					}
					
				}
				else if (cp.Column == col)
				{
					isValid = true;
					byte distance = (byte)Math.Abs((cp.Row - row));
					if (isCapturing)
					{
						distance -= 1;
					}
					bool movingRight = cp.Row - row < 0 ? true : false;

					for (int i = 0; i < distance && isValid; i++)
					{
						if (movingRight)
						{
							if (ChessBoard.WhitePieces.Contains(ChessBoard.GetPieceByRowAndColumn((byte)(cp.Row + 1 + i), cp.Column)) || ChessBoard.BlackPieces.Contains(ChessBoard.GetPieceByRowAndColumn((byte)(cp.Row + 1 + i), cp.Column)))
							{
								isValid = false;
							}
						}
						else
						{
							if (ChessBoard.WhitePieces.Contains(ChessBoard.GetPieceByRowAndColumn((byte)(cp.Row - 1 - i), cp.Column)) || ChessBoard.BlackPieces.Contains(ChessBoard.GetPieceByRowAndColumn((byte)(cp.Row - 1 - i), cp.Column)))
							{
								isValid = false;
							}
						}

					}
				}
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular Queen's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the Queen</param>
		/// <param name="row">The row the Queen is moving to.</param>
		/// <param name="col">The column the Queen is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the Queen off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckQueenValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			if(CheckRookValidity(cp, row, col, isCapturing, b) || CheckBishopValidity(cp, row, col, isCapturing, b))
			{
				isValid = true;
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular King's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the King</param>
		/// <param name="row">The row the King is moving to.</param>
		/// <param name="col">The column the King is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the King off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckKingValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			if (Math.Abs(cp.Row - row) <= 1 && Math.Abs(cp.Column - col) <= 1 && !(cp.Row == row && cp.Column == col))
			{
				isValid = CheckQueenValidity(cp, row, col, isCapturing, b);
			}
			return isValid;
		}

		/// <summary>
		/// Checks to see if a particular Pawn's movement is valid.
		/// </summary>
		/// <param name="cp">The instance of the Pawn</param>
		/// <param name="row">The row the Pawn is moving to.</param>
		/// <param name="col">The column the Pawn is moving to.</param>
		/// <param name="isCapturing">Whether or not the piece is going to capture another piece.</param>
		/// <param name="b">The instance of the board to check the validity of the Pawn off of.</param>
		/// <returns>True if the move is valid, false if the move is not.</returns>
		public bool CheckPawnValidity(ChessPiece cp, byte row, char col, bool isCapturing, Board b)
		{
			bool isValid = false;
			int rowDistance = 1;
			int rowDistanceToCheck = 0;
			if (cp.Color == ChessColor.BLACK)
			{
				rowDistanceToCheck = cp.Row - row;
			}
			else
			{
				rowDistanceToCheck = row - cp.Row;
			}
			if (!(cp.Row == row))
			{
				if (isCapturing)
				{
					if(Math.Abs(col - cp.Column) == 1 && rowDistanceToCheck == 1)
					{
						if(b.GetPieceByRowAndColumn(row, col) != null)
						{
							isValid = true;
						}
					}
				}
				else
				{
					
					if (!cp.HasMoved)
					{
						rowDistance = 2;
					}

					if(cp.Column - col == 0 && rowDistanceToCheck > 0 && rowDistanceToCheck <= rowDistance)
					{
						isValid = true;
					}

					if(isValid)
					{
						for (int i = 1; i <= rowDistance; i++)
						{
							if (b.GetPieceByRowAndColumn((byte)(cp.Row + i), cp.Column) != null)
							{
								isValid = false;
							}
						}
					}
				}
			}
			
			
			return isValid;
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
					if(CheckMoveValidity(cp, i, j, true, b) || CheckMoveValidity(cp, i, j, false, b))
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

			Board b = CloneBoard();
			ChessPiece p = b.GetPieceByRowAndColumn(cp.Row, cp.Column);

			if (isCapturing)
			{
				switch(cp.Color)
				{
					case ChessColor.WHITE:
						b.WhitePieces.Remove(b.GetPieceByRowAndColumn(m.Row, m.Column));
						break;
					case ChessColor.BLACK:
						b.BlackPieces.Remove(b.GetPieceByRowAndColumn(m.Row, m.Column));
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
		/// <returns>A new copy of the collection.</returns>
		public List<ChessPiece> CopyPieces(ChessColor cc)
		{

			List<ChessPiece> pieces = (cc == ChessColor.WHITE ? ChessBoard.WhitePieces : ChessBoard.BlackPieces);
			List<ChessPiece> newPieces = new List<ChessPiece>();

			foreach (ChessPiece cp in pieces)
			{
				if (cp.GetType() == typeof(King))
				{
					King k = new King();
					k.Color = cp.Color;
					k.Row = cp.Row;
					k.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						k.PossibleMoves.Add(m2);
					}
					newPieces.Add(k);
				}
				else if (cp.GetType() == typeof(Queen))
				{
					Queen q = new Queen();
					q.Color = cp.Color;
					q.Row = cp.Row;
					q.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						q.PossibleMoves.Add(m2);
					}
					newPieces.Add(q);
				}
				else if (cp.GetType() == typeof(Knight))
				{
					Knight k = new Knight();
					k.Color = cp.Color;
					k.Row = cp.Row;
					k.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						k.PossibleMoves.Add(m2);
					}
					newPieces.Add(k);
				}
				else if (cp.GetType() == typeof(Bishop))
				{
					Bishop b = new Bishop();
					b.Color = cp.Color;
					b.Row = cp.Row;
					b.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						b.PossibleMoves.Add(m2);
					}
					newPieces.Add(b);
				}
				else if (cp.GetType() == typeof(Rook))
				{
					Rook r = new Rook();
					r.Color = cp.Color;
					r.Row = cp.Row;
					r.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						r.PossibleMoves.Add(m2);
					}
					newPieces.Add(r);
				}
				else if (cp.GetType() == typeof(Pawn))
				{
					Pawn p = new Pawn();
					p.Color = cp.Color;
					p.Row = cp.Row;
					p.Column = cp.Column;
					foreach (Move m in cp.PossibleMoves)
					{
						Move m2 = new Move();
						m2.Column = m.Column;
						m2.Row = m.Row;
						p.PossibleMoves.Add(m2);
					}
					newPieces.Add(p);
				}
			}
			return newPieces;
		}

		/// <summary>
		/// Creates a clone of the current board.
		/// </summary>
		/// <returns>A new board which is a copy of the original board.</returns>
		public Board CloneBoard()
		{
			Board b = new Board();
			List<ChessPiece> whitePieceCopy = CopyPieces(ChessColor.WHITE);
			List<ChessPiece> blackPieceCopy = CopyPieces(ChessColor.BLACK);

			b.WhitePieces = whitePieceCopy;
			b.BlackPieces = blackPieceCopy;

			return b;
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
