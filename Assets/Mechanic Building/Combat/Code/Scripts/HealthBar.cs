using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
#endif
public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _whiteImg;
    [SerializeField] Image _healthImg;
    public void SetValue(float newValue)
    {
        ++_key;
        StartCoroutine(HealthFillAnimation(_healthImg, newValue, 0.15f, Ease.OutQuart));
        StartCoroutine(HealthFillAnimation(_whiteImg, newValue, 0.5f, Ease.InOutQuart));
    }
    public void SetBarImmediete(float newValue)
    {
        _healthImg.fillAmount = Mathf.Clamp(newValue, 0, 1);
        _whiteImg.fillAmount = Mathf.Clamp(newValue, 0, 1);
    }

    byte _key;
    IEnumerator HealthFillAnimation(Image img, float newValue, float duration, Ease.Function easeFunction)
    {
        byte requirement = _key;
        float t = 0;
        float oldValue = img.fillAmount;
        while(t < 1 && requirement == _key)
        {
            t += Time.unscaledDeltaTime/duration;
            img.fillAmount = Mathf.Lerp(oldValue, newValue, easeFunction(t));
            yield return null;
        }
        if(requirement == _key)
        {
            img.fillAmount = newValue;
        }
    }


#if UNITY_EDITOR
    [Range(0,1)] [SerializeField] float _newValue = 1;
    void OnValidate()
    {
        if(_healthImg != null && _whiteImg != null)
        SetValue(_newValue);
    }
    void OnEnable() => UnityEditor.EditorApplication.update += EditorUpdate;
    void OnDisable() => UnityEditor.EditorApplication.update -= EditorUpdate;
    
    void ForceRepaint()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }
    void EditorUpdate()
    {
        ForceRepaint();
    }
#endif

}
