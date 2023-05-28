using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicBar : MonoBehaviour
{
    [SerializeField] Image _image;
    void OnEnable()
    {
        ΩLul.Global.MagicCore.OnMagicChanged += OnMagicChanged;
    }
    void OnDisable()
    {
        ΩLul.Global.MagicCore.OnMagicChanged -= OnMagicChanged;
    }
    void OnMagicChanged(MagicCore core)
    {
        _image.fillAmount = core.Data.Magic / core.Data.MaxMagic;
    }
}
