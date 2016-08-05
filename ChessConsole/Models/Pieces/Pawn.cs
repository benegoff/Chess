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
		public Pawn(ChessColor c = ChessColor.WHITE, char row = 'A', byte col = 1) : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'P';
		}

		public override bool CheckMoveValidity(char row1, byte col1, char row2, byte col2, Board b, bool isCapturing)
		{
			return true;
		}
	}
}
