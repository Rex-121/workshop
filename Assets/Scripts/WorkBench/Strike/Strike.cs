using Sirenix.OdinInspector;

namespace Tyrant
{
    public enum Strike
    {
        [LabelText("塑形")]
        Shape,
        [LabelText("品质")]
        Quality
    }

    public static class StrikeExtension
    {
        public static string Description(this Strike o)
        {
            return o switch
            {
                Strike.Shape => "制作",
                Strike.Quality => "品质",
                _ => ""
            };
        }
    }

    public class StrikePower
    {
        [HideLabel, BoxGroup("Strike", ShowLabel = false), HorizontalGroup("Strike/A")]
        public Strike strike;
        
        [HideLabel, BoxGroup("Strike", ShowLabel = false), HorizontalGroup("Strike/A")]
        public int power;

        public StrikePower(Strike strike, int p)
        {
            this.strike = strike;
            power = p;
        }

        public string debugDescription => $"{strike}->{power}<-";
        public static StrikePower operator +(StrikePower a, StrikePower b)
        {
            if (a.strike != b.strike) return a;
            a.power += b.power;
            return a;
        }
    }
}