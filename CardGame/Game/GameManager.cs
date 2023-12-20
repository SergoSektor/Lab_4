using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CardGame
{
    [DataContract]
    /// <summary> Класс определяющий вывод данных об игре </summary>
    public class GameManager : GameBoardEvents
    {
        public GameManager()
        {
            Player_HP = Enemy_HP = 10;
            Player_MP = Enemy_MP = 0;

            PlayerCardDeck = CardsManager.GetPlayerDeck();
            EnemyCardDeck = CardsManager.GetEnemyDeck();

            PlayerCards = new List<Card>(0);
            EnemyCards = new List<Card>(0);

            RoundNumber = 0;

            gameLines = new GameLine[5] {
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
                new GameLine(true),
            };

            NewRound();
        }

        public GameLine[] BorderStatus()
        {
            return gameLines;
        }

        public string GameStatus()
        {
            return $"{((RoundNumber % 2 != 0) ? ("Вы атакуете"):("Вы защищаетесь"))}";
        }


        public List<Card> PlayerHandStatus()
        {
            return PlayerCards;
        }
    }
}
