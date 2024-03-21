using System.Collections;
using UnityEngine;

public class Panel : MonoBehaviour
{
    protected RectTransform rectTransform;
    private Coroutine animateCoroutine;

    [SerializeField] private float animationDuration = 0.5f;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(); // Start the open animation
    }

    public virtual void Init()
    {
        // Start animate in
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }
        animateCoroutine = StartCoroutine(AnimateScale(Vector3.zero, Vector3.one, animationDuration)); // Half a second to animate in
        audioSource.clip = openClip;
        audioSource.Play();
    }

    public virtual void DeInit()
    {
        // Start animate out
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }
        animateCoroutine = StartCoroutine(AnimateScale(Vector3.one, Vector3.zero, animationDuration)); // Half a second to animate out
        audioSource.clip = closeClip;
        audioSource.Play();
    }

    private IEnumerator AnimateScale(Vector3 startScale, Vector3 endScale, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            rectTransform.localScale = Vector3.Slerp(startScale, endScale, time / duration);
            time += Time.deltaTime;

            
            yield return null;
        }
        rectTransform.localScale = endScale; // Ensure it ends at the exact scale
    }

    private void OnDisable()
    {
        DeInit(); // Start the close animation
    }
}