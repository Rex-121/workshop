using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tyrant
{
    public class CharacterMono : MonoBehaviour
    {

        public Animator animator;
        private static readonly int WalkID = Animator.StringToHash("Walk");
        private static readonly int AttackID = Animator.StringToHash("Attack");
        private static readonly int HurtID = Animator.StringToHash("Hurt");
        private static readonly int DeathID = Animator.StringToHash("Death");
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        
        [Button]
        public void Attack()
        {
            animator.SetTrigger(AttackID);
        }


        [Button]
        public void Walk(bool isWalk)
        {
            animator.SetBool(WalkID, isWalk);
        }

        [Button]
        public void Hurt()
        {
            animator.SetTrigger(HurtID);
        }

        [Button]
        public void Death()
        {
            animator.SetTrigger(DeathID);
        }
    }
}
