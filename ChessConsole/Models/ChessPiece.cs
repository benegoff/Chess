using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models
{
	public abstract class ChessPiece
	{
		public ChessColor Color { get; set; }
		public byte Row { get; set; }
		public char Column { get; set; }
		public bool HasMoved { get; set; }
		public List<Move> PossibleMoves { get; set; }

		public ChessPiece(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A')
		{
			Color = c;
			Row = row;
			Column = col;
			PossibleMoves = new List<Move>();
		}

		/// <summary>
		/// Returns the char the piece represents.
		/// </summary>
		/// <returns>'K' if the piece is a King, 'Q' for Queen, 'R' for Rook, 'N' for Knight, 'B' for Bishop, or 'P' for Pawn.</returns>
		public abstract char GetPieceChar();

		/// <summary>
		/// Returns a char representing the color of the piece.
		/// </summary>
		/// <returns>'W' if the piece is white, 'B' if the piece is black.</returns>
		public char GetColorChar()
		{
			char c = '-';
			switch(Color)
			{
				case ChessColor.BLACK:
					c = 'B';
					break;
				case ChessColor.WHITE:
					c = 'W';
					break;
			}
			return c;
		}

		public abstract ChessPiece CopyPiece();
	}
}
