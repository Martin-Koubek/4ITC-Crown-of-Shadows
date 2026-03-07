using UnityEngine;

public class menuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject controls;
    public bool controlsOpen;
    public void OpenControls()
    {
        if (controlsOpen)
        {
            controlsOpen = false;
            controls.SetActive(false);
            menu.SetActive(true);
        }
        else
        {
            controlsOpen=true;
            menu.SetActive(false);
            controls.SetActive(true);

        }
    }
}
