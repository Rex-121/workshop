using System;
using Tyrant;
using UniRx;

namespace WorkBench
{
    public static class WorkBenchManagerExt
    {

        public static IObservable<WorkBenchManager.CheckerPack> CheckerPackStatus(this WorkBenchManager o)
        {
            return o.cardInHandStream
                .CombineLatest(o.checker, (card, checkerboard)
                    => new WorkBenchManager.CheckerPack(card, checkerboard));
        }
        
    }
}