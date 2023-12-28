using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;

namespace Tyrant
{
    public class EnemyMono: MonoBehaviour, IAmHero
    {
        public TextMeshProUGUI healthBar;

        [ShowInInspector] public Enemy enemy => _enemy;
        
        private Enemy _enemy;

        public Attribute attribute => _enemy.attribute;

        public string heroName => _enemy.heroName;

        public IAmHero heroic => _enemy;
        public bool stillAlive => _enemy.stillAlive;
        
        public Health health => _enemy.health;

        public DOTweenAnimation doTweenAnimation;
        public TextMeshProUGUI attackLabel;

        public TextMeshProUGUI nameLabel;

        public HeroActionQueue actionQueue => _enemy.actionQueue;
        
        
        private CharacterMono _character;
        private void Awake()
        {
            _enemy = new Enemy();
        }

        public void NewEnemy(EnemySO enemySO)
        {
            _enemy = new Enemy(enemySO);
            nameLabel.text = _enemy.heroName;
            _character = Instantiate(enemySO.characterSO.characterMonoPrefab, transform);
        }
        
        private void Start()
        {
            attackLabel.color = Color.clear;
            heroic.health.healthBarDisplay
                .Subscribe(v => healthBar.text = v)
                .AddTo(this);
        }

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = _enemy.Attack(battleVersus);
            PlayAttackAnimation(attack);
            return attack;
        }

        public void PlayAttackAnimation(Attack attack)
        {
            _character.Attack();
            attackLabel.color = Color.white;
            attackLabel.text = $"{attack.damage}";
            doTweenAnimation.DORestart();
        }

        public void TakeDamage(Attack attack)
        {
            _character.Hurt();
            _enemy.TakeDamage(attack);
            
            if (!_enemy.stillAlive)
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
    }
}