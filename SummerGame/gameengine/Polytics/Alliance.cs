using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameEngine
{
    /// <summary>
    /// Call it a block, an alliance or whatever -- a bunch of countries temporary fighting together
    /// </summary>
    public class Alliance
    {
        private Countries[] members;

        /// <summary>
        /// Создаёт кампанию, состоящую из стран списка members
        /// </summary>
        /// <param name="members"></param>
        public Alliance(params Countries[] members)
        { 
            this.members = members; 
        }

    }
}
