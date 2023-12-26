
#if UNITY_EDITOR

namespace Tyrant.Editor
{
    using System.Linq;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    public class DungeonsTable
    {
        
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<DungeonSO> _allDungeonSos;
        
        public DungeonsTable(IEnumerable<DungeonSO> characters)
        {
            this._allDungeonSos = characters.ToList();//.Select(x => new CharacterWrapper(x)).ToList();
        }
    }
    
    
    
    public class BuffTable
    {
        
        [TableList(IsReadOnly = true, AlwaysExpanded = true), ShowInInspector]
        private readonly List<BuffDataSO> _allBuffs;
        
        public BuffTable(IEnumerable<BuffDataSO> characters)
        {
            this._allBuffs = characters.ToList();//.Select(x => new CharacterWrapper(x)).ToList();
        }
    }
}

#endif