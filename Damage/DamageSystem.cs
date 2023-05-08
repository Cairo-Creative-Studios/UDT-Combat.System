using System;
using System.Linq;
using System.Numerics;
using UDT.Core;
using Vector3 = UnityEngine.Vector3;

namespace Armada.Systems.Damage
{
    public class DamageSystem : System<DamageSystem>
    {
        public static void Damage(DamageInstigatorData damageInstigatorData, Vector3 position)
        {
            
        }
        
        /// <summary>
        /// Damages a target StandardObject
        /// </summary>
        /// <param name="damageInstigatorData"></param>
        /// <param name="target"></param>
        /// <param name="direction"></param>
        public static void Damage(DamageInstigatorData damageInstigatorData, StandardObject target, Vector3 direction = default)
        {
            DamageableComponent damageableComponent = target.GetComponent<DamageableComponent>();
            if (damageableComponent != null)
            {
                damageableComponent.Damage(damageInstigatorData.damage);
            }
        }
        
        public static DamageableComponent[] GetDamageablesInRange(Vector3 position, float radius)
        {
            return (DamageableComponent[])Array.FindAll(GetComponents<DamageableComponent>(),damageableComponent => Vector3.Distance(position, damageableComponent.transform.position) <= radius).ToArray();
        }
    }
}