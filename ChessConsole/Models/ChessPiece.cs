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
		public char Row { get; set; }
		public byte Column { get; set; }

		public ChessPiece(ChessColor c = ChessColor.WHITE, char row = 'A', byte col = 1)
		{
			Color = c;
			Row = row;
			Column = col;
		}

		public abstract char GetPieceChar();

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
	}
}
