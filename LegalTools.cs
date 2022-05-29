using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_Ex02_Niv_311451694_Goody_206591810
{
    internal class LegalTools
    {
        private Boolean m_isFirstPlayerPlaying;
        private CheckersBoard m_Board;
        
        public LegalTools(CheckersBoard i_gameBoard)
        {
            m_isFirstPlayerPlaying = true;
            m_Board = i_gameBoard;
        }
        public Boolean GetIsFirstPlayerPlaying()
        {
            return m_isFirstPlayerPlaying;
        }
        public Boolean CheckIfMoveIsLegelForGame(String i_move)
        {
            Boolean properCommand = IsLegalCommand(i_move);
            List <string> JumpingMovesList = AllJumpingMoves();
            List<string> regularMovesList = AllLegalRegularMoves();
            Boolean regularMove = properCommand && JumpingMovesList.Count == 0 && IsLegalMove(i_move);
            Boolean jumpingMove = properCommand && JumpingMovesList.Contains(i_move);
            return regularMove || jumpingMove;
        }   
        public void advanceTheTurn()
        {
            m_isFirstPlayerPlaying = !m_isFirstPlayerPlaying;
        }
        public static Boolean IsLetter(string i_strInput)
        {
            Boolean allLetters = true;
            for (int i = 0; i < i_strInput.Length; i++)
            {
                if (!((i_strInput[i] >= 'A' && i_strInput[i] <= 'Z') || (i_strInput[i] >= 'a' && i_strInput[i] <= 'z')))
                {
                    allLetters = false;
                }
            }
            return allLetters;
        }
        public static Boolean IsLegalName(string i_name)
        {
            
            return IsLetter(i_name) && i_name.Length <=10;
        }
        public static Boolean IsLegalSize(string i_size)
        {
            int size;
            return int.TryParse(i_size, out size) && (size == 6 || size == 8 || size == 10);
        }
        private static Boolean IsLowerCaseLetter(char i_letter)
        {
            return i_letter >= 'a' && i_letter <= 'z';
        }
        private static Boolean IsUpperCaseLetter(char i_letter)
        {
            return i_letter >= 'A' && i_letter <= 'Z';
        }
        public Boolean IsLegalCommand(string i_move)
        {
            Boolean properCommandLength = i_move.Length == 5;
            Boolean properSeparator = true;
            Boolean properLetterOrder = true;
            Boolean inTheBoard = true;
            if (properCommandLength)
            {
                properSeparator = i_move[2] == '>';
                properLetterOrder = (IsUpperCaseLetter(i_move[0]) && IsUpperCaseLetter(i_move[3]) &&
                    IsLowerCaseLetter(i_move[1]) && IsLowerCaseLetter(i_move[4]));
                int[] moves = ProcessCommand(i_move);
                int boardSize = m_Board.GetSizeOfBoard();
                for (int i = 0; i < moves.Length; i++)
                {
                    if (moves[i] >= boardSize || moves[i] < 0)
                    {
                        inTheBoard = false;
                    }
                }
            }
            return properCommandLength && properSeparator && properLetterOrder && inTheBoard;
        }
        private Boolean IsLegalMove(string i_move, int i_eatingModifier = 0,int i_kingModifer=1)
        {
            int[] moves = ProcessCommand(i_move);
            int movmentDirection;
            if (!m_isFirstPlayerPlaying)
            {
                movmentDirection = (-1 - i_eatingModifier) * i_kingModifer;
            }
            else
            {
                movmentDirection = (1 +i_eatingModifier) * i_kingModifer;
            }
            Boolean legalPositon = (Math.Abs(moves[0] - moves[2]) == Math.Abs(movmentDirection)) && moves[1] - moves[3] == movmentDirection &&
                m_Board.IsEmpty(moves[2], moves[3]);
            Boolean legalColor = (m_Board.IsWhite(moves[0], moves[1]) == m_isFirstPlayerPlaying &&
                m_Board.IsBlack(moves[0], moves[1]) != m_isFirstPlayerPlaying);
            Boolean pieceToEat = true;
            if (i_eatingModifier != 0)
            {
                pieceToEat = !m_Board.IsEmpty((moves[0] + moves[2]) / 2, (moves[1] + moves[3]) / 2) &&
                    m_Board.IsBlack((moves[0] + moves[2]) / 2, (moves[1] + moves[3]) / 2) == m_isFirstPlayerPlaying;
            }
            Boolean legelKingMovement = false;
            if (m_Board.IsKing(moves[0], moves[1]) && i_kingModifer==1)
            {
                legelKingMovement = IsLegalMove(i_move, i_eatingModifier, - 1);
            }
            return (legelKingMovement || legalPositon) && legalColor && pieceToEat;
        }
        public List<string> AlllegalmMovesFromASquareByCommand(string i_move, int i_eatingModifier = 0)
        {
            int[] indexes = ProcessCommand(i_move);
            return AlllegalmMovesFromASquareByIndex(indexes[0], indexes[1], i_eatingModifier);
        }
        public  List<string> AlllegalmMovesFromASquareByIndex(int i_column, int i_row, int i_eatingModifier = 0)
        {
            int movmentDirection;
            List<string> legalMoves = new List<string>();
            if (m_isFirstPlayerPlaying)
            {
                movmentDirection = (-1 - i_eatingModifier);
            }
            else
            {
                movmentDirection = (1 + i_eatingModifier);
            }
            string optinalMoveRight = ProcessIndexesToCommand(i_column, i_row, i_column + movmentDirection, i_row + movmentDirection);
            if (IsLegalCommand(optinalMoveRight) && IsLegalMove(optinalMoveRight, i_eatingModifier))
            {
                legalMoves.Add(optinalMoveRight);
            }
            string optinalMoveLeft = ProcessIndexesToCommand(i_column, i_row, i_column - movmentDirection, i_row + movmentDirection);

            if (IsLegalCommand(optinalMoveLeft) && IsLegalMove(optinalMoveLeft, i_eatingModifier))
            {
                legalMoves.Add(optinalMoveLeft);
            }
            string optinalMoveBackRight = ProcessIndexesToCommand(i_column, i_row, i_column + movmentDirection, i_row - movmentDirection);

            if (IsLegalCommand(optinalMoveBackRight) && IsLegalMove(optinalMoveBackRight, i_eatingModifier))
            {
                legalMoves.Add(optinalMoveBackRight);
            }
            string optinalMoveBackLeft = ProcessIndexesToCommand(i_column, i_row, i_column - movmentDirection, i_row - movmentDirection);
            if (IsLegalCommand(optinalMoveBackLeft) && IsLegalMove(optinalMoveBackLeft, i_eatingModifier))
            {
                    legalMoves.Add(optinalMoveBackLeft);
            }
            return legalMoves;
        }
        public List<string> AllLegalRegularMoves(int i_eatingModifier = 0)
        {
            List<string> allMoves = new List<string>();
            for(int i=0; i<m_Board.GetSizeOfBoard(); i++)
            {
                for (int j = 0; j < m_Board.GetSizeOfBoard(); j++)
                {
                    List<string> newMoves = (AlllegalmMovesFromASquareByIndex(i, j, i_eatingModifier));
                    foreach(string move in newMoves)
                    {
                        allMoves.Add(move);
                    }
                }
            }
            return allMoves;
        }
        public List<string> AllJumpingMoves()
        {
            return AllLegalRegularMoves(1);
        }
        public static int[] ProcessCommand(string i_move)
        {
            int[] moves = new int[4];
            moves[0] = i_move[0] - ('A');
            moves[1] = i_move[1] - ('a');
            moves[2] = i_move[3] - ('A');
            moves[3] = i_move[4] - ('a');
            return moves;
        }
        private static string ProcessIndexesToCommand(int i_sourceColumn,int i_sourceRow , int i_targetColumn, int i_targetRow)
        {
            string move = "";
            move += (char)(i_sourceColumn + 'A');
            move += (char)(i_sourceRow + 'a');
            move += '>';
            move += (char)(i_targetColumn + 'A');
            move += (char)(i_targetRow + 'a');
            return move;
        }
        public static Boolean IsJumpingMove(string i_move)
        {
            int[] moves = ProcessCommand(i_move);
            return Math.Abs(moves[1] - moves[3]) == 2;
        }
        public Boolean noLegalMoves()
        {
            return AllLegalRegularMoves().Count == 0 && AllJumpingMoves().Count == 0; ;
        }
    }
}
