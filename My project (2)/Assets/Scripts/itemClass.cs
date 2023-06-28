using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemClass", menuName = "ItemClass")]
public class itemClass : ScriptableObject
{
    public Sprite sprite;
    public enum WeaponType
    {
        Ranged,
        Melee,
        Hamburguesa,
        Other
    };
    public WeaponType weapontype;

    public bool explodes = false;
    public float explosionDelay = 0;

    public Vector2 weaponSize = new Vector2(0.8f, 0.8f);
    public bool canHoldDown;
    public Sprite projectileSprite;
    public GameObject projectile;
    public Vector2 bulletSize = new Vector2(1, 1);
    public Color bulletColor = Color.white;
    public float projectileSpeed;
    public float timeBetweenShots;
    public float bulletDamage;
    public float gravityScale = 0;

    public Vector2 MeleeAtackArea;
    public float timeBetweenMeleeAtack;
    public float attackSpeed;
    public float MeleeDamage;
}
