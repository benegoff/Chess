using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class Queen : ChessPiece
	{
		public Queen(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'Q';
		}

	}
}
