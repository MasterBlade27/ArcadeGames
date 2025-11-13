using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLoad : MonoBehaviour
{
    [SerializeField]
    private bool Demo, Menu;

    [SerializeField]
    private string SceneName, MenuName;

    private PlayerMove pInput;

    public bool BSwitch = false;

    private Coroutine BufferTime;

    private void OnEnable()
    {
        if (!Demo)
        {
            BSwitch = false;

            if (BufferTime != null)
                StopCoroutine(BufferTime);

            BufferTime = StartCoroutine(Buffering(2.5f));
        }

        else if (Demo || Menu)
        {
            pInput = new PlayerMove();
            pInput.Enable();

            pInput.Movement.Escape.performed += ctx => QuitGame();
        }
    }

    private void OnDisable()
    {
        if (Demo || Menu)
        {
            pInput.Disable();

            pInput.Movement.Escape.performed -= ctx => QuitGame();
        }
    }

    void Update()
    {
        if (BSwitch)
            TransScene(SceneName);

        if (Demo)
            if (!Menu)
                if (pInput.Movement.Play.IsPressed())
                    StartGame();
    }

    private void TransScene(string ToScene)
    {
        Debug.Log(ToScene);
        BSwitch = false;
        SceneManager.LoadScene(ToScene);
    }

    private IEnumerator Buffering(float Dura)
    {
        yield return new WaitForSeconds(Dura);

        BSwitch = true;
        BufferTime = null;
    }

    public void StartGame()
    {
        if (BufferTime != null)
            StopCoroutine(BufferTime);

        BufferTime = StartCoroutine(Buffering(0.5f));
    }

    public void QuitGame()
    {
        if (Menu)
            Application.Quit();
        else
            TransScene(MenuName);
    }
}
