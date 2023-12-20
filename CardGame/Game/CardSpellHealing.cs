using System.Runtime.Serialization;

namespace CardGame
{
    [DataContract]
    /// <summary> Заклинание лечения </summary>
    public class CardSpellHealing : Card
    {
        [DataMember]
        public int treatment { get; set; } // Лечение (значение)

        public CardSpellHealing(int price, string name, int treatment_value, string file_image)
        {
            Price = price; // Цена карты
            Name = name; // Имя карты
            treatment = treatment_value; // Лечение (значение)

            sprite_file = file_image; // Путь к изображению карты
            DebugOff = false; // Переключатель отладочного режима
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Spell_Healing; // Получение типа карты (заклинание лечения)
            }
        }

        public void Treatment(CardWarrior cardWarrior)
        {
            if (!DebugOff)
                Debug.Log($"Карта '{Name}' лечение {cardWarrior.CardStatus()} на величину {treatment}"); // Вывод информации о лечении картой лечения
            cardWarrior.Treatment(treatment); // Вызов метода Treatment() для применения лечения на карте воина
        }

        public override string CardStatus()
        {
            return base.CardStatus() + $"Карта лечения '{Name}' Лечение: {treatment}"; // Получение статуса карты (включая имя и лечение)
        }
    }
}