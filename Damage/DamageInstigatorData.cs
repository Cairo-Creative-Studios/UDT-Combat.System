using UDT.Core;
using UnityEngine;

namespace Armada.Systems.Damage
{
    [CreateAssetMenu(fileName = "Damage Instigator", menuName = "Armada/Damage/DamageInstigatorComponentData", order = 0)]
    public class DamageInstigatorData : ScriptableObject
    {
        public float damage;
        public float radius;
        public AnimationCurve damageFalloff;
        public float knockback;
    }
}