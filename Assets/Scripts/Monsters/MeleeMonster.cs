using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class MeleeMonster : Monster
    {
        [System.NonSerialized] protected new MeleeMonsterBlueprint monsterBlueprint;
        protected float timeSinceLastAttack;

        public override void Setup(int monsterIndex, Vector2 position, MonsterBlueprint monsterBlueprint, float hpBuff = 0)
        {
            base.Setup(monsterIndex, position, monsterBlueprint, hpBuff);
            this.monsterBlueprint = (MeleeMonsterBlueprint) monsterBlueprint;
        }

        protected override void Update()
        {
            base.Update();
            // Attack cooldown
            timeSinceLastAttack += Time.deltaTime;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector2 moveDirection = (playerCharacter.transform.position - transform.position).normalized;
            rb.linearVelocity += moveDirection * monsterBlueprint.acceleration * Time.fixedDeltaTime;
            entityManager.Grid.UpdateClient(this);
        }

        void OnCollisionStay2D(Collision2D col)
        {
            if (alive && ((monsterBlueprint.meleeLayer & (1 << col.collider.gameObject.layer)) != 0) && timeSinceLastAttack >= 1.0f/monsterBlueprint.atkspeed)
            {
                playerCharacter.TakeDamage(monsterBlueprint.atk);
                timeSinceLastAttack = Mathf.Repeat(timeSinceLastAttack, 1.0f/monsterBlueprint.atkspeed);
            }
        }
    }
}
