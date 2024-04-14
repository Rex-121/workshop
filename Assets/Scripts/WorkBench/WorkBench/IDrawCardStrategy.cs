using System;
using UniRx;

namespace Tyrant
{
    public interface IDrawCardStrategy
    {
        public int amount { get; }
        
        IObservable<IDrawCardStrategy> readyToDraw { get; }

    }


    public readonly struct DrawImmediately : IDrawCardStrategy
    {
        public int amount { get; }

        public DrawImmediately(int amount)
        {
            this.amount = amount;
        }
        public IObservable<IDrawCardStrategy> readyToDraw => Observable.Return(this as IDrawCardStrategy);
    }



    public readonly struct DelayDraw : IDrawCardStrategy
    {

        public int amount { get; }
        
        private readonly TimeSpan _timeSpan;

        public DelayDraw(int amount, TimeSpan s)
        {
            _timeSpan = s;
            this.amount = amount;
        }
        public IObservable<IDrawCardStrategy> readyToDraw
        {
            get
            {
                var draw = this as IDrawCardStrategy;
                return Observable
                    .Timer(_timeSpan)
                    .Select(v => draw)
                    .Take(1);
            }
        }
    }
    
}