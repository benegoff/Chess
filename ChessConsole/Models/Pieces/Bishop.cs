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

		public override char GetPieceChar()
		{
			return 'B';
		}

		public override bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing)
		{
			bool isValid = false;
			if(Math.Abs(row1 - row2) == Math.Abs(col1 - col2) && row1 - row2 != 0)
			{
				isValid = true;

				byte distance = (byte)Math.Abs((col1 - col2));
				if (isCapturing)
				{
					distance -= 1;
				}
				bool movingUp = row1 - row2 < 0 ? true : false;
				bool movingRight = col1 - col2 < 0 ? true : false;

				for (int i = 1; i <= distance && isValid; i++)
				{
					if (movingUp)
					{
						if(movingRight)
						{
							if(b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 + i))))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - i), (char)(col1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - i), (char)(col1 + i))))
							{
								isValid = false;
							}
						}
					}
					else
					{
						if(movingRight)
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 - i))))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - i), (char)(col1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - i), (char)(col1 - i))))
							{
								isValid = false;
							}
						}
					}

				}
			}
			return isValid;
		}
	}
}
