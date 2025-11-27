using UnityEngine;

public enum WeaponType
{
    OneHanded,
    TwoHanded
}
public class Sword : MonoBehaviour
{
    public Sprite icon;
    public int baseDmg;
    public int damage;
    public WeaponType type;
    public int lvl;

    public Sword(int lvl)
    {
        this.lvl = lvl;
    }

    private void Awake()
    {
        damage = baseDmg * lvl;
    }
}
