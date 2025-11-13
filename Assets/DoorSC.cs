using UnityEngine;

public class DoorSC : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField]
    private Animator animator;
    int _AnimatorDoorOpenID;
    private void Awake()
    {
        _AnimatorDoorOpenID = Animator.StringToHash("isOpen");
    }
    private void Update()
    {
        if (isOpen)
        {
            animator.SetBool(_AnimatorDoorOpenID, true);
        }
        else
        {
            animator.SetBool(_AnimatorDoorOpenID, false);
        }
    }
}
