using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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


        public Action<Hero> heroInfoDisplay;
        
        private void Awake()
        {
            _heroRequest = GetComponent<HeroRequest>();
        }

        public void RestoreFromSO(CharacterSO characterSO, JobSO jobSO)
        {
            _hero = Hero.FromSO(characterSO, jobSO);
            
            _character = Instantiate(characterSO.characterMonoPrefab, transform);
            
            nameLabel.text = _hero.heroName;
        }

        private CharacterMono _character;
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

            BuffDisplay();
        }


        public BuffInfoDisplay buffInfoDisplayPrefab;
        public RectTransform buffInfoPanel;
        private void BuffDisplay()
        {
            _hero.buffHandler.buffs.ForEach(v =>
            {
                var display = Instantiate(buffInfoDisplayPrefab, buffInfoPanel);
                display.gameObject.SetActive(true);
                display.NewBuff(v);
            });
        }

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = _hero.Attack(battleVersus);
            PlayAttackAnimation(attack);
            return attack;
        }
        
        public void BattleDidEnd()
        {
            _hero.BattleDidEnd();
        }

        public void PlayAttackAnimation(Attack attack)
        {
            if (AdventureManager.main.adventurePlayFast) return;
            
            _character.Attack();
            
            attackLabel.color = attack.damageColor;
            var q = attack.isCritical ? "!" : "";
            attackLabel.text = $"{attack.damage}{q}";
            doTweenAnimation.DORestart();
        }

        public void TakeDamage(Attack attack)
        {
            
            _character.Hurt();
            
            _hero.TakeDamage(attack);


            if (!_hero.stillAlive)
            {
                _character.Death();
            }
        }

        private void OnMouseEnter()
        {
            nameLabel.enabled = true;
        }

        private void OnMouseExit()
        {
            nameLabel.enabled = false;
        }

        private void OnMouseDown()
        {
            heroInfoDisplay?.Invoke(_hero);
        }
    }
}
