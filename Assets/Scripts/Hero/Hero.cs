using System;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    [RequireComponent(typeof(HeroRequest))]
    public class Hero : MonoBehaviour
    {
        
        private HeroRequest _heroRequest;
        public GameObject healthBar;
        public Attribute attribute;

        public string heroName;

        private BehaviorSubject<bool> _isOnAnaAdventure = new(false);

        public HeroActionQueue actionQueue;
        private void Awake()
        {
            _heroRequest = GetComponent<HeroRequest>();
            actionQueue = GetComponent<HeroActionQueue>();
        }

        private void Start()
        {
            _isOnAnaAdventure
                .Subscribe(v =>
            {
                healthBar.SetActive(v);
                _heroRequest.gameObject.SetActive(!v);
            }).AddTo(this);
        }
    }
}
