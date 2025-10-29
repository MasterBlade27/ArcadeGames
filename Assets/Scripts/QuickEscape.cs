using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    private Restart RS;
    private BallMove BM;
    [SerializeField]
    private GameObject GMS, INS;
    private bool XOR, PlzEsc = false;
    private PaddleMove pInput;

    private void Start()
    {
        pInput = new PaddleMove();
        pInput.Enable();

        pInput.Movement.Escape.performed += ctx => PressEsc(true);

        RS = FindAnyObjectByType<Restart>();
        BM = FindAnyObjectByType<BallMove>();
    }

    private void OnDisable()
    {
        pInput.Disable();

        pInput.Movement.Escape.performed -= ctx => PressEsc(true);
    }

    void Update()
    {
        XOR = GMS.activeSelf ^ INS.activeSelf;

        if (!XOR)
            if (PlzEsc)
                ESCAPE();
    }

    public void ESCAPE()
    {
        BM.Starto = false;
        RS.lives = 0;
        RS.BallReset();
    }

    private void PressEsc(bool t)
    {
        PlzEsc = t;
    }
}
