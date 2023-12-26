using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
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
        
        private void Awake()
        {
            _enemy = new Enemy();
        }

        public void NewEnemy(EnemySO enemySO)
        {
            _enemy = new Enemy(enemySO);
            
            healthBar.text = _enemy.health.healthBarDisplay;

            nameLabel.text = _enemy.heroName;
        }
        
        private void Start()
        {
            attackLabel.color = Color.clear;

            

            // _isOnAnaAdventure
            //     .Subscribe(v =>
            // {
            //     healthBar.gameObject.SetActive(v);
            //     // _heroRequest.gameObject.SetActive(!v);
            // }).AddTo(this);
        }

        public Attack Attack(IBattleVersus battleVersus)
        {
            var attack = _enemy.Attack(battleVersus);
            PlayAttackAnimation(attack);
            return attack;
        }

        public void PlayAttackAnimation(Attack attack)
        {
            attackLabel.color = Color.white;
            attackLabel.text = $"{attack.damage}";
            doTweenAnimation.DORestart();
        }

        public void TakeDamage(Attack attack)
        {
            _enemy.TakeDamage(attack);
            healthBar.text = _enemy.health.healthBarDisplay;
        }
    }
}