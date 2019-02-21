using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script to maintain player stats and modifiers between scenes
// HP methods adapted from the free Unity store asset 'Simple Health Heart System' by ariel oliveira [o.arielg@gmail.com]
public class Stats
{
    // Count the number of rooms the player's passed through.
    private static int roomCount = 0;

    // the total possible number of heart containers the player can have
    public const int MAX_TOTAL_HEALTH = 6;
    public delegate void OnHealthChangedDelegate();
    public static OnHealthChangedDelegate onHealthChangedCallback;

    // the player's current hp 
    private static float hp = 3.0f;
    // the total amount of hp the player can currently have 
    private static float maxHp = 3.0f;

    // the amount of damage the player's melee attack currently does.
    private static float dmg = 0.5f;
    // the number of tiles away the player can melee attack. 
    private static int range = 1;

    // Each type of item the player has. 
    public static List<Item> passives = new List<Item>();
    public static Item active;

    /* ROOM COUNT */
    // Will probably need a full reset at some point to reset all this stuff for another run - setup function somewhere?
    public static int RoomCount
    {
        get
        {
            return roomCount;
        }
        set
        {
            roomCount = value; // incrementing is the only setting that should occur, no jumping between numbers
        }
    }

    /* HP */
    public static float Hp
    {
        get
        {
            return hp;
        }
        set 
        {
            hp = value;
        }
    }

    public static float MaxHp
    {
        get
        {
            return maxHp;
        }
    }

    public static void Heal(float health)
    {
        hp += health;
        ClampHealth();
    }
    public static void TakeDamage(float dmg)
    {
        hp -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHp < MAX_TOTAL_HEALTH)
        {
            maxHp += 1;
            hp = maxHp;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    static void ClampHealth()
    {
        hp = Mathf.Clamp(hp, 0, MAX_TOTAL_HEALTH);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    /* DAMAGE */
    public static float Dmg
    {
        get
        {
            return dmg;
        }
        set
        {
            dmg = value;
        }
    }

    /* RANGE */
    public static int Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

}
