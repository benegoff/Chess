using ChessConsole.Enums;
using ChessConsole.Models.Pieces;
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

		private void GeneratePawns(ChessColor cc)
		{
			byte column = cc == ChessColor.WHITE ? (byte)2 : (byte)7;

			for (int i = 0; i < 8; i++)
			{
				Pawn pawn = new Pawn(cc, (char)(65 + i), column);
				if(cc == ChessColor.WHITE)
				{
					WhitePieces.Add(pawn);
				}
				else
				{
					BlackPieces.Add(pawn);
				}
			}
		}

		private void GenerateOtherPieces(ChessColor cc)
		{
			byte column = cc == ChessColor.WHITE ? (byte)1 : (byte)8;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece cp = new Pawn();
				switch((char)(65 + i))
				{
					case 'A':
					case 'H':
						cp = new Rook(cc, (char)(65 + i), column);
						break;
					case 'B':
					case 'G':
						cp = new Knight(cc, (char)(65 + i), column);
						break;
					case 'C':
					case 'F':
						cp = new Bishop(cc, (char)(65 + i), column);
						break;
					case 'D':
						cp = new Queen(cc, (char)(65 + i), column);
						break;
					case 'E':
						cp = new King(cc, (char)(65 + i), column);
						break;
				}
				if (cc == ChessColor.WHITE)
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
			Console.WriteLine("-----------------------");
			for(int i = 8; i >= 1; i--)
			{
				for (char c = 'A'; c <= 'H'; c++)
				{
					string stringToPrint = "-- ";
					foreach(ChessPiece cp in WhitePieces)
					{
						if(cp.Column == i && cp.Row == c)
						{
							stringToPrint = "" + cp.GetColorChar() + cp.GetPieceChar() + " ";
						}
					}

					foreach (ChessPiece cp in BlackPieces)
					{
						if (cp.Column == i && cp.Row == c)
						{
							stringToPrint = "" + cp.GetColorChar() + cp.GetPieceChar() + " ";
						}
					}
					Console.Write(stringToPrint);
				}
				Console.WriteLine();
			}
			Console.WriteLine("-----------------------");
		}
	}
}
