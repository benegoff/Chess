using ChessConsole.Enums;
using ChessConsole.Models;
using ChessConsole.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole
{
    public class ValidityControl
    {
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
			if (cp.GetType() == typeof(King))
			{
				isValid = CheckKingValidity(cp, row, col, isCapturing, b);
			}
			else if (cp.GetType() == typeof(Queen))
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
						if (isCapturing && b.GetPieceByRowAndColumn(row, col).Color != cp.Color)
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
						bool movingUp = cp.Column - col < 0 ? true : false;

						for (int i = 0; i < distance; i++)
						{
							if (movingUp)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column + 1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column + 1 + i))))
								{
									isValid = false;
								}
							}
							else
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column - 1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(cp.Row, (char)(cp.Column - 1 - i))))
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
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + 1 + i), cp.Column)) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row + 1 + i), cp.Column)))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - 1 - i), cp.Column)) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(cp.Row - 1 - i), cp.Column)))
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
			if (CheckRookValidity(cp, row, col, isCapturing, b) || CheckBishopValidity(cp, row, col, isCapturing, b))
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
					if (Math.Abs(col - cp.Column) == 1 && rowDistanceToCheck == 1)
					{
						if (b.GetPieceByRowAndColumn(row, col) != null)
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

					if (cp.Column - col == 0 && rowDistanceToCheck > 0 && rowDistanceToCheck <= rowDistance)
					{
						isValid = true;
					}

					if (isValid)
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
	}
}
