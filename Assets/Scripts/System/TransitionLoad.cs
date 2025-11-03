using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLoad : MonoBehaviour
{
    [SerializeField]
    private bool Demo;

    [SerializeField]
    private string SceneName;

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

            BufferTime = StartCoroutine(Buffering());
        }

        else
        {
            pInput = new PlayerMove();
            pInput.Enable();

            pInput.Movement.Escape.performed += ctx => QuitGame();
        }
    }

    private void OnDisable()
    {
        if (Demo)
        {
            pInput.Disable();

            pInput.Movement.Escape.performed -= ctx => QuitGame();
        }
    }

    void Update()
    {
        if (BSwitch)
            TransScene();

        if (Demo)
        {
            if (pInput.Movement.Play.IsPressed())
                StartGame();

            else if (Input.GetKeyUp(KeyCode.Escape))
                QuitGame();
        }
    }

    private void TransScene()
    {
        BSwitch = false;
        SceneManager.LoadScene(SceneName);
    }

    private IEnumerator Buffering()
    {
        yield return new WaitForSeconds(2.5f);

        BSwitch = true;
        BufferTime = null;
    }

    public void StartGame()
    {
        BSwitch = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
