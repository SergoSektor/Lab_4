using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CardGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardType
    {
        CardNull,                       // Пустая карта
        Warrior,                        // Воин
        Spell_Improve,                  // Заклинание улучшения
        Spell_Attack,                   // Заклинание атаки
        Spell_Healing                   // Заклинание исцеления
    }

    [DataContract]
    public class Card
    {
        [DataMember]
        public string Name { get; set; }                    // Название карты

        [DataMember]
        public int Price { get; set; }                      // Цена карты

        [DataMember]
        /// <summary> Отключение вывода статистики </summary>
        public bool DebugOff { get; set; }                  // Флаг отключения вывода отладочной информации

        [DataMember]
        public string sprite_file { get; set; }             // Имя файла спрайта для карты

        public virtual CardType GetCardType
        {
            get { return CardType.CardNull; }               // Получение типа карты
        }

        public virtual string CardStatus()
        {
            return $"";                                     // Возвращает статус карты (пустая реализация)
        }

        public Card()
        {
            Price = 0;                                      // Конструктор по умолчанию для карты, устанавливает цену в 0
        }
    }
}
