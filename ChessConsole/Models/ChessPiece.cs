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

		public ChessPiece(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A')
		{
			Color = c;
			Row = row;
			Column = col;
		}

		public abstract char GetPieceChar();

		public abstract bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing);

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
