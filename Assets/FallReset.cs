using UnityEngine;

public class FallReset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interaction>(out Interaction interaction))
        {
            interaction.ResetPlayer(other.gameObject);
            interaction.toReset = true;
        }
    }
}
