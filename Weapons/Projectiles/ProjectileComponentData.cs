using System.Collections.Generic;
using Armada.Systems.Damage;
using NaughtyAttributes;
using UDT.Core;
using UnityEngine;

namespace Armada.Systems.Weapons
{
    [CreateAssetMenu(fileName = "ProjectileComponentData", menuName = "Armada/Weapons/ProjectileComponentData", order = 0)]
    public class ProjectileComponentData : ComponentData<ProjectileComponent>
    {
        [HorizontalLine(1f, EColor.Gray)]
        [Header("Targets and Damage")]
        [Tooltip("The tags of the targets that the projectile can hit. If this list is empty, the projectile can hit any target.")]
        public List<string> targetTags = new List<string>();
        [Tooltip("When a Target is hit, the given DamageInstigator will be used to deal damage to the target.")]
        public DamageInstigatorData damageInstigator;
        [HorizontalLine(1f, EColor.Gray)]
        [Header("General")]
        public float lifeTime;
        [Tooltip("The base speed of the projectile.")]
        public float speed;
        [Tooltip("The speed modifier of the projectile. The speed will be multiplied by this curve over time.")]
        public AnimationCurve speedModifier = AnimationCurve.Linear(0, 1, 1, 0);
        [HorizontalLine(1f, EColor.Gray)]
        [Header("Magnet")]
        [Tooltip("The Radius of the projectile's magnet. If a target is within this radius, it will be pulled towards the projectile.")]
        public float magnetRadius;
        [Tooltip("The force of the projectile's magnet. If a target is within the magnet's radius, it will be pulled towards the projectile with this force.")]
        public float magneticForce;
        [Tooltip("The force modifier of the projectile's magnet. The force will be multiplied by this curve over time.")]
        public AnimationCurve magneticForceModifier = AnimationCurve.Linear(0, 1, 1, 0);

        [Tooltip("The maximum angle that the projectile can rotate towards a target.")]
        public float lockedOnTurnSpeed = 5f;
    }
}