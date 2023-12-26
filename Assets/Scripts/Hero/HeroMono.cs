using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    [RequireComponent(typeof(HeroRequest))]
    public class HeroMono : MonoBehaviour, IAmHero
    {
        
        private HeroRequest _heroRequest;
        public TextMeshProUGUI healthBar;

        [ShowInInspector]
        private Hero _hero;

        public Attribute attribute => _hero.attribute;

        public string heroName => _hero.heroName;

        public IAmHero heroic => _hero;

        public Health health => _hero.health;

        public bool stillAlive => _hero.stillAlive;

        public DOTweenAnimation doTweenAnimation;
        public TextMeshProUGUI attackLabel;
        public TextMeshProUGUI nameLabel;

        private BehaviorSubject<bool> _isOnAnaAdventure = new(true);

        public HeroActionQueue actionQueue => _hero.actionQueue;
        private void Awake()
        {
            _heroRequest = GetComponent<HeroRequest>();
        }

        public void RestoreFromSO(JobSO jobSO)
        {
            _hero = Hero.FromSO(jobSO);
            
            // healthBar.text = _hero.health.healthBarDisplay;

            nameLabel.text = _hero.heroName;
        }

        private void OnEnable()
        {
            attackLabel.color = Color.clear;
        }


        private void Start()
        {

            heroic.health.healthBarDisplay.Subscribe(v =>
            {
                healthBar.text = v;
            }).AddTo(this);
        }

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = _hero.Attack(battleVersus);
            PlayAttackAnimation(attack);
            return attack;
        }

        public void PlayAttackAnimation(Attack attack)
        {
            if (AdventureManager.main.adventurePlayFast) return;
            attackLabel.color = attack.damageColor;
            var q = attack.isCritical ? "!" : "";
            attackLabel.text = $"{attack.damage}{q}";
            doTweenAnimation.DORestart();
        }

        public void TakeDamage(Attack attack)
        {
            _hero.TakeDamage(attack);
            // healthBar.text = _hero.health.healthBarDisplay;
        }

        private void OnMouseEnter()
        {
            nameLabel.enabled = true;
        }

        private void OnMouseExit()
        {
            nameLabel.enabled = false;
        }
    }
}
