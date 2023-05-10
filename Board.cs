using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    internal class Board
    {
        private int[,] board = new int[9, 9];


        public Board()
        {
            int i = 0, j = 0;

            for (i = 0; i < board.GetLength(0); i++)
            {
                for (j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = 0;
                }
            }
        }

        public bool IsExistInBox(int row, int col, int val)
        {
            int boxStartRow = (row / 3) * 3, boxStartCol = (col / 3) * 3, i = 0, j = 0;
            const int BOX_SIZE = 3;
            bool isExist = false;

            for (i = boxStartRow; i < boxStartRow + BOX_SIZE && !isExist; i++)
            {
                for (j = boxStartCol; j < boxStartCol + BOX_SIZE && !isExist; j++)
                {
                    if (board[i, j] == val)
                    {
                        isExist = true;
                    }
                }
            }


            return isExist;
        }

        public bool IsExistInRow(int row, int val)
        {
            bool exists = false;
            int i = 0;

            for (i = 0; i < board.GetLength(1) && !exists; i++)
            {
                if (board[row, i] == val)
                {
                    exists = true;
                }
            }

            return exists;
        }

        public bool IsExistInCol(int col, int val)
        {
            bool exists = false;
            int i = 0;

            for (i = 0; i < board.GetLength(0) && !exists; i++)
            {
                if (board[i, col] == val)
                {
                    exists = true;
                }
            }

            return exists;
        }

        public void CopyBoard(Board boardBeingCopied)
        {
            int i = 0, j = 0;

            for (i = 0; i < 9; i++)
            {
                for (j = 0; j < 9; j++)
                {
                    board[i, j] = boardBeingCopied.board[i, j];
                }
            }
        }

        public void Clear()
        {
            int i = 0, j = 0;

            for (i = 0; i < board.GetLength(0); i++)
            {
                for (j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = 0;
                }
            }
        }

        public int GetSpot(int row, int col)
        {
            return board[row, col];
        }

        public void SetSpot(int row, int col, int value)
        {
            board[row, col] = value;
        }
    }
}
