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

		public override char GetPieceChar()
		{
			return 'N';
		}

		public override bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing)
		{
			bool isValid = false;
			if(Math.Abs(row1 - row2) == 1)
			{
				if(Math.Abs(col1 - col2) == 2)
				{
					isValid = true;
					if(b.GetPieceByRowAndColumn(row2, col2) != null)
					{
						isValid = false;
						if(isCapturing && b.GetPieceByRowAndColumn(row2, col2).Color != this.Color)
						{
							isValid = true;
						}
					}
				}
			}
			else if(Math.Abs(col1 - col2) == 1)
			{
				if (Math.Abs(row1 - row2) == 2)
				{
					isValid = true;
					if (b.GetPieceByRowAndColumn(row2, col2) != null)
					{
						isValid = false;
						if (isCapturing && b.GetPieceByRowAndColumn(row2, col2).Color != this.Color)
						{
							isValid = true;
						}
					}
				}
			}
			return isValid;
		}
	}
}
