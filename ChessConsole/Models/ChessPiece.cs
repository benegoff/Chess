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
		public Pieces Piece { get; set; }
		public ChessColors Color { get; set; }
		public char Row { get; set; }
		public byte Column { get; set; }

		public ChessPiece(Pieces p = Pieces.PAWN, ChessColors c = ChessColors.WHITE, char row = 'A', byte col = 1)
		{
			Piece = p;
			Color = c;
			Row = row;
			Column = col;
		}

		public static char GetPieceChar(Pieces p)
		{
			char c = '-';
			switch(p)
			{
				case Pieces.KING:
					c = 'K';
					break;
				case Pieces.QUEEN:
					c = 'Q';
					break;
				case Pieces.KNIGHT:
					c = 'N';
					break;
				case Pieces.BISHOP:
					c = 'B';
					break;
				case Pieces.ROOK:
					c = 'R';
					break;
				case Pieces.PAWN:
					c = 'P';
					break;
			}
			return c;
		}

		public static char GetColorChar(ChessColors cc)
		{
			char c = '-';
			switch(cc)
			{
				case ChessColors.BLACK:
					c = 'B';
					break;
				case ChessColors.WHITE:
					c = 'W';
					break;
			}
			return c;
		}
	}
}
