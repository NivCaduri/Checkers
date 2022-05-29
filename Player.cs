using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_Ex02_Niv_311451694_Goody_206591810
{
    internal class Player
    {
        private string m_name;
        private LegalTools m_rules;
        private int m_score;

        public Player(string i_name)
        {
            m_name = i_name;
            m_score = 0;
        }
        public  void addToScore(int i_newScore)
        {
            m_score += i_newScore;
        }
        public int getScore()
        {
            return m_score;
        }
        public Player(LegalTools i_rules)
        {
            m_name = "computer";
            m_rules = i_rules;
            m_score = 0;
        }
        public string playComputerMove()
        {
            string move = "";
            Random rand = new Random();
            List<string> listOfJumpingMoves = m_rules.AllJumpingMoves();
            List<string> listOfregularMoves = m_rules.AllLegalRegularMoves();
            if (listOfJumpingMoves.Count != 0)
            {
                move = listOfJumpingMoves[rand.Next(listOfJumpingMoves.Count)];
            }
            else if (listOfregularMoves.Count != 0)
            {
                move = listOfregularMoves[rand.Next(listOfregularMoves.Count)];
            }
            return move;
        }
        public string playComputerJumpingMove(int i_column, int i_row)
        {
            List<string> possibleEatingMoves = m_rules.AlllegalmMovesFromASquareByIndex(i_column, i_row, 1);
            Random rand = new Random();
            int randomMove = rand.Next(possibleEatingMoves.Count);
            Console.WriteLine(m_rules.AlllegalmMovesFromASquareByIndex(i_column, i_row, 1).Count);
            string move = m_rules.AlllegalmMovesFromASquareByIndex(i_column, i_row, 1)[randomMove];
            return move;
        }
        public string Name
        {
            get { return m_name; }
        }
    }
}
