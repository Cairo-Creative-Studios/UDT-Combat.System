# UDT - Foundational Combat System
Weapons, Projectiles and a Damage System, the foundations of Combat systems, integrated with the Unity Development Toolkit.
===
# Installation
Follow the instructions for these git URL's in order
```
https://github.com/Cairo-Creative-Studios/Unity-Development-Toolkit.git
https://github.com/Cairo-Creative-Studios/UDT-Combat.System.git
```
1. Within the Unity Editor Menu Bar, Click Package Manager
2. At the top-left, click the + button
3. click "Add package by git URL"
4. copy and paste the URL
5. Click Add, and wait for it to install

---
# Components 

## Damageables
The Damageable Standard Component enables Damage instigation for the Object you're adding it to. Standard Events are declared for each Damage/Heal call.

## Damage Instigators 
Extend the Damage Instigator class to create new forms of Damage, which apply Damage in different ways. 

## Projectiles 
Projectiles collide with Damageables and create Damage Instigators. 

---
# Usage

## In-Editor
Select Game Object > Add Component > Damageable, Weapon, or Projectile
(Don't be alarmed that the hierarchy of your Object will change a bit. This is due to the design of UDT Standard Objects and Components)

For Data assets including Damage Instigators, right click in the Project Browser and find Damaegable Data, DamageInstigator, Weapon Data, or Projectile Data.

---
## Creating a simple Character in the Unity Editor
1. AddComponent, Damageable
2. AddComponent, Weapon
3. Edit the properties of the Components to your liking.
4. Create a basic MonoBehaviour for more control of your Character.

## Creating a simple Projectile in the Unity Editor
1. AddComponent, Projectile
2. Edit the properties of the Components to your liking.

## Creating a simple Character template in C#
Create a class extending StandardComponent for your character, and add the decorators:
```c#
[RequireStandardComponent(typeof(Damageable))]
[RequireStandardComponent(typeof(Weapon))]
public class Character : StandardComponent<CharacterData, CharacterSystem>
{

}
```
To use this newly created Standard Component, add it to your Game Object just like any Standard Component, and the UDT will work out the rest.
Of course, you will have to change some properties to set up your character to your desires.

## Creating a simple Projectile in C#
```c#
[RequireStandardComponent(typeof(Projectile))]
public class MyProjectile : StandardComponent<MyProjectiledata, WeaponSystem>
{

}
```
This newly created StandardComponent would log itself to the WeaponSystem so the Weapon System can keep track of it's state.
