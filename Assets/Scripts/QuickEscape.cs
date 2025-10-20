using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    private Restart RS;
    private BallMove BM;
    [SerializeField]
    private GameObject GMS, INS;
    private bool XOR;

    private void Start()
    {
        RS = FindAnyObjectByType<Restart>();
        BM = FindAnyObjectByType<BallMove>();
    }

    void Update()
    {
        XOR = GMS.activeSelf ^ INS.activeSelf;

        if (!XOR)
            if (Input.GetKeyUp(KeyCode.Escape))
                ESCAPE();
    }

    public void ESCAPE()
    {
        BM.Starto = false;
        RS.lives = 0;
        RS.BallReset();
    }
}
