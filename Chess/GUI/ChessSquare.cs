using Chess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chess.GUI
{
	public class ChessSquare : Button
	{
		private ChessPiece _piece;
		public char Column { get; set; }
		public byte Row { get; set; }

		public ChessPiece Piece
		{
			get
			{
				return _piece;
			}
			set
			{
				_piece = value;
				if(_piece != null)
				{
					this.Content = "" + _piece.GetColorChar() + _piece.GetPieceChar();
				}
				else
				{
					this.Content = "";
				}
			}
		}

	}
}
