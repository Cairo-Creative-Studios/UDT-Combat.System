using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UDT.Core;
using UDT.Core.Controllables;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Combat.Weapons
{
    [ExecuteAlways]
    public class WeaponComponent : StandardComponent<WeaponComponentData>, IControllable
    {
        #region Serialized Variables
        /// <summary>
        /// The root of the weapon. The weapon will be attached to this object.
        /// </summary>
        [HorizontalLine(1, EColor.Gray)]
        [Header("Construction")]
        [Tooltip("The root of the weapon. The weapon will be attached to this object.")]
        [SerializeField]
        private Transform root;
        /// <summary>
        /// The offset of the weapon's root, relative to the weapon's angle.
        /// </summary>
        [Tooltip("The offset of the weapon's root, relative to the weapon's angle.")]
        [SerializeField]
        private Vector3 rootAngleOffset;
        /// <summary>
        /// The direction of the weapon.
        /// </summary>
        [Tooltip("The direction of the weapon.")]
        public Vector3 direction = Vector3.forward;
        /// <summary>
        /// The total ammo the weapon can hold.
        /// </summary>
        [HorizontalLine(1, EColor.Gray)]
        [Header("Ammo")]
        [Tooltip("The total ammo the weapon can hold.")]
        public int leftoverAmmo;
        /// <summary>
        /// The ammo in the current clip.
        /// </summary>
        [Tooltip("The ammo in the current clip.")]
        public int clipAmmo;
        /// <summary>
        /// The weapon's muzzle. The projectile will be spawned at this object.
        /// </summary>
        [Tooltip("The weapon's muzzle. The projectile will be spawned at this object.")]
        [SerializeField]
        private List<Transform> muzzles;
        [Button("Add Muzzle")]
        private void AddMuzzle()
        {
            muzzles.Add(new GameObject("Muzzle").transform);
            muzzles[^1].parent = transform;
            muzzles[^1].localPosition = Vector3.zero;
            muzzles[^1].localRotation = Quaternion.identity;
        }
        #endregion
        
        #region Hidden Variables
        /// <summary>
        /// The time the weapon last shot.
        /// </summary>
        private float _lastShotTime;
        /// <summary>
        /// Whether the root is null.
        /// </summary>
        private bool _isrootNull;
        /// <summary>
        /// Whether the weapon has just shot.
        /// </summary>
        private bool _justShot = true;
        /// <summary>
        /// The offset of the muzzle.
        /// </summary>
        private Vector3 _muzzleOffset;

        private bool _shooting;

        private int shotsFired;
        #endregion

        #region Events
        [HorizontalLine(1, EColor.Gray)]
        [Header("Events")]
        [Tooltip("The event that is invoked when the weapon is spawned.")]
        public StandardEvent<WeaponComponent> onWeaponSpawned;
        [Tooltip("The event that is invoked when the weapon is fired.")]
        public StandardEvent<WeaponComponent> onWeaponFired;
        [Tooltip("The event that is invoked when the weapon spawns a Projectile.")]
        public StandardEvent<ProjectileComponent> onProjectileSpawned;
        [Tooltip("The event that is invoked when the weapon is reloaded.")]
        public StandardEvent<WeaponComponent> onWeaponReloaded;
        [Tooltip("The event that is invoked when the weapon fails to reload.")]
        public StandardEvent<WeaponComponent> onWeaponReloadFailed;
        [Tooltip("The event that is invoked when the weapon is emptied.")]
        public StandardEvent<WeaponComponent> onWeaponEmpty;
        #endregion

        void Start()
        {
            _isrootNull = root == null;
            onWeaponSpawned?.Invoke(this);
        }
        
        private void Update()
        {
            if (muzzles == null || muzzles.Count == 0)
            {
                Transform[] children = GetComponentsInChildren<Transform>();
                if (children.Length > 0)
                {
                    foreach (Transform child in children)
                    {
                        if (child.name == "Muzzle")
                        {
                            muzzles.Add(child);
                            break;
                        }
                    }
                }
                else
                {
                    AddMuzzle();
                }
            }
            if (root != null)
            {
                //Rotate the weapon to face the aim direction
                transform.position = root.position;
                transform.rotation = root.rotation;
                transform.Rotate(rootAngleOffset);
            }
            
            //If the weapon is shooting, fire the weapon
            if (_shooting)
                Fire();
        }

        public override void OnInputAction(InputAction.CallbackContext context)
        {
            if (context.action.name == "Fire")
            {
                //If the fire button is pressed, fire the weapon
                if (context.performed)
                {
                    _shooting = true;
                    Fire();
                    _justShot = false;
                }
                //If the fire button is released, reset the justShot variable
                if (context.canceled)
                {
                    _shooting = false;
                    _justShot = true;
                }
            }
        }

        public void Fire()
        {
            //Check if can Fire
            if(Data.projectile == null) return;
            if (clipAmmo == 0 && Data.clipSize > 0) return;
            if(_lastShotTime + Data.fireRate > Time.time) return;
            
            //Fire the weapon
            _lastShotTime = Time.time;
            switch (Data.fireMode)
            {
                //Shoot a single projectile
                case WeaponComponentData.FireMode.Single:
                    if (!_justShot) return;
                    SpawnProjectile(shotsFired%muzzles.Count);
                    break;
                //Shoot multiple projectiles
                case WeaponComponentData.FireMode.Burst:
                    if (!_justShot) return;
                    for (int i = 0; i < Data.shotCount; i++)
                    {
                        SpawnProjectile(shotsFired%muzzles.Count);
                    }
                    break;
                //Shoot a single projectile every *fireRate* seconds
                case WeaponComponentData.FireMode.Auto:
                    SpawnProjectile(shotsFired%muzzles.Count);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SpawnProjectile(int i)
        {
            onWeaponFired?.Invoke(this);
            //Handle ammo count
            clipAmmo--;

            //Spawn projectile
            GameObject projectile = Instantiate(Data.projectile, muzzles[i].position, muzzles[i].rotation);
            
            //Rotate projectile using the spread curve
            float spreadAlpha = Random.Range(0f, 1f);
            Vector3 spread = new Vector3(Data.spreadCurve.Evaluate(spreadAlpha), Data.spreadCurve.Evaluate(spreadAlpha),
                Data.spreadCurve.Evaluate(spreadAlpha));
            projectile.transform.Rotate(Random.Range(-Data.spread, Data.spread) * spread.x, Random.Range(-Data.spread, Data.spread) * spread.y, Random.Range(-Data.spread, Data.spread) * spread.z);

            //Check if weapon is empty
            if (clipAmmo == 0)
            {
                onWeaponEmpty?.Invoke(this);
                if(Data.reloadOnEmpty)
                    Reload();
            }
            
            shotsFired++;
        }
        
        public void Reload()
        {
            if (clipAmmo == Data.clipSize) return;
            if (leftoverAmmo == 0)
            {
                onWeaponReloadFailed?.Invoke(this);
                return;
            }
            clipAmmo = Mathf.Min(leftoverAmmo - Data.clipSize, 0);
            onWeaponReloaded?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            if(muzzles == null) return;
            for (int i = 0; i < muzzles.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(muzzles[i].position, muzzles[i].forward * 10);
            }
        }

        //TODO: Change Weapon to Weapon Root, and turn Muzzle in Weapon, and create a List of Weapons in the Weapon Root, to allow for multiple weapons on the same Component
        public struct Muzzle
        {
            public Transform transform;

            public enum ShotType
            {
                Single,
                Burst,
                Auto
            }
            
            public ProjectileComponentData projectile;
        }
    }
}