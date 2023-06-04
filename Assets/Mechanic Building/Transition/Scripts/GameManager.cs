using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [ReadOnly] public LevelManager Level;

    public void SetActiveAllInput(bool isActive)
    {
        transform.GetChild(0).gameObject.SetActive(!isActive);
    }

    
}
