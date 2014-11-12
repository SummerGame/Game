using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using GameEngine.Characters;
using UserInterface.Presenters;

namespace UserInterface
{
    [ValueConversion(typeof(UnitPresenter), typeof(String))]
    /// <summary>
    /// Конвертирует информацию об отряде
    /// </summary>
    public class LanguageConverter
    {
    }
}
