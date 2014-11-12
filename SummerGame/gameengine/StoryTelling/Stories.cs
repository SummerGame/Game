using System.Collections.Generic;

namespace GameEngine.StoryTelling
{
    /// <summary>
    /// Класс "Истории"
    /// Предназначен, как заготовка для Storyboard WPF.
    /// Имеет свой набор историй для юнитов и итемов по времени.
    /// </summary>
    public class Stories
    {
        #region Fields
        private List<Story> stories = new List<Story>();

        #endregion

        #region Properties

        public Story this[int i]
        {
            get { return stories[i]; }
        }

        public List<Story> GetStories
        {
            get { return stories; }
        }

        #endregion
    }
}
