using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    private Restart RS;

    private void Start()
    {
        RS = FindAnyObjectByType<Restart>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            RS.lives = 0;
            RS.BallReset();
        }
    }
}
