using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionGroup : MonoBehaviour
{   
    [System.Serializable] class SelectionButton
    {
        public Button Button;
        public UnityEvent OnClick;
        public UnityEvent OnDisabled;

    }
    [Header("Selection Buttons")]
    [SerializeField] SelectionButton[] _selectionButtons = new SelectionButton[3];
    void Awake()
    {
        for(int i = 0; i < _selectionButtons.Length; i++)
        {
            int index = i; // You have to make a new variable here because of some C# bullcrap
            _selectionButtons[i].Button.onClick.AddListener(() => {

                // Disable all button
                for(int j = 0; j < _selectionButtons.Length; j++)
                {
                    _selectionButtons[j].OnDisabled.Invoke();
                }

                // Enable current button
                _selectionButtons[index].OnClick.Invoke();
            });
        }
    }
}
