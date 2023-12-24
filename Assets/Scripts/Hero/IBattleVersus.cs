namespace Tyrant
{
    public interface IBattleVersus
    {
        public Attack Attack(IBattleVersus battleVersus);


        public void TakeDamage(Attack attack);


        public Health health { get; }

        public HeroActionQueue actionQueue { get;  }
    }
}