using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //인풋매니저
    InputManager input = new InputManager();
    public static InputManager Input { get { return Instance.input; } }

    //카메라
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera changeCamera;

    //포스트 프로세싱
    [SerializeField] PostProcessProfile postProcessProfile;
    ColorGrading colorGrading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Color Grading
        postProcessProfile.TryGetSettings(out colorGrading);
        colorGrading.colorFilter.value = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        input.OnUpdate();
    }

    public void PlayerChangeStart()
    {
        changeCamera.enabled = true;
        StartCoroutine(PlayerChangeStartCoroutine());
    }

    IEnumerator PlayerChangeStartCoroutine()
    {
        Color postColor = colorGrading.colorFilter.value;
        while (postColor.r > 0.5f)
        {
            postColor = new Color(postColor.r - Time.unscaledDeltaTime, postColor.g - Time.unscaledDeltaTime, postColor.b - Time.unscaledDeltaTime);
            colorGrading.colorFilter.value = postColor;
            yield return null;
        }
        postColor = new Color(0.5f, 0.5f, 0.5f);
        colorGrading.colorFilter.value = postColor;
    }

    public void PlayerChangeEnd()
    {
        StartCoroutine(PlayerChangeEndCoroutine());
    }

    IEnumerator PlayerChangeEndCoroutine()
    {
        Color postColor = colorGrading.colorFilter.value;
        while (postColor.r < 1f)
        {
            postColor = new Color(postColor.r + Time.unscaledDeltaTime, postColor.g + Time.unscaledDeltaTime, postColor.b + Time.unscaledDeltaTime); ;
            colorGrading.colorFilter.value = postColor;
            yield return null;
        }
        postColor = new Color(1f, 1f, 1f);
        colorGrading.colorFilter.value = postColor;
        changeCamera.enabled = false;
    }
}
