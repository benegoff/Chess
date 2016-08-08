﻿using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models.Pieces
{
	public class Rook : ChessPiece
	{
		public Rook(ChessColor c = ChessColor.WHITE, byte row = 1, char col = 'A') : base(c, row, col) { }

		public override char GetPieceChar()
		{
			return 'R';
		}

		public override bool CheckMoveValidity(byte row1, char col1, byte row2, char col2, Board b, bool isCapturing)
		{
			bool isValid = false;
			if(!(row1 == row2 && col1 == col2))
			{
				if (row1 == row2)
				{
					isValid = true;
					byte distance = (byte)Math.Abs((col1 - col2));
					if (isCapturing)
					{
						distance -= 1;
					}
					bool movingUp = col1 - col2 < 0 ? true : false;

					for(int i = 0; i < distance; i++)
					{
						if(movingUp)
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 + 1 + i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 + 1 + i))))
							{
								isValid = false;
							}
						}
						else
						{
							if (b.WhitePieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 - 1 - i))) || b.BlackPieces.Contains(b.GetPieceByRowAndColumn(row1, (char)(col1 - 1 - i))))
							{
								isValid = false;
							}
						}
						
					}
				}
				else if(col1 == col2)
				{
					isValid = true;
					byte distance = (byte)Math.Abs((row1 - row2));
					if (isCapturing)
					{
						distance -= 1;
					}
					bool movingRight = row1 - row2 < 0 ? true : false;

					for (int i = 0; i < distance && isValid; i++)
					{
						if (movingRight)
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
			return isValid;
		}
	}
}
