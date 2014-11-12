using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Characters
{
    /// <summary>
    /// Перечислимый тип родов войск, к которым относятся все войска в игре
    /// </summary>
    public enum UnitType
    {
        Infantry,   // Пехота / общевойсковые соединения и объедиенния
        Engineers,  // Инженерные войска
        Cavalry,    // Кавалерия
        Artillery,  // Артиллерия
        Armor,      // Танковые войска
        AirForce,   // Авиация
        Navy        // Флот
    }
}
