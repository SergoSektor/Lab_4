using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CardGame
{
    /// <summary> Заклинание атаки </summary>
    [DataContract]
    public class CardSpellAttack : Card
    {
        [DataMember]
        public int damage { get; set; }

        public CardSpellAttack(int price, string name, int damage_value, string file_image)
        {
            Price = price; // Цена карты
            Name = name; // Имя карты
            damage = damage_value; // Урон

            sprite_file = file_image; // Путь к изображению карты

            DebugOff = false; // Переключатель отладочного режима
        }

        public void Damage(CardWarrior cardWarrior)
        {
            if (!DebugOff)
                Debug.Log($"Карта атаки '{Name}' нанесен урон {cardWarrior.CardStatus()} в размере {damage}"); // Вывод информации о нанесении урона картой атаки
            cardWarrior.Damage(damage); // Вызов метода Damage() для причинения урона на карте воина
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Spell_Attack; // Получение типа карты (заклинание атаки)
            }
        }

        public override string CardStatus()
        {
            return base.CardStatus() + $"Карта атаки '{Name}' Урон: {damage}"; // Получение статуса карты (включая имя и урон)
        }
    }
}