using System.Diagnostics;
using System.Globalization;

namespace Sudoku
{
    internal class Sudoku
    {
        const int BOARD_SIZE = 9, HIGHEST_NUM = 9, EMPTY_SLOT = 0, FIRST_COLUMN = 0;
        const string RDY_BOARDS_FOLDER = "..\\..\\..\\ReadyBoards\\";
        enum DIFFICULTY { Easy = 25, Medium = 23, Hard = 19, Evil = 17 };
        static Stopwatch watch = new Stopwatch();
        private static Board frontBoard = new Board(), backBoard = new Board();
        private static string currDifficulty = "Evil";

        private static void Main(string[] args)
        {
            StartGame();
            //SaveBoardToFile();
        }

        public static void StartGame()
        {
            frontBoard = new Board();
            backBoard = new Board();
            bool continueGame = true;
            int digitAmount = 0;

            while (continueGame)
            {
                digitAmount = GetStartingAmount(currDifficulty);
                CreateBoard(digitAmount);
                Console.Clear();
                PrintBoard(backBoard);
                PrintBoard(frontBoard);
                continueGame = false;
                watch.Stop();
                Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
            }
        }

        private static void CreateBoard(int digitAmount)
        {
            Random rnd = new Random();
            bool valid = false;

            while (!valid)
            {
                watch.Restart();
                InitStartingDigits(backBoard, digitAmount);
                frontBoard.CopyBoard(backBoard);
                valid = Solve(backBoard);
                if (!valid)
                {
                    Console.WriteLine("invalid board, restarting...");
                    backBoard.Clear();
                    frontBoard.Clear();
                }
            }
        }

        private static void InitStartingDigits(Board board, int digitAmount)
        {
            int index = 0, currRow = 0, currCol = 0, newVal = 0, currSpot = 0;
            Random rnd = new Random();

            while (index < digitAmount)
            {
                currRow = rnd.Next(BOARD_SIZE);
                currCol = rnd.Next(BOARD_SIZE);
                currSpot = board.GetSpot(currRow, currCol);
                newVal = rnd.Next(HIGHEST_NUM) + 1;

                if (currSpot == EMPTY_SLOT && IsValidPlacement(board, currRow, currCol, newVal))
                {
                    board.SetSpot(currRow, currCol, newVal);
                    index++;
                }
            }
        }

        private static int GetStartingAmount(string diff)
        {
            int amountToShow = 0;

            switch (diff)
            {
                case "Easy":
                    amountToShow = (int)DIFFICULTY.Easy;
                    break;
                case "Medium":
                    amountToShow = (int)DIFFICULTY.Medium;
                    break;
                case "Hard":
                    amountToShow = (int)DIFFICULTY.Hard;
                    break;
                case "Evil":
                    amountToShow = (int)DIFFICULTY.Evil;
                    break;
                default:
                    break;
            }

            return amountToShow;
        }

        private static bool IsValidPlacement(Board board, int row, int col, int val)
        {
            return !(board.IsExistInBox(row, col, val)
                  || board.IsExistInRow(row, val)
                  || board.IsExistInCol(col, val));
        }

        private static bool Solve(Board board, int row = 0, int col = 0)
        {
            int i = 0;

            if (watch.ElapsedMilliseconds % 1000 == 0)
            {
                Console.Write(".");
            }
            if (watch.ElapsedMilliseconds > 30000)
            {
                Console.WriteLine("returning");
                return false;
            }

            if (row == BOARD_SIZE) //if finished solving
            {
                return true;
            }
            else if (col == BOARD_SIZE) //if on last column, move down a row and start from col 0
            {
                return Solve(board, row + 1, FIRST_COLUMN);
            }
            else if (board.GetSpot(row, col) != EMPTY_SLOT) //if the current spot isn't empty, continues to the next column
            {
                return Solve(board, row, col + 1);
            }
            else
            {
                for (i = 1; i < BOARD_SIZE + 1; i++) //goes over the numbers 1-10 and tries to place each one into the spot
                {
                    if (IsValidPlacement(board, row, col, i))
                    {
                        board.SetSpot(row, col, i); //places the value into the spot
                        if (Solve(board, row, col + 1)) //continues the loop to the next column
                        {
                            return true;
                        }
                        else //if cant solve, backtracks and replaces the numbers with empty slots
                        {
                            board.SetSpot(row, col, EMPTY_SLOT);
                        }
                    }
                }
                return false;
            }
        }

        private static void PrintBoard(Board board)
        {
            int i = 0, j = 0, currVal = 0;

            for (i = 0; i < BOARD_SIZE; i++)
            {
                for (j = 0; j < BOARD_SIZE; j++)
                {
                    currVal = board.GetSpot(i, j);

                    if (currVal != EMPTY_SLOT)
                    {
                        Console.Write(currVal + " ");
                    }
                    else
                    {
                        Console.Write("■ ");
                    }
                }
                Console.WriteLine();
            }
        }

        private static void SaveBoardToFile()
        {
            string fileName = "";
            int index = 0, i = 0, j = 0;

            do
            {
                index++;
                fileName = currDifficulty + "Board" + index;
            } while (File.Exists(RDY_BOARDS_FOLDER + currDifficulty + "\\" + fileName));

            using (StreamWriter sw = File.CreateText(RDY_BOARDS_FOLDER + currDifficulty + "\\" + fileName))
            {
                sw.WriteLine("{");
                for (i = 0; i < BOARD_SIZE; i++)
                {
                    sw.Write("[");
                    for (j = 0; j < BOARD_SIZE; j++)
                    {
                        if (j == 8)
                        {
                            sw.Write("" + backBoard.GetSpot(i, j));
                        }
                        else
                        {
                            sw.Write("" + backBoard.GetSpot(i, j) + ", ");
                        }
                    }
                    sw.WriteLine("]");
                }
                sw.WriteLine("}");

                sw.WriteLine();

                sw.WriteLine("{");
                for (i = 0; i < BOARD_SIZE; i++)
                {
                    sw.Write("[");
                    for (j = 0; j < BOARD_SIZE; j++)
                    {
                        if (j == 8)
                        {
                            sw.Write("" + frontBoard.GetSpot(i, j));
                        }
                        else
                        {
                            sw.Write("" + frontBoard.GetSpot(i, j) + ", ");
                        }
                    }
                    sw.WriteLine("]");
                }
                sw.Write("}");
            }
        }
    }
}