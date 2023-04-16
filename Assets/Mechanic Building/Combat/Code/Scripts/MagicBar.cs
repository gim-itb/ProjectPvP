using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicBar : MonoBehaviour
{
    [SerializeField] Image _image;

    void Update()
    {
        _image.fillAmount = ΩLul.Global.IceMagicCore.CurrentMagic / ΩLul.Global.IceMagicCore.MaxMagic;
    }
}
