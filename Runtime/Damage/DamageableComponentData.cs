using UDT.Core;
using UnityEngine;

namespace Combat.Damage
{
    [CreateAssetMenu(fileName = "DamageableComponentData", menuName = "UDT Combat/DamageableComponentData", order = 0)]
    public class DamageableComponentData : ComponentData<DamageableComponent>
    {
        public float maxHealth;
        public AnimationCurve overTimeHealCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}