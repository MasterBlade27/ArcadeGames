using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    public bool BO, CT;

    private Restart RS;
    private BallMove BM;
    [SerializeField]
    private GameObject GMS, INS; //GMS = Game Over Screen | INS = Name Input Screen
    private bool XOR, PlzEsc = false;
    private PlayerMove pInput;

    private void Start()
    {
        pInput = new PlayerMove();
        pInput.Enable();

        pInput.Movement.Escape.performed += ctx => PressEsc(true);
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
        RS = FindAnyObjectByType<Restart>();
        RS.lives = 0;

        if (BO)
        {
            BM = FindAnyObjectByType<BallMove>();
            BM.Starto = false;
            RS.BallReset();
        }

        if (CT)
        {
            RS.CentiReset();
        }
    }

    private void PressEsc(bool t)
    {
        PlzEsc = t;
    }
}
