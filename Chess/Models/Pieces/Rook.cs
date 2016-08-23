using Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.Pieces
{
	public class Rook : ChessPiece
	{
		public Rook(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override ChessPiece CopyPiece()
		{
			Rook r = new Rook();
			r.Color = this.Color;
			r.Row = this.Row;
			r.Column = this.Column;
			foreach (Move m in this.PossibleMoves)
			{
				Move m2 = new Move();
				m2.Column = m.Column;
				m2.Row = m.Row;
				r.PossibleMoves.Add(m2);
			}
			return r;
		}

		public override char GetPieceChar()
		{
			return 'R';
		}

	}
}
