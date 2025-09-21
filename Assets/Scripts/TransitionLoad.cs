using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class TransitionLoad : MonoBehaviour
{
    [SerializeField]
    private bool Demo;

    [SerializeField]
    private string SceneName;

    public bool BSwitch = false;

    private Coroutine BufferTime;

    private void OnEnable()
    {
        if (!Demo)
        {
            BSwitch = false;

            if (BufferTime != null)
                StopCoroutine(BufferTime);

            BufferTime = StartCoroutine(Buffering());
        }
    }

    void Update()
    {
        if (BSwitch)
        {
            TransScene();
        }

        if (Demo)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                BSwitch = true;
            }

            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    private void TransScene()
    {
        BSwitch = false;
        SceneManager.LoadScene(SceneName);
    }
    
    private IEnumerator Buffering()
    {
        float endtime = 2.5f;
        float timer = 0f;
        while (timer < endtime)
        {
            timer += Time.deltaTime;

            Debug.Log(timer);

            if (timer >= endtime)
            {
                BSwitch = true;
                BufferTime = null;
            }

            yield return null;
        }
    }
}
