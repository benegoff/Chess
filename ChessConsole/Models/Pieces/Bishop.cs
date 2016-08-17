using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class Bishop : ChessPiece
	{
		public Bishop(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override ChessPiece CopyPiece()
		{
			Bishop b = new Bishop();
			b.Color = this.Color;
			b.Row = this.Row;
			b.Column = this.Column;
			foreach (Move m in this.PossibleMoves)
			{
				Move m2 = new Move();
				m2.Column = m.Column;
				m2.Row = m.Row;
				b.PossibleMoves.Add(m2);
			}
			return b;
		}

		public override char GetPieceChar()
		{
			return 'B';
		}

	}
}
