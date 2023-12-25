using System;
using Unity.VisualScripting;

namespace Tyrant
{
    public interface IBuffModule
    {
        public void Apply(BuffInfo buffInfo, Attack attack = new Attack(), Action<Attack> attackHandler = null);
    }
    
    
    
}