using Tyrant.Items;

namespace Tyrant
{
    public interface IMaterial: IItem
    {
        public MaterialType type { get; }
        
    }
}