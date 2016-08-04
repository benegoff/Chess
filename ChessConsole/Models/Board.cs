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
		public List<ChessPiece> WhitePieces{ get; set; }
		public List<ChessPiece> BlackPieces { get; set; }
		public Board()
		{
			WhitePieces = new List<ChessPiece>();
			BlackPieces = new List<ChessPiece>();

			//GeneratePawns(ChessColors.BLACK);
			//GeneratePawns(ChessColors.WHITE);

			//GenerateOtherPieces(ChessColors.WHITE);
			//GenerateOtherPieces(ChessColors.BLACK);
		}

		private void GeneratePawns(ChessColors cc)
		{
			byte column = cc == ChessColors.WHITE ? (byte)2 : (byte)7;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece pawn = new ChessPiece(Pieces.PAWN, cc, (char)(65 + i), column);
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
				Pieces piece = Pieces.PAWN;

				switch((char)(65 + i))
				{
					case 'A':
					case 'H':
						piece = Pieces.ROOK;
						break;
					case 'B':
					case 'G':
						piece = Pieces.KNIGHT;
						break;
					case 'C':
					case 'F':
						piece = Pieces.BISHOP;
						break;
					case 'D':
						piece = Pieces.QUEEN;
						break;
					case 'E':
						piece = Pieces.KING;
						break;
				}
				ChessPiece cp = new ChessPiece(piece, cc, (char)(65 + i), column);
				if (cc == ChessColors.WHITE)
				{
					WhitePieces.Add(cp);
				}
				else
				{
					BlackPieces.Add(cp);
				}
			}
		}

		public void PrintBoard()
		{
			Console.WriteLine();
			for(int i = 8; i >= 1; i--)
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
			Console.WriteLine();
		}
	}
}
