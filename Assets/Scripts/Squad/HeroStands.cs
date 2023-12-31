using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
namespace Tyrant
{
    public class HeroStands: MonoBehaviour
    {
        [ShowInInspector]
        private Hero _hero;
        
        public Hero hero
        {
            set
            {
                _hero = value;
                _sprites = _hero.characterSO.sprites;
                _idleCount = _sprites.Length;
                Refresh();
            }
            get => _hero;
        }

        private Sprite[] _sprites;

        public Image image;

        public float timeSpan = 0.2f;
        
        private void Refresh()
        {
            image.sprite = _sprites.FirstOrDefault();
            Observable.Interval(TimeSpan.FromSeconds(timeSpan))
                .Select(_ => GetIndex())
                .Subscribe(v => image.sprite = _sprites[v])
                .AddTo(this);
        }

        private int _index = 0;
        private int _idleCount;
        private int GetIndex()
        {
            var value = _index % _idleCount;
            _index++;
            if (value == _idleCount - 1)
            {
                _index = 0;
            }
            return value;
        }
        
    }
}