using ChessConsole.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessConsole.Models
{
	public class ChessPiece
	{
		public Pieces Piece { get; set; }
		public ChessColors Color { get; set; }

		public char Row { get; set; }
		public byte Column { get; set; }
	}
}
