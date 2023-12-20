using System.Runtime.Serialization;

namespace CardGame
{
    [DataContract]
    /// <summary> Заклинание улучшения </summary>
    public class CardSpellImprove : Card
    {
        [DataMember]
        public int attack_mod { get; set; } // Модификатор атаки

        [DataMember]
        public int health_mod { get; set; } // Модификатор здоровья

        [DataMember]
        public Effects effect_mod { get; set; } // Модификатор эффекта

        public CardSpellImprove()
        {
            Price = 0; // Цена карты
            DebugOff = false; // Переключатель отладочного режима
        }

        public CardSpellImprove(int price, string name, int attack_mod_value, int health_mod_value, string file_image)
        {
            Price = price; // Цена карты
            Name = name; // Имя карты

            attack_mod = attack_mod_value; // Модификатор атаки
            health_mod = health_mod_value; // Модификатор здоровья

            effect_mod = Effects.Null; // Модификатор эффекта (по умолчанию Null)

            sprite_file = file_image; // Путь к изображению карты
            DebugOff = false; // Переключатель отладочного режима
        }

        public CardSpellImprove(int price, string name, int attack_mod_value, int health_mod_value, Effects effect, string file_image)
        {
            Price = price; // Цена карты
            Name = name; // Имя карты

            attack_mod = attack_mod_value; // Модификатор атаки
            health_mod = health_mod_value; // Модификатор здоровья

            effect_mod = effect; // Модификатор эффекта

            sprite_file = file_image; // Путь к изображению карты
            DebugOff = false; // Переключатель отладочного режима
        }

        public CardSpellImprove(int price, string name, Effects effect, string file_image)
        {
            Price = price; // Цена карты
            Name = name; // Имя карты

            effect_mod = effect; // Модификатор эффекта

            attack_mod = 0; // Нулевой модификатор атаки
            health_mod = 0; // Нулевой модификатор здоровья

            sprite_file = file_image; // Путь к изображению карты
            DebugOff = false; // Переключатель отладочного режима
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Spell_Improve; // Получение типа карты (заклинание улучшения)
            }
        }

        public void Modification(CardWarrior friendly_warrior)
        {

            if (!DebugOff)
                Debug.Log($"Карта '{Name}' модификация {friendly_warrior.CardStatus()} на величину {attack_mod}/{health_mod}"); // Вывод информации о модификации

            friendly_warrior.Modification(attack_mod, health_mod); // Применение модификации на воине

            if (effect_mod != Effects.Null)
                friendly_warrior.Effect = effect_mod; // Применение эффекта модификации, если он задан
        }

        public override string CardStatus()
        {
            string effect = string.Empty;
            switch (effect_mod)
            {
                case Effects.Hardness: effect = "И Стойкость"; break; // Если эффект модификации - стойкость
                case Effects.Upgradable: effect = "И Грабитель"; break; // Если эффект модификации - грабитель
                case Effects.Finite: effect = "Но Выгорание"; break; // Если эффект модификации - выгорание
            }

            return base.CardStatus() + $"{effect}"; // Получение статуса карты (включая эффект модификации)
        }
    }
}
