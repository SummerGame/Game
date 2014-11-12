using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine.Characters
{
    /// <summary>
    /// Организационный уровень войск (например, батальон, полк, дивизия)
    /// </summary>
    public enum OrganizationLevel
    {
        Battalion,  // батальон / дивизион / эскадрон
        Regiment,   // полк / группа
        Brigade,    // бригада
        Division,   // дивизия
        Corps,      // корпус
        Army,       // армия
        ArmyGroup   // фронт / группа армий
    }
}
