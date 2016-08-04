using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models
{
	public class ChessPiece
	{
		public Piece Piece { get; set; }
		public ChessColor Color { get; set; }
		public char Row { get; set; }
		public byte Column { get; set; }

		public ChessPiece(Piece p = Piece.PAWN, ChessColor c = ChessColor.WHITE, char row = 'A', byte col = 1)
		{
			Piece = p;
			Color = c;
			Row = row;
			Column = col;
		}

		public static char GetPieceChar(Piece p)
		{
			char c = '-';
			switch(p)
			{
				case Piece.KING:
					c = 'K';
					break;
				case Piece.QUEEN:
					c = 'Q';
					break;
				case Piece.KNIGHT:
					c = 'N';
					break;
				case Piece.BISHOP:
					c = 'B';
					break;
				case Piece.ROOK:
					c = 'R';
					break;
				case Piece.PAWN:
					c = 'P';
					break;
			}
			return c;
		}

		public static char GetColorChar(ChessColor cc)
		{
			char c = '-';
			switch(cc)
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
	}
}
