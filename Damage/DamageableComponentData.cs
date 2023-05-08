using UDT.Core;
using UnityEngine;

namespace Armada.Systems.Damage
{
    [CreateAssetMenu(fileName = "DamageableComponentData", menuName = "Armada/Damage/DamageableComponentData", order = 0)]
    public class DamageableComponentData : ComponentData<DamageableComponent>
    {
        public float maxHealth;
        public AnimationCurve overTimeHealCurve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}