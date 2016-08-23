using Chess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models.Pieces
{
	public class Pawn : ChessPiece
	{
		public Pawn(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override ChessPiece CopyPiece()
		{
			Pawn p = new Pawn();
			p.Color = this.Color;
			p.Row = this.Row;
			p.Column = this.Column;
			foreach (Move m in this.PossibleMoves)
			{
				Move m2 = new Move();
				m2.Column = m.Column;
				m2.Row = m.Row;
				p.PossibleMoves.Add(m2);
			}
			return p;
		}

		public override char GetPieceChar()
		{
			return 'P';
		}

	}
}
