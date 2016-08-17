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

		public override ChessPiece CopyPiece()
		{
			King k = new King();
			k.Color = this.Color;
			k.Row = this.Row;
			k.Column = this.Column;
			foreach (Move m in this.PossibleMoves)
			{
				Move m2 = new Move();
				m2.Column = m.Column;
				m2.Row = m.Row;
				k.PossibleMoves.Add(m2);
			}
			return k;
		}

		public override char GetPieceChar()
		{
			return 'K';
		}

	}
}
