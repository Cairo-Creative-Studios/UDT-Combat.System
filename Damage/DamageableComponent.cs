using UDT.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Armada.Systems.Damage
{
    public class DamageableComponent : StandardComponent<DamageableComponentData, DamageSystem>
    {
        public float health;
        public StandardEvent<DamageableComponent> onDamage;
        public StandardEvent<DamageableComponent> onOverTimeDamage;
        public StandardEvent<DamageableComponent> onDeath;
        public StandardEvent<DamageableComponent> onHeal;
        public StandardEvent<DamageableComponent> onOverTimeHeal;
        public StandardEvent<DamageableComponent> onSetHealth;
        public StandardEvent<DamageableComponent> onFullHealth;
        

        public override void OnInstantiate()
        {
            health = Data.maxHealth;
        }

        public void Damage(float damage)
        {
            onDamage?.Invoke(this);
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public Coroutine DamageOverTime(float damage, float interval)
        {
            return StartCoroutine(DamageOverTimeCoroutine(damage, interval));
        }   
        
        private System.Collections.IEnumerator DamageOverTimeCoroutine(float damage, float interval)
        {
            while (true)
            {
                onOverTimeDamage?.Invoke(this);
                
                health -= damage;
                if (health <= 0)
                {
                    Destroy(gameObject);
                }
                
                yield return new WaitForSeconds(interval);
            }
        }
        
        public void Heal(float heal)
        {
            health += heal;
            if (health > Data.maxHealth)
            {
                health = Data.maxHealth;
                onFullHealth?.Invoke(this);
            }
            onHeal?.Invoke(this);
        }
        
        public Coroutine HealOverTime(float heal, float interval)
        {
            return StartCoroutine(HealOverTimeCoroutine(heal, interval));
        }
        
        private System.Collections.IEnumerator HealOverTimeCoroutine(float heal, float interval)
        {
            while (true)
            {   
                onOverTimeHeal?.Invoke(this);
                
                health += heal;
                if (health > Data.maxHealth)
                {
                    health = Data.maxHealth;
                    onFullHealth?.Invoke(this);
                }
                
                yield return new WaitForSeconds(interval);
            }
        }
        
        public void SetHealth(float health)
        {
            this.health = health;
            onSetHealth?.Invoke(this);
        }
        
        public void SetMaxHealth(float maxHealth)
        {
            Data.maxHealth = maxHealth;
        }
    }
}