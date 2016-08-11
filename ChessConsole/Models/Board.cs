using ChessConsole.Enums;
using ChessConsole.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models
{
	public class Board
	{
		public List<ChessPiece> WhitePieces{ get; set; }
		public List<ChessPiece> BlackPieces { get; set; }
		public Board()
		{
			WhitePieces = new List<ChessPiece>();
			BlackPieces = new List<ChessPiece>();
		}

		/// <summary>
		/// Returns the piece at the specified row and column, or null if there is no piece there.
		/// </summary>
		/// <param name="row">The row of the piece to return</param>
		/// <param name="col">The column of the piece to return</param>
		/// <returns>The chess piece at the specified location.</returns>
		public ChessPiece GetPieceByRowAndColumn(byte row, char col)
		{
			ChessPiece cp = null;
			bool pieceWasFound = false;
			for (int i = 0; i < WhitePieces.Count && !pieceWasFound; i++)
			{
				if (WhitePieces[i].Row == row && WhitePieces[i].Column == col)
				{
					cp = WhitePieces[i];
					pieceWasFound = true;
				}
			}
			for (int i = 0; i < BlackPieces.Count && !pieceWasFound; i++)
			{
				if (BlackPieces[i].Row == row && BlackPieces[i].Column == col)
				{
					cp = BlackPieces[i];
					pieceWasFound = true;
				}
			}
			return cp;
		}


	}
}
