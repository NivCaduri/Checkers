using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_Ex02_Niv_311451694_Goody_206591810
{
    internal class CheckersBoard
    {
        private possibilities_for_squares[,] m_GameBoard;
        private int m_sizeOfBoard;
        private int m_numberOfWhitePieces;
        private int m_numberOfBlackPieces;

        public int GetSizeOfBoard()
        {
            return m_sizeOfBoard;
        }
        public CheckersBoard(int i_sizeOfBoard)
        {
                m_sizeOfBoard = i_sizeOfBoard;
            m_GameBoard = new possibilities_for_squares[i_sizeOfBoard , i_sizeOfBoard];
            for (int i = 0; i < i_sizeOfBoard; i++)
            {
                for (int j = 0; j < i_sizeOfBoard; j++)
                {
                    if( i % 2 == j % 2)
                    {
                         m_GameBoard[i, j] = possibilities_for_squares.empty;
                    }
                    else if (i < i_sizeOfBoard/2 - 1)
                    {
                        m_GameBoard[i, j] = possibilities_for_squares.O;
                        m_numberOfBlackPieces++;
                    }

                    else if(i > i_sizeOfBoard / 2){
                        m_GameBoard[i, j] = possibilities_for_squares.X;
                        m_numberOfWhitePieces++;
                    }
                    else
                    {
                        m_GameBoard[i, j] = possibilities_for_squares.empty;
                    }
                }
            }
        }
        public void ShowBoard()
        {
            char LetterOfCollumn = 'A';
            char LetterOfLine = 'a';
            for (int i = 0; i < m_sizeOfBoard; i++)
            {

                Console.Write("   " + LetterOfCollumn);
                LetterOfCollumn += (char)1;
            }
            Console.WriteLine();
            PrintSeperationLine(m_sizeOfBoard);
            for (int i = 0; i < m_sizeOfBoard; i++)
            {
                Console.Write(LetterOfLine + "| ");
                for(int j = 0; j < m_sizeOfBoard; j++)
                {
                    if(m_GameBoard[i, j] != possibilities_for_squares.empty)
                    {
                        Console.Write(m_GameBoard[i, j] + " | ");

                    }
                    else
                    {
                        Console.Write("  | ");
                    }
                }
                Console.WriteLine();
                PrintSeperationLine(m_sizeOfBoard);
                LetterOfLine += (char)1;
            }

        }
        private void PrintSeperationLine(int i_sizeOfBoard)
        {
            for (int i = 0; i < i_sizeOfBoard * 4 + 2; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }
        public void playMove(string i_movePlayed , Boolean i_jumpingMove)
        {
            int sourceColumn = i_movePlayed[0] - 'A';
            int sourceRow = i_movePlayed[1] - 'a';
            int targetColumn = i_movePlayed[3] - 'A';
            int targetRow = i_movePlayed[4] - 'a';
            m_GameBoard[targetRow, targetColumn] = m_GameBoard[sourceRow, sourceColumn];
            m_GameBoard[sourceRow, sourceColumn] = possibilities_for_squares.empty;
            if(targetRow == m_sizeOfBoard - 1 || targetRow == 0)
            {
                Crown(targetColumn, targetRow);
            }
            if (i_jumpingMove)
            {
                possibilities_for_squares EatenSquare = m_GameBoard[(sourceRow + targetRow) / 2, (sourceColumn + targetColumn) / 2];
                m_GameBoard[(sourceRow + targetRow) /2, (sourceColumn + targetColumn) /2] = possibilities_for_squares.empty;
                if(EatenSquare == possibilities_for_squares.X)
                {
                    m_numberOfWhitePieces--;
                }
                if (EatenSquare == possibilities_for_squares.Z)
                {
                    m_numberOfWhitePieces -= 4;
                }
                if (EatenSquare == possibilities_for_squares.O)
                {
                    m_numberOfBlackPieces--;
                }
                if (EatenSquare == possibilities_for_squares.Q)
                {
                    m_numberOfBlackPieces -= 4;
                }
            }
        }
        private void Crown(int i_column, int i_row)
        {
            Console.WriteLine(i_row + "    " + i_row);
            Console.ReadLine();
            if(m_GameBoard[i_row, i_column] == possibilities_for_squares.X)
            {
                m_GameBoard[i_row, i_column] = possibilities_for_squares.Z;
            }
            if (m_GameBoard[i_row, i_column] == possibilities_for_squares.O)
            {
                m_GameBoard[i_row, i_column] = possibilities_for_squares.Q;
            }
        }
        public possibilities_for_squares GetSquare(int i_column, int i_row)
        {
            return m_GameBoard[i_row, i_column];
        }
        public Boolean IsWhite(int i_sourceColumn, int i_sourceRow) 
        {
            return m_GameBoard[i_sourceRow, i_sourceColumn] == possibilities_for_squares.X ||
                                                    m_GameBoard[i_sourceRow, i_sourceColumn] == possibilities_for_squares.Z;
        }
        public Boolean IsBlack(int i_sourceColumn, int i_sourceRow)
        {
            return !(IsWhite(i_sourceColumn, i_sourceRow) || IsEmpty(i_sourceColumn, i_sourceRow));
        }
        public Boolean IsEmpty(int i_sourceColumn, int i_sourceRow)
        {
            return m_GameBoard[i_sourceRow, i_sourceColumn] == possibilities_for_squares.empty;
        }
        public Boolean IsKing(int i_sourceColumn, int i_sourceRow)
        {
            return (m_GameBoard[i_sourceRow, i_sourceColumn] == possibilities_for_squares.Q ||
                m_GameBoard[i_sourceRow, i_sourceColumn] == possibilities_for_squares.Z);
        }
        public enum possibilities_for_squares
        {
            X,
            Q,
            O,
            Z,
            empty,
        }
        public Boolean gameEmpty()
        {
            return m_numberOfWhitePieces == 0 || m_numberOfBlackPieces == 0;
        }
        public int gameScore()
        {
            int sumOfScoreOfWhitePlayer = 0;
            int sumOfScoreOfBlackPlayer = 0;
            foreach (possibilities_for_squares piece in m_GameBoard)
            {
                if (piece == possibilities_for_squares.X)
                {
                    sumOfScoreOfWhitePlayer++;
                }
                if (piece == possibilities_for_squares.Z)
                {
                    sumOfScoreOfWhitePlayer+=4;
                }
                if (piece == possibilities_for_squares.O)
                {
                    sumOfScoreOfWhitePlayer++;
                }
                if (piece == possibilities_for_squares.Q)
                {
                    sumOfScoreOfWhitePlayer += 4;
                }
            }
            return sumOfScoreOfWhitePlayer - sumOfScoreOfBlackPlayer;
        }
    }
}
