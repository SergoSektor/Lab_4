﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CardGame
{
    [DataContract]
    /// <summary> Класс определяющий действия игрока на игровом поле </summary>
    public class GameBoardEvents: GameBoard
    {
        /// <summary> Новый раунд игры </summary>
        public void NewRound() {
            if (PlayerCardDeck.Count > 0 && PlayerCards.Count < 4)
            {
                PlayerCards.Add(PlayerCardDeck[0]);
                PlayerCardDeck.RemoveAt(0);
            }

            if (EnemyCardDeck.Count > 0)
            {
                EnemyCards.Add(EnemyCardDeck[0]);
                EnemyCardDeck.RemoveAt(0);
            }

            RoundNumber++;
            Player_MP = Enemy_MP = RoundNumber + 1;

            Player_MP = Enemy_MP = (Enemy_MP > MaxMP) ? (MaxMP):(Enemy_MP);
        }

        /// <summary> Игрок размещается карту </summary>
        public int[] PutCard(int card_number) {

            var selected_card = PlayerCards[card_number - 1];

            if (selected_card.Price > Player_MP)
                throw new Exception("Нехватает MP, чтобы разыграть карту");

            var lines = from GameLine line in gameLines
                        where line.EnemyWarrior.GameStatus == true
                        select line;

            if (lines.Count<GameLine>() == 0 && RoundNumber % 2 == 0)
                throw new Exception("Нет атакующих врагов");


            switch (selected_card.GetCardType)
            {
                case CardType.Warrior: PutWarrior((CardWarrior)selected_card); return null; 
                case CardType.Spell_Attack: return LineNumbers(true); 
                case CardType.Spell_Healing: return LineNumbers(false); 
                case CardType.Spell_Improve: return LineNumbers(false);

                default: return null;
            }
        }

        public void PutCard(int card_number, int index) {

            var selected_card = PlayerCards[card_number - 1];


            if (selected_card.Price > Player_MP)
                throw new Exception("Нехватает MP, чтобы разыграть карту");

            var lines = from GameLine line in gameLines
                        where line.EnemyWarrior.GameStatus == true
                        select line;

            if (lines.Count<GameLine>() == 0 && RoundNumber % 2 == 0)
                throw new Exception("Нет атакующих врагов");

            switch (selected_card.GetCardType)
            {
                case CardType.Spell_Attack: PutSpellAttack((CardSpellAttack)selected_card, index); break;
                case CardType.Spell_Healing: PutSpellHealing((CardSpellHealing)selected_card, index); break;
                case CardType.Spell_Improve: PutImproveSpell((CardSpellImprove)selected_card, index); break;
            }
        }

        private int[] LineNumbers(bool Enemy) {
            List<int> numbers = new List<int>();

            for (int i = 0; i < gameLines.Length; i++)
                if (!((Enemy) ? (gameLines[i].EnemyWarrior) : (gameLines[i].FriendlyWarrior)).GameStatus)
                {
                    numbers.Add(i);
                }

            return numbers.ToArray();
        }

        /// <summary> Игрок размещается существо на поле </summary>
        public void PutWarrior(CardWarrior warrior) {

            for (int i = 0; i < gameLines.Length; i++)
                if (!gameLines[i].FriendlyWarrior.GameStatus)
                {
                    Player_MP -= warrior.Price;
                    gameLines[i].FriendlyWarrior = warrior;
                    PlayerCards.Remove(warrior);
                    return;
                }

            throw new Exception("Нет места для нового бойца");
        }

        /// <summary> Игрок выбрасывает карту, наносящую урон </summary>
        public void PutSpellAttack(CardSpellAttack spell, int index) {

            var active_enemys = new List<CardWarrior>(0);
            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.EnemyWarrior.GameStatus)
                    active_enemys.Add(gameLine.EnemyWarrior);
            }

            if (active_enemys.Count == 0)
                throw new Exception("Нет врагов, чтобы применить заклинание атаки");


            if(!gameLines[index - 1].EnemyWarrior.GameStatus)
                throw new Exception("На данной линии нет враждебных существ существ.");

           
            Player_MP -= spell.Price;
            spell.Damage(active_enemys[index - 1]);
            PlayerCards.Remove(spell);
        }

        /// <summary> Игрок выбрасывает карту, лечащую союзное существо </summary>
        public void PutSpellHealing(CardSpellHealing spell, int index) {
            var friendly_warriors = new List<CardWarrior>(0);

            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.FriendlyWarrior.GameStatus)
                    friendly_warriors.Add(gameLine.FriendlyWarrior);
            }

            if (friendly_warriors.Count == 0)
                throw new Exception("Нет бойцов, чтобы применить заклинание");

            if (!gameLines[index - 1].FriendlyWarrior.GameStatus)
                throw new Exception("На данной линии нет дружественных существ.");

            Player_MP -= spell.Price;
            spell.Treatment(friendly_warriors[index - 1]);
            PlayerCards.Remove(spell);

        }

        /// <summary> Игрок выбрасывает карту улучшение </summary>
        public void PutImproveSpell(CardSpellImprove spell, int index) {
            var line_for_spell = new List<GameLine>(0);
            foreach (GameLine gameLine in gameLines)
            {
                if (gameLine.FriendlyWarrior.GameStatus)
                    line_for_spell.Add(gameLine);
            }

            if (line_for_spell.Count == 0)
                throw new Exception("Нет бойцов, чтобы применить заклинание");

            if (!gameLines[index - 1].FriendlyWarrior.GameStatus)
                throw new Exception("На данной линии нет дружественных существ.");


            Player_MP -= spell.Price;
            spell.Modification(line_for_spell[index - 1].FriendlyWarrior);
            PlayerCards.Remove(spell);

        }
    }
}
