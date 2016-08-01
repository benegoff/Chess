using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models
{
	public class Board
	{
		List<ChessPiece> WhitePieces;
		List<ChessPiece> BlackPieces;
		public Board()
		{
			WhitePieces = new List<ChessPiece>();
			BlackPieces = new List<ChessPiece>();

			GeneratePawns(ChessColors.BLACK);
			GeneratePawns(ChessColors.WHITE);

			GenerateOtherPieces(ChessColors.WHITE);
			GenerateOtherPieces(ChessColors.BLACK);
		}

		private void GeneratePawns(ChessColors cc)
		{
			byte column = cc == ChessColors.WHITE ? (byte)2 : (byte)7;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece pawn = new ChessPiece();
				pawn.Piece = Pieces.PAWN;
				pawn.Color = cc;
				pawn.Column = column;
				pawn.Row = (char)(65 + i);
				if(cc == ChessColors.WHITE)
				{
					WhitePieces.Add(pawn);
				}
				else
				{
					BlackPieces.Add(pawn);
				}
			}
		}

		private void GenerateOtherPieces(ChessColors cc)
		{
			byte column = cc == ChessColors.WHITE ? (byte)1 : (byte)8;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece piece = new ChessPiece();
				piece.Color = cc;
				piece.Column = column;
				piece.Row = (char)(65 + i);

				switch(piece.Row)
				{
					case 'A':
					case 'H':
						piece.Piece = Pieces.ROOK;
						break;
					case 'B':
					case 'G':
						piece.Piece = Pieces.KNIGHT;
						break;
					case 'C':
					case 'F':
						piece.Piece = Pieces.BISHOP;
						break;
					case 'D':
						piece.Piece = Pieces.QUEEN;
						break;
					case 'E':
						piece.Piece = Pieces.KING;
						break;
				}

				if (cc == ChessColors.WHITE)
				{
					WhitePieces.Add(piece);
				}
				else
				{
					BlackPieces.Add(piece);
				}
			}
		}

		public void PrintBoard()
		{
			for(int i = 1; i <= 8; i++)
			{
				for (char c = 'A'; c <= 'H'; c++)
				{
					string stringToPrint = "-- ";
					foreach(ChessPiece cp in WhitePieces)
					{
						if(cp.Column == i && cp.Row == c)
						{
							stringToPrint = "" + ChessPiece.GetColorChar(cp.Color) + ChessPiece.GetPieceChar(cp.Piece) + " ";
						}
					}
					foreach (ChessPiece cp in BlackPieces)
					{
						if (cp.Column == i && cp.Row == c)
						{
							stringToPrint = "" + ChessPiece.GetColorChar(cp.Color) + ChessPiece.GetPieceChar(cp.Piece) + " ";
						}
					}
					Console.Write(stringToPrint);
				}
				Console.WriteLine();
			}
		}
	}
}
