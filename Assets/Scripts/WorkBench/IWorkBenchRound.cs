namespace Tyrant
{
    public interface IWorkBenchRound
    {

        public void PrepareNewRound();

        public void DidEndRound();

        public void NewTurn();

    }
}