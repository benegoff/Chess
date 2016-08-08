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

			GeneratePawns(ChessColor.BLACK);
			GeneratePawns(ChessColor.WHITE);

			GenerateOtherPieces(ChessColor.WHITE);
			GenerateOtherPieces(ChessColor.BLACK);
		}

		private void GeneratePawns(ChessColor cc)
		{
			byte row = cc == ChessColor.WHITE ? (byte)2 : (byte)7;

			for (int i = 0; i < 8; i++)
			{
				Pawn pawn = new Pawn(cc, row, (char)(65 + i));
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
			byte row = cc == ChessColor.WHITE ? (byte)1 : (byte)8;

			for (int i = 0; i < 8; i++)
			{
				ChessPiece cp = new Pawn();
				switch((char)(65 + i))
				{
					case 'A':
					case 'H':
						cp = new Rook(cc, row, (char)(65 + i));
						break;
					case 'B':
					case 'G':
						cp = new Knight(cc, row, (char)(65 + i));
						break;
					case 'C':
					case 'F':
						cp = new Bishop(cc, row, (char)(65 + i));
						break;
					case 'D':
						cp = new Queen(cc, row, (char)(65 + i));
						break;
					case 'E':
						cp = new King(cc, row, (char)(65 + i));
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

		public ChessPiece GetPieceByRowAndColumn(byte row, char col)
		{
			ChessPiece cp = null;
			bool pieceWasFound = false;
			for (int i = 0; i < WhitePieces.Count && !pieceWasFound; i++)
			{
				if (WhitePieces[i].Row == row && WhitePieces[i].Column == col)
				{
					cp = WhitePieces[i];
					pieceWasFound = true;
				}
			}
			for (int i = 0; i < BlackPieces.Count && !pieceWasFound; i++)
			{
				if (BlackPieces[i].Row == row && BlackPieces[i].Column == col)
				{
					cp = BlackPieces[i];
					pieceWasFound = true;
				}
			}
			return cp;
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
						if(cp.Row == i && cp.Column == c)
						{
							stringToPrint = "" + cp.GetColorChar() + cp.GetPieceChar() + " ";
						}
					}

					foreach (ChessPiece cp in BlackPieces)
					{
						if (cp.Row == i && cp.Column == c)
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
