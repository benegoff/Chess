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
		public Bishop(ChessColor c = ChessColor.WHITE, char row = 'A', byte col = 1) : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'B';
		}

		public override bool CheckMoveValidity(char row1, byte col1, char row2, byte col2, Board b, bool isCapturing)
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

				for (int i = 0; i < distance && isValid; i++)
				{
					if (movingUp)
					{
						if(movingRight)
						{
							if(b.WhitePieces.Contains(b.GetPieceByRowAndColumn((char)(row1 + i), (byte)(col1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((char)(row1 + i), (byte)(col1 + i))))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((char)(row1 - i), (byte)(col1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((char)(row1 - i), (byte)(col1 + i))))
							{
								isValid = false;
							}
						}
					}
					else
					{
						if(movingRight)
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((char)(row1 + i), (byte)(col1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((char)(row1 + i), (byte)(col1 - i))))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((char)(row1 - i), (byte)(col1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((char)(row1 - i), (byte)(col1 - i))))
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
