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

		public override ChessPiece CopyPiece()
		{
			Queen q = new Queen();
			q.Color = this.Color;
			q.Row = this.Row;
			q.Column = this.Column;
			foreach (Move m in this.PossibleMoves)
			{
				Move m2 = new Move();
				m2.Column = m.Column;
				m2.Row = m.Row;
				q.PossibleMoves.Add(m2);
			}
			return q;
		}

		public override char GetPieceChar()
		{
			return 'Q';
		}

	}
}
