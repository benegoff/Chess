using Chess.Enums;
using Chess.GUI;
using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ChessSquare selectedSquare = null;
		public GameControl c = new GameControl();
		public MainWindow()
		{
			InitializeComponent();
			CreateBoard();
			UpdateLabel();
			//c.PlayerTurn();
		}

		private void CreateBoard()
		{
			bool white = true;
			for(int i = 0; i < 8; i++)
			{
				VisualChessBoard.ColumnDefinitions.Add(new ColumnDefinition());
				VisualChessBoard.RowDefinitions.Add(new RowDefinition());
			}
			for(char i = 'A'; i <= 'H'; i++)
			{
				int index = 0;
				for(byte j = 8; j >= 1; j--)
				{
					ChessSquare cs = new ChessSquare();
					cs.BorderBrush = Brushes.Black;
					cs.Background = white ? Brushes.Wheat : Brushes.Gray;
					cs.Row = j;
					cs.Column = i;
					cs.Click += ChessSquare_Click;
					Grid.SetRow(cs, index);
					Grid.SetColumn(cs, i - 65);
					VisualChessBoard.Children.Add(cs);
					white = !white;
					index++;
				}
				white = !white;
			}
			foreach (ChessSquare cs in VisualChessBoard.Children)
			{
				foreach (ChessPiece cp in c.ChessBoard.WhitePieces)
				{
					if (cp.Row == cs.Row && cp.Column == cs.Column)
					{
						cs.Piece = cp;
					}
				}
				foreach (ChessPiece cp in c.ChessBoard.BlackPieces)
				{
					if (cp.Row == cs.Row && cp.Column == cs.Column)
					{
						cs.Piece = cp;
					}
				}
			}
			
		}

		private void UpdateLabel()
		{
			if(c.IsWhitesTurn)
			{
				InfoLabel.Content = "It's White's Turn!";
				if(c.GetKingByColor(ChessColor.WHITE, c.ChessBoard).IsInCheck)
				{
					InfoLabel.Content += "\nYour king is in check!";
				}
			}
			else
			{
				InfoLabel.Content = "It's Black's Turn!";
				if (c.GetKingByColor(ChessColor.BLACK, c.ChessBoard).IsInCheck)
				{
					InfoLabel.Content += "\nYour king is in check!";
				}
			}
			
		}

		private void ChessSquare_Click(object sender, RoutedEventArgs e)
		{
			ChessSquare cs = (ChessSquare)e.Source;
			
			
			if (selectedSquare == null)
			{
				selectedSquare = cs;
				ChessColor cc = selectedSquare.Piece.Color;
				if (c.IsWhitesTurn && cc == ChessColor.WHITE || !(c.IsWhitesTurn) && cc == ChessColor.BLACK)
				{
					
					foreach (Move m in cs.Piece.PossibleMoves)
					{
						if (c.CheckValidityOfHypotheticalMove(cs.Piece, m, true) || c.CheckValidityOfHypotheticalMove(cs.Piece, m, false))
						{
							foreach (ChessSquare cs2 in VisualChessBoard.Children)
							{
								if (cs2.Row == m.Row && cs2.Column == m.Column)
								{
									cs2.BorderBrush = Brushes.Orange;
								}
							}
						}
					}
				}
				
			}
			else
			{
				if (cs.BorderBrush == Brushes.Orange)
				{
					string patternMovement = @"^([A-H])([1-8])\s*([A-H])([1-8])$";
					string patternCapture = @"^([A-H])([1-8])\s*([A-H])([1-8])[/*]$";

					string command = "" + selectedSquare.Column + selectedSquare.Row + " " + cs.Column + cs.Row;
					if (c.ChessBoard.GetPieceByRowAndColumn(cs.Row, cs.Column) != null)
					{
						command += "*";
						Match matchCapture = Regex.Match(command, patternCapture, RegexOptions.None);
						c.CapturePiece(matchCapture);
					}
					else
					{
						Match matchMovement = Regex.Match(command, patternMovement, RegexOptions.None);
						c.MovePieces(matchMovement);
					}

					cs.Piece = selectedSquare.Piece;
					selectedSquare.Piece = null;
					
				}
				foreach (ChessSquare square in VisualChessBoard.Children)
				{
					square.BorderBrush = Brushes.Black;
				}
				selectedSquare = null;
			}
			UpdateLabel();
		}
	}
}
