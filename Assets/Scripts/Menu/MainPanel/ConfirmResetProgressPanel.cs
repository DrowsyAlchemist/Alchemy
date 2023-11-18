using UnityEngine;

public class ConfirmResetProgressPanel : MonoBehaviour
{
    private void OnDisable()
    {
        gameObject.SetActive(false);
    }
}
