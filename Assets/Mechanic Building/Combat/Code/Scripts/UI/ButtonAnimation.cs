using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    public void OnButtonDisabled(int index)
    {
        transform.GetChild(index).gameObject.GetComponent<Image>().enabled = false;
    }
    public void OnButtonEnabled(int index)
    {
        transform.GetChild(index).gameObject.GetComponent<Image>().enabled = true;
    }
}
