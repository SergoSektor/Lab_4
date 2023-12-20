using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CardGame
{
    [DataContract]
    /// <summary> Игровая логика </summary>
    public class Controller
    {
        [DataMember]
        private GameManager game; // Экземпляр игрового менеджера

        private EnemyAI enemyAI; // Экземпляр искусственного интеллекта противника

        [DataMember]
        public bool WaitFlag { get; set; } // Флаг ожидания хода

        [DataMember]
        public bool BattleFlag { get; set; } // Флаг битвы

        [DataMember]
        private string logs; // Логи игры

        // Метки для отображения информации о раунде, здоровье и мане противника, здоровье и мане игрока
        private Label round_number;
        private Label round_status;
        private Label enemyHP;
        private Label enemyMP;
        private Label playerHP;
        private Label playerMP;

        private Button[] card_buttons; // Кнопки для карт в руке игрока
        private Canvas[] card_canvas; // Холсты для отображения карт в руке игрока
        private Canvas[] border; // Границы, отображающие карты на поле

        private CardDraw cardDraw; // Объект, отвечающий за отрисовку карт

        private Label footnote; // Метка для отображения сноски

        private Image[] stage_images; // Изображения для отображения текущего этапа игры

        [DataMember]
        private int stage; // Текущий этап игры

        public Controller(Label RoundNumber, Label RoundStatus, Label EnemyHP, Label EnemyMP, Label PlayerHP, Label PlayerMP, Button[] buttons, Canvas[] canvas, Label label, Canvas[] canvas1, Image[] image_stages)
        {
            game = new GameManager(); // Инициализация игрового менеджера
            enemyAI = new EnemyAI(game); // Инициализация искусственного интеллекта противника
            Debug.DebugInit(game); // Инициализация отладочных данных
            cardDraw = new CardDraw(); // Инициализация объекта для отрисовки карт

            logs = "";

            round_number = RoundNumber;
            round_status = RoundStatus;
            enemyHP = EnemyHP;
            enemyMP = EnemyMP;
            playerHP = PlayerHP;
            playerMP = PlayerMP;

            card_buttons = buttons;
            card_canvas = canvas;
            border = canvas1;

            footnote = label;
            stage_images = image_stages;

            stage = 0;
            RoundStage(); // Установка начального этапа игры

            GameOutPut(); // Вывод начального состояния игры
        }

        public void GameOutPut()
        {
            round_number.Content = "Round:" + game.RoundNumber.ToString();
            round_status.Content = game.GameStatus();

            enemyHP.Content = game.Enemy_HP.ToString();
            enemyMP.Content = game.Enemy_MP.ToString();

            playerHP.Content = game.Player_HP.ToString();
            playerMP.Content = game.Player_MP.ToString();

            BorderStatusUpdate();
            PlayerHand();
        }

        public void PlayerHand()
        {
            List<Card> list = game.PlayerCards;

            for (int i = 0; i < card_canvas.Length; i++)
                card_canvas[i].Children.Clear();


            for (int i = 0; i < card_buttons.Length; i++)
                card_buttons[i].Visibility = Visibility.Hidden;

            for (int i = 0; i < list.Count; i++)
            {
                cardDraw.DrawSprite(card_canvas[i], list[i]);
                card_buttons[i].Visibility = Visibility.Visible;
            }
        }


        public int[] PlayCard(int number)
        {
            if (game.RoundNumber % 2 != 0 && stage > 0)
                throw new Exception("На данном этапе атаковать нельзя");

            if (game.PlayerCards.Count == 0)
                throw new Exception("В руке нет карт");

            if (game.PlayerCards.Count < number)
                throw new Exception($"Ошибка - индекс неверен {number}");

            return game.PutCard(number);
        }

        public void PlayCard(int number, int line)
        {

            try
            {
                if (game.PlayerCards.Count == 0)
                    throw new Exception("В руке нет карт");

                if (game.PlayerCards.Count < number)
                    throw new Exception($"Ошибка - индекс неверен {number}");

                game.PutCard(number, line);
            }
            catch (Exception ex)
            {
                footnote.Content = ex.Message;
            }
        }

        public void Skip()
        {

            if (enemyAI.GameMove())
            {
                if (WaitFlag)
                {
                    WaitFlag = false;
                    return;
                }

                stage = stage % 3;

                switch (stage)
                {

                    case 0: game.RoundBegin(); GameOutPut(); stage++; break;
                    case 1: game.RoundStart(); GameOutPut(); stage++; break;
                    case 2: game.RoundEnd(); GameOutPut(); stage++; game.NewRound(); RoundStage(); break;

                }
            }

            GameOutPut();
        }

        public void RoundStage()
        {
            stage = stage % 3;

            for (int i = 0; i < stage_images.Length; i++)
                stage_images[i].Visibility = Visibility.Hidden;

            for (int i = 0; i < stage + 1; i++)
                stage_images[i].Visibility = Visibility.Visible;
        }

        private void BorderStatusUpdate()
        {
            GameLine[] gameLines = game.BorderStatus();
            for (int i = 0; i < border.Length; i++)
                border[i].Children.Clear();

            for (int i = 0; i < gameLines.Length; i++)
            {
                cardDraw.DrawSprite(border[i * 2], gameLines[i].EnemyWarrior);
                cardDraw.DrawSprite(border[i * 2 + 1], gameLines[i].FriendlyWarrior);
            }
        }

        public void Save()
        {
            logs = Debug.SavingLogs();
            game.Save();
        }

        public void Load(Label RoundNumber, Label RoundStatus, Label EnemyHP, Label EnemyMP, Label PlayerHP, Label PlayerMP, Button[] buttons, Canvas[] canvas, Label label, Canvas[] canvas1, Image[] image_stages)
        {
            Debug.LoadLogs(logs);
            game.Load();
            Debug.DebugInit(game);
            enemyAI = new EnemyAI(game);
            cardDraw = new CardDraw();

            round_number = RoundNumber;
            round_status = RoundStatus;
            enemyHP = EnemyHP;
            enemyMP = EnemyMP;
            playerHP = PlayerHP;
            playerMP = PlayerMP;

            card_buttons = buttons;
            card_canvas = canvas;
            border = canvas1;

            footnote = label;

            stage_images = image_stages;

            RoundStage();
        }
    }
}