using System;
using System.Drawing;
using System.Windows.Forms;

namespace fill_board_8x8
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            // Board is a TableLayoutPanel 8 x 8
            tableLayoutPanel.AutoSize = true;
            tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            // Add squares
            IterateBoard(addSquare);
            // Add pieces
            IterateBoard(initImage);
        }

        void IterateBoard(Action<int, int> action)
        {
            for (int column = 0; column < 8; column++)
            {
                for (int row = 0; row < 8; row++)
                {
                    action(column, row);
                }
            }
        }

        private void addSquare(int column, int row)
        {
            var color = ((column + row) % 2) == 0 ? Color.White : Color.Black;
            var square = new Square(column, row, color);
            tableLayoutPanel.Controls.Add(square, column, row);
            // Hook the mouse events here
            square.Click += onSquareClicked;
            square.MouseHover += onSquareMouseHover;
        }

        private void onSquareClicked(object sender, EventArgs e) =>
            MessageBox.Show($"Clicked: {sender}");

        private void onSquareMouseHover(object sender, EventArgs e) =>
            _tt.SetToolTip((Control)sender, sender.ToString());

        ToolTip _tt = new ToolTip();

        private void initImage(int column, int row)
        {
            var square = (Square)tableLayoutPanel.GetControlFromPosition(column, row);
            switch (row)
            {
                case 0:
                case 1:
                    square.Piece = Piece.Black;
                    break;
                case 6:
                case 7:
                    square.Piece = Piece.Red;
                    break;
                default:
                    square.Piece = Piece.None;
                    break;
            }
        }
    }
    enum Piece { None, Black, Red };
    class Square : PictureBox // Gives more visual control than Button
    {
        static ToolTip _tt = new ToolTip();
        public Square(int column, int row, Color color)
        {
            BackColor = color;
            Column = column;
            Row = row;
            Size = new Size(80, 80);
            Margin = new Padding(0);
            Padding = new Padding(10);
            Anchor = (AnchorStyles)0xf;
            SizeMode = PictureBoxSizeMode.StretchImage;
        }
        public int Column { get; }
        public int Row { get; }
        Piece _piece = Piece.None;
        public Piece Piece
        {
            get => _piece;
            set
            {
                if(!Equals(_piece, value))
                {
                    _piece = value;
                    switch (_piece)
                    {
                        case Piece.None:
                            Image = null;
                            break;
                        case Piece.Black:
                            Image = Resources.black;
                            break;
                        case Piece.Red:
                            Image = Resources.red;
                            break;
                    }
                }
            }
        }
        public override string ToString() =>
            Piece == Piece.None ? 
                $"Empty {BackColor.Name} square [column:{Column} row:{Row}]" :
                $"{Piece} piece [column:{Column} row:{Row}]";
    }
}
