using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    private Restart RS;
    private BallMove BM;

    private void Start()
    {
        RS = FindAnyObjectByType<Restart>();
        BM = FindAnyObjectByType<BallMove>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BM.Starto = false;
            RS.lives = 0;
            RS.BallReset();
        }
    }
}
