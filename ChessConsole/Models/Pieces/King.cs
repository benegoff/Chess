using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class King : ChessPiece
	{
		public King(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public bool IsInCheck { get; set; }

		public override char GetPieceChar()
		{
			return 'K';
		}

	}
}
