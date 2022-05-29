using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B22_Ex02_Niv_311451694_Goody_206591810
{
    internal class Game
    {
        private Player m_firstPlayer;
        private Player m_secondPlayer;
        private CheckersBoard m_board;
        private LegalTools m_rules;
        private Boolean m_gameVsComputer;

        public  Game()
        {
            Console.WriteLine("Hello, please enter your name");
            string firstPlayerName = Console.ReadLine();
            while (!(LegalTools.IsLegalName(firstPlayerName)))
            {
                Console.WriteLine("Name is invalid, please enter a valid name");
                firstPlayerName = Console.ReadLine();
            }
            m_firstPlayer = new Player(firstPlayerName);
            Console.WriteLine("Please enter your desired size board");
            string sizeBoard = Console.ReadLine();
            while(!(LegalTools.IsLegalSize(sizeBoard)))
            {
                Console.WriteLine("Size is invalid, please enter a valid Size");
                sizeBoard = Console.ReadLine();
            }
            m_board = new CheckersBoard(int.Parse(sizeBoard));
            m_rules = new LegalTools(m_board);
            Console.WriteLine("type 'computer' to play against the computer or 'player' to play against another player");
            string firstPlayerChoice = Console.ReadLine();
            while (!(firstPlayerChoice.Equals("computer") || firstPlayerChoice.Equals("player")))
            {
                Console.WriteLine("Input invalid, please try again");
                firstPlayerChoice = Console.ReadLine();
            }
            if (firstPlayerChoice.Equals("computer"))
            {
                m_secondPlayer = new Player(m_rules);
                m_gameVsComputer = true;
            }
            else if (firstPlayerChoice.Equals("player"))
            {
                Console.WriteLine("Please enter second player name");
                string secondPlayerName = Console.ReadLine();
                while (!(LegalTools.IsLegalName(secondPlayerName)))
                {
                    Console.WriteLine("Name is invalid, please enter a valid name");
                    secondPlayerName = Console.ReadLine();
                }
                m_secondPlayer = new Player(secondPlayerName);
                m_gameVsComputer = false;
            }
        }
        public void playGame()
        {
            Boolean firstTurn = true;
            string move = "";
            while (!(m_rules.noLegalMoves() || m_board.gameEmpty()))
            {
                Ex02.ConsoleUtils.Screen.Clear();
                m_board.ShowBoard();
                if (m_gameVsComputer && !m_rules.GetIsFirstPlayerPlaying())
                {
                     move = m_secondPlayer.playComputerMove();
                    Console.WriteLine(move);
                }
                else
                {
                    if (firstTurn)
                    {
                        Console.WriteLine("Please enter your move");
                    }
                    else if (!m_rules.GetIsFirstPlayerPlaying())
                    {
                        Console.WriteLine(m_firstPlayer.Name + "'s move was: " + move);
                        Console.WriteLine(m_secondPlayer.Name + "'s Turn: ");
                    }
                    else
                    {
                        Console.WriteLine(m_secondPlayer.Name + "'s move was: " + move);
                        Console.WriteLine(m_firstPlayer.Name + "'s Turn: ");
                    }
                    firstTurn = false;  
                    move = Console.ReadLine();
                    if (move.Equals("Q"))
                    {
                        if (m_board.gameScore() > 0)
                        {
                            m_firstPlayer.addToScore(m_board.gameScore());
                        }
                        else
                        {
                            m_secondPlayer.addToScore(m_board.gameScore());
                        }
                        break;
                    }
                }
                while (!m_rules.CheckIfMoveIsLegelForGame(move))
                {
                    Console.WriteLine("move invalid, please enter another move");
                    move = Console.ReadLine();
                }
                m_board.playMove(move, LegalTools.IsJumpingMove(move));
                int[] index_of_moves = LegalTools.ProcessCommand(move);
                if (LegalTools.IsJumpingMove(move))
                {
                    while (m_rules.AlllegalmMovesFromASquareByIndex(index_of_moves[2], index_of_moves[3], 1).Count != 0)
                    {
                        if ((m_gameVsComputer && !m_rules.GetIsFirstPlayerPlaying()))
                        {
                            move = m_secondPlayer.playComputerJumpingMove(index_of_moves[2], index_of_moves[3]);
                        }
                        else
                        {
                            Console.WriteLine("Please enter a jumping move");
                            move = Console.ReadLine();
                        }
                        while (!(LegalTools.IsJumpingMove(move) && m_rules.CheckIfMoveIsLegelForGame(move)))
                        {
                            Console.WriteLine("move invalid, please enter another jumping move");
                            move = Console.ReadLine();
                        }
                        Ex02.ConsoleUtils.Screen.Clear();
                        m_board.ShowBoard();
                        m_board.playMove(move, LegalTools.IsJumpingMove(move));
                    }
                }
                m_rules.advanceTheTurn();
            }
            if(!m_rules.noLegalMoves())
            {
                if (m_board.gameScore() > 0)
                {
                    m_firstPlayer.addToScore(m_board.gameScore());
                }
                else
                {
                    m_secondPlayer.addToScore(m_board.gameScore());
                }
            }
            this.nextGame();
        }
        public void nextGame()
        {
            Console.WriteLine("would you like to play another game? type yes or no");
            String answerAboutNextGame = Console.ReadLine();
            while(!(answerAboutNextGame.Equals("yes") || answerAboutNextGame.Equals("no")))
            {
                Console.WriteLine("input invalid, please type yes or no");
                answerAboutNextGame = Console.ReadLine();
            }
            if (answerAboutNextGame.Equals("yes"))
            {
                m_board = new CheckersBoard(m_board.GetSizeOfBoard());
                this.playGame();
            }
        }
    }
}
