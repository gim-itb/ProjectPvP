using UnityEngine;
using System.Collections;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] RectTransform _orangeSquareRT;
    [SerializeField] float _startXScale = 0;
    [SerializeField] float _endXScale = 1;

    
    float _outFinishedDelay = 0;
    float _outDuration = 0.3f;
    public IEnumerator OutAnimation()
    {
        _orangeSquareRT.pivot = new Vector2(0.5f, 1);
        float t = 0;
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_outDuration;
            float newX = Mathf.Lerp(_startXScale, _endXScale, Ease.OutQuart(t));
            _orangeSquareRT.localScale = new Vector3(1, newX, 1);
            yield return null;
        }
        _orangeSquareRT.localScale = new Vector3(1, 1, 1);
        OnAnimationOutFinished?.Invoke();
    }

    float _inStartedDelay = 0.5f;
    float _inDuration = 0.3f;
    public IEnumerator InAnimation()
    {
        _orangeSquareRT.pivot = new Vector2(0.5f, 0);
        float t = 0;
        yield return new WaitForSecondsRealtime(_inStartedDelay);
        while(t <= 1)
        {
            t += Time.unscaledDeltaTime/_inDuration;
            float newX = Mathf.Lerp(_endXScale, _startXScale, Ease.OutQuart(t));
            _orangeSquareRT.localScale = new Vector3(1, newX, 1);
            yield return null;
        }
        _orangeSquareRT.localScale = new Vector3(0, 1, 1);
        OnAnimationInFinished?.Invoke();
    }

    public System.Action OnAnimationOutFinished;
    public System.Action OnAnimationInFinished;
    public static void StartSceneTransition(TransitionAnimation transition, string sceneName)
    {
        TransitionAnimation animation = Instantiate(transition, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(animation.gameObject);
        animation.StartCoroutine(animation.OutAnimation());
        animation.OnAnimationOutFinished = () => {
            animation.StartCoroutine(animation.InAnimation());
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        };
        animation.OnAnimationInFinished = () => {
            Destroy(animation.gameObject);
        };
    }
}