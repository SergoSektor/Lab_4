using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace CardGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Effects
    {
        Null,           // Нет эффекта
        Hardness,       // Стойкость
        Upgradable,     // Грабитель
        Finite          // Выгорание
    }

    [DataContract]
    /// <summary> Карта существо </summary>
    public class CardWarrior : Card
    {
        [DataMember]
        private int attack;         // Значение атаки

        [DataMember]
        private int health;         // Значение здоровья
        [DataMember]
        private int max_health;     // Максимальное значение здоровья

        [DataMember]
        private bool alive;         // Флаг жизни

        [DataMember]
        public Effects Effect { get; set; } // Эффект карты

        public CardWarrior()
        {
            attack = 0;
            max_health = 0;
            health = 0;

            Effect = Effects.Null;
            alive = false;

            sprite_file = "CardNull.png";
            DebugOff = false;
        }

        public CardWarrior(int price, string name, int attack_value, int max_healt_value, Effects warrior_effects, string file_image)
        {
            Price = price;
            Name = name;
            attack = attack_value;
            max_health = max_healt_value;
            health = max_healt_value;

            Effect = warrior_effects;
            alive = true;

            sprite_file = file_image;
            DebugOff = false;
        }

        public override CardType GetCardType
        {
            get
            {
                return CardType.Warrior;
            }
        }

        public bool GameStatus
        {
            get { return alive; }
        }

        public void Death()
        {
            if (!DebugOff)
                Debug.Log($"{CardStatus()} смерть");

            alive = false;
            attack = 0;
            Effect = Effects.Null;
            health = 0;
        }

        public int AttackPoints
        {
            get { return attack; }
            set { attack = (value <= 0) ? (0) : (value); }
        }

        public void Modification(int attack_mod, int health_mod)
        {
            AttackPoints += attack_mod;

            max_health += health_mod;
            health += health_mod;
        }

        public void Damage(int damage_value)
        {
            if (!DebugOff)
                Debug.Log($"'{Name}' получает урон - {damage_value}");
            health -= damage_value;

            if (Effect == Effects.Hardness)
                health += 1;

            if (health <= 0)
                Death();
        }

        public void Treatment(int treatment_value)
        {
            health += treatment_value;

            if (health > max_health)
                health = max_health;
        }

        public void AttackStatus()
        {
            switch (Effect)
            {
                case Effects.Upgradable: AttackPoints++; break;
                case Effects.Finite: Damage(1); AttackPoints--; break;
                default: break;
            }
        }

        public void DefendStatus()
        {
            switch (Effect)
            {
                case Effects.Finite: Damage(1); break;
                default: break;
            }
        }

        public override string CardStatus()
        {
            string effect = string.Empty;
            switch (Effect)
            {
                case Effects.Hardness: effect = "Эффект: Стойкость"; break;
                case Effects.Upgradable: effect = "Эффект: Грабитель"; break;
                case Effects.Finite: effect = "Эффект: Выгорание"; break;
            }
            return base.CardStatus() + $"{effect}";
        }

        public int Health
        {
            get { return health; }
        }
    }
}
