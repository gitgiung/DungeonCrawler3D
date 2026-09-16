using UnityEngine;

namespace Enemy
{

    public interface IEnemyMove
    {
        public void MoveTo(Vector3 destination);

        public void StopMoving();

        public void ResumeMoving();

        public void SetMoveSpeedMultiplier(float multiplier);
    }

    public interface IEnemyAttack
    {
        public void Attack(Transform target);
    }

    public interface IDamageable
    {
        public void TakeDamage(int damage);
    }

    public interface IEnemyDead
    {
        public void Die();
    }
}