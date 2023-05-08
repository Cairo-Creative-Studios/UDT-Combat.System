using System.Linq;
using Armada.Systems.Damage;
using UDT.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Armada.Systems.Weapons
{
    [RequireComponent(typeof(BoxCollider))]
    public class ProjectileComponent : StandardComponent<ProjectileComponentData>
    {
        private Transform[] _nearbyTargets;
        private float spawnTime;
        private float lifeTime;
        private bool _isDataNull;
        public StandardEvent<ProjectileComponent> onSpawn;
        public StandardEvent<ProjectileComponent> onDespawn;
        private Vector3 _lastPosition;
        [FormerlySerializedAs("target")] public Transform lockOnTarget;
        private bool _isLockedOn;

        private void Start()
        {
            _isDataNull = Data == null;
        }

        private void Update()
        {
            lifeTime += Time.deltaTime;
            if (lifeTime > Data.lifeTime)
            {
                Destroy(gameObject);
                return;
            }
            
            transform.position += transform.forward * (Data.speed * Data.speedModifier.Evaluate((Time.time - spawnTime) / lifeTime) * Time.deltaTime);

            // If the projectile is locked on, rotate towards the target
            if (_isLockedOn)
            {
                var rotation = Quaternion.LookRotation(lockOnTarget.position - transform.position);
                transform.eulerAngles = transform.eulerAngles.LerpAngle(rotation.eulerAngles, Data.lockedOnTurnSpeed * Time.deltaTime);
            }
            // Otherwise, rotate towards the direction of travel, and magnetize towards nearby targets
            else
            {
                if (Data.magnetRadius > 0)
                {
                    var _nearbyColliders = Physics.OverlapSphere(transform.position, Data.magnetRadius).Select(collider => collider.transform).ToArray();
                    _nearbyTargets = _nearbyColliders.Where(target => Data.targetTags.Contains(target.tag)).ToArray();
                    foreach (Transform target in _nearbyTargets)
                    {
                        var rotation = Quaternion.LookRotation(target.position - transform.position);
                        transform.eulerAngles = transform.eulerAngles.LerpAngle(rotation.eulerAngles, Data.magneticForceModifier.Evaluate(1-(transform.position - target.position).magnitude/Data.magnetRadius) * Time.deltaTime);
                    }
                }
            }

            _lastPosition = transform.position;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            //Handle collision with Damageables
            if (_isDataNull) return;
            if (Data.damageInstigator == null) return;
            if (Data.targetTags.Count > 0 && !Data.targetTags.Contains(collision.gameObject.tag)) return;
            
            var damageable = collision.gameObject.GetComponent<DamageableComponent>();
            if (damageable == null) return;
            DamageSystem.Damage(Data.damageInstigator, damageable.Object);
            Destroy(gameObject);
        }

        public void OnTriggerEnter(Collider other)
        {
            //Handle collision with Damageables
            if (_isDataNull) return;
            if (Data.damageInstigator == null) return;
            if (Data.targetTags.Count > 0 && !Data.targetTags.Contains(other.gameObject.tag)) return;
            
            var damageable = other.gameObject.GetComponent<DamageableComponent>();
            if (damageable == null) return;
            DamageSystem.Damage(Data.damageInstigator, damageable.Object);
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.forward * 10);
        }

        /// <summary>
        /// Locks the projectile onto a target, overriding the projectile's default trajectory to follow the target
        /// </summary>
        /// <param name="target"></param>
        public void LockOn(Transform target)
        {
            lockOnTarget = target;
            _isLockedOn = true;
        }
    }
}