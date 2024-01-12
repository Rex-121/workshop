using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tyrant
{
    public class HeroMono : MonoBehaviour, IAmHero, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public TextMeshProUGUI healthBar;

        [ShowInInspector, BoxGroup("HERO", centerLabel: true, AnimateVisibility = true), InlineProperty, HideLabel]
        private Hero _hero;

        public Hero hero
        {
            set
            {
                _hero = value;
                
                _character = Instantiate(_hero.characterSO.characterMonoPrefab, transform);
            
                nameLabel.text = _hero.heroName;
            }
            get => _hero;
        }

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
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            nameLabel.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            nameLabel.enabled = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            heroInfoDisplay?.Invoke(_hero);
        }
    }
}
