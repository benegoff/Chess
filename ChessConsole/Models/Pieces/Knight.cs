using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class Knight : ChessPiece
	{
		public Knight(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override ChessPiece CopyPiece()
		{
			Knight k = new Knight();
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
			return 'N';
		}

	}
}
