using UnityEngine;

public enum WeaponType
{
    OneHanded,
    TwoHanded
}
public class Sword : MonoBehaviour
{
    public Sprite icon;
    public int damage;
    public WeaponType type;
}
