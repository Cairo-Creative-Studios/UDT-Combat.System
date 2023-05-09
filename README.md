# UDT - Foundational Combat System
Weapons, Projectiles and a Damage System, the foundations of Combat systems, integrated with the Unity Development Toolkit.

# Installation
To use this, you must first have the UDT Core installed in your project: https://github.com/Cairo-Creative-Studios/Unity-Development-Toolkit
Clone this Repository into your project. (.unitypackage coming soon) 

# Components 

## Damageables
The Damageable Standard Component enables Damage instigation for the Object you're adding it to. Standard Events are declared for each Damage/Heal call.

## Damage Instigators 
Extend the Damage Instigator class to create new forms of Damage, which apply Damage in different ways. 

## Projectiles 
Projectiles collide with Damageables and create Damage Instigators. 

# Usage

## In-Editor
Select Game Object > Add Component > Damageable, Weapon, or Projectile
(Don't be alarmed that the hierarchy of your Object will change a bit. This is due to the design of UDT Standard Objects and Components)

For Data assets including Damage Instigators, right click in the Project Browser and find Damaegable Data, DamageInstigator, Weapon Data, or Projectile Data.

## Creating a simple Character
Create a class extending StandardComponent for your character, and add the decorators:
### Damageable
``` C#
[RequireStandardComponent(typeof(Damageable))]
```
### Weapon
``` C#
[RequireStandardComponent(typeof(Weapon))]
```
### Projectile
``` C#
[RequireStandardComponent(typeof(Projectile))]
```
Then interface with them within your new Character Component. 
