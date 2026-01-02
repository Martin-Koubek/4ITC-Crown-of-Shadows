using UnityEngine;

public class ConnectionPoint : MonoBehaviour
{
    public bool used = false;

    // Smìr, kterým má chodba vyjít z místnosti (nastavit v editoru)
    public Vector3 direction = Vector3.forward;

    // Vrátí start pozici chodby mimo místnost
    public Vector3 GetCorridorStart() // zvýšený offset
    {
        return transform.position + direction.normalized; //* offset;
    }
}
