using UnityEngine;

public enum WeaponType
{
    OneHanded,
    TwoHanded
}
public class Sword : MonoBehaviour
{
    public int damage;
    public WeaponType type;
}
