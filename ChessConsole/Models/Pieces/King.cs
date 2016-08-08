using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class King : ChessPiece
	{
		public King(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'K';
		}

		public override bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing)
		{
			bool isValid = false;
			if(Math.Abs(row1 - row2) <= 1 && Math.Abs(col1 - col2) <= 1 && !(row1 == row2 && col1 == col2))
			{
				if (Math.Abs(row1 - row2) == Math.Abs(col1 - col2) && row1 - row2 != 0)
				{
					isValid = true;

					byte distance = (byte)Math.Abs((col1 - col2));
					if(isCapturing)
					{
						distance -= 1;
					}
					bool movingUp = row1 - row2 < 0 ? true : false;
					bool movingRight = col1 - col2 < 0 ? true : false;

					for (int i = 0; i < distance && isValid; i++)
					{
						if (movingUp)
						{
							if (movingRight)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + i), (char)(col1 + i))))
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
							if (movingRight)
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
				else if (!(row1 == row2 && col1 == col2))
				{
					if (row1 == row2)
					{
						isValid = true;
						byte distance = (byte)Math.Abs((col1 - col2));
						if (isCapturing)
						{
							distance -= 1;
						}
						bool movingRight = col1 - col2 < 0 ? true : false;

						for (int i = 0; i < distance; i++)
						{
							if (movingRight)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 + 1 + distance))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 + 1 + distance))))
								{
									isValid = false;
								}
							}
							else
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 - 1 - distance))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 - 1 - distance))))
								{
									isValid = false;
								}
							}

						}
					}
					else if (col1 == col2)
					{
						isValid = true;
						byte distance = (byte)Math.Abs((row1 - row2));
						if (isCapturing)
						{
							distance -= 1;
						}
						bool movingUp = row1 - row2 < 0 ? true : false;

						for (int i = 0; i < distance && isValid; i++)
						{
							if (movingUp)
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + 1 + i), col1)) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 + 1 + i), col1)))
								{
									isValid = false;
								}
							}
							else
							{
								if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - 1 - i), col1)) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn((byte)(row1 - 1 - i), col1)))
								{
									isValid = false;
								}
							}

						}
					}
				}
			}
			return isValid;
		}
	}
}
