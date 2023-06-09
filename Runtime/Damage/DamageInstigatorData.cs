using UDT.Core;
using UnityEngine;

namespace Combat.Damage
{
    [CreateAssetMenu(fileName = "Damage Instigator", menuName = "UDT Combat/DamageInstigatorComponentData", order = 0)]
    public class DamageInstigatorData : ScriptableObject
    {
        public float damage;
        public float radius;
        public AnimationCurve damageFalloff;
        public float knockback;
    }
}