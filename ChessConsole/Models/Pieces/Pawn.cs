using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class Pawn : ChessPiece
	{
		public Pawn(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'P';
		}

		public override bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing)
		{
			return true;
		}
	}
}
