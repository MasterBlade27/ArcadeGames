using UnityEngine;

public class QuickEscape : MonoBehaviour
{
    public bool BO, CT, DD;

    private Restart RS;
    private BallMove BM;
    private Player CTP;
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
        pInput.Movement.Escape.performed -= ctx => PressEsc(true);

        pInput.Disable();
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
        if (BO)
        {
            RS = FindAnyObjectByType<Restart>();
            RS.lives = 0;
            BM = FindAnyObjectByType<BallMove>();
            BM.Starto = false;
            RS.BallReset();
        }

        if (CT)
        {
            CTP = FindAnyObjectByType<Player>();
            CTP.lives = 0;
            CTP.LoseLife();
        }

        if (DD)
        {
            FindFirstObjectByType<TransitionLoad>().enabled = true;
        }
    }

    private void PressEsc(bool t)
    {
        PlzEsc = t;
    }
}
