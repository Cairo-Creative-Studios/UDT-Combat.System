using NaughtyAttributes;
using UDT.Core;
using UnityEngine;

namespace Combat.Weapons
{
    [CreateAssetMenu(fileName = "WeaponComponentData", menuName = "UDT Combat/WeaponComponentData", order = 0)]
    public class WeaponComponentData : ComponentData<WeaponComponent>
    {
        [HorizontalLine(1, EColor.Gray)]
        [Header("Construction Defaults")]
        [Tooltip("The Prefab that will be instantiated when the weapon is spawned.")]
        public GameObject prefab;
        [Tooltip("The Projectile that will be spawned when the weapon is fired.")]
        public GameObject projectile;
        
        [HorizontalLine(1, EColor.Gray)]
        [Header("Fire Settings")]
        [Tooltip("The angle of spread of the weapon.")]
        public float spread;
        public AnimationCurve spreadCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 1f);
        [Tooltip("The number of projectiles that will be spawned when the weapon is fired in auto mode.")]
        public int shotCount;
        [Tooltip("The rate of fire of the weapon.")]
        public float fireRate;
        public enum FireMode
        {
            Single,
            Burst,
            Auto
        }
        [Tooltip("The fire mode of the weapon.")]
        public FireMode fireMode;
        
        [HorizontalLine(1, EColor.Gray)]
        [Header("Ammo Settings")]
        [Tooltip("The ammo a clip of the weapon can hold. Negative values mean infinite ammo.")]
        public int clipSize;
        [Tooltip("The total ammo the weapon can hold.")]
        public int maxAmmo;
        public enum ReloadMode
        {
            None,
            Timed,
            Animation
        }
        [Tooltip("Controls the reload functionality of the Weapon.")]
        public ReloadMode reloadMode;
        [HideIf("reloadMode", ReloadMode.None)]
        [Tooltip("Determines whether the weapon will reload automatically when the clip is emptied.")]
        public bool reloadOnEmpty;
        [Tooltip("The time it takes for the weapon to reload.")]
        public float reloadTime;

        public override string GetAttachedGOPath()
        {
            return "Weapon";
        }
    }
}