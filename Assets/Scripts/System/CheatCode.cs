using UnityEngine;

public class CheatCode : MonoBehaviour
{
    private PlayerMove KInput;
    private string KCS = "UUDDLRLRBA";
    [SerializeField]
    private string CurrentSeq = "";
    private float ResetTime = 0.5f, LastInputTime;
    public static bool ACT;

    private void OnEnable()
    {
        ACT = false;

        KInput = new PlayerMove();
        KInput.Enable();

        KInput.CheatCode.Up.performed += ctx => AddInput("U");
        KInput.CheatCode.Down.performed += ctx => AddInput("D");
        KInput.CheatCode.Left.performed += ctx => AddInput("L");
        KInput.CheatCode.Right.performed += ctx => AddInput("R");
        KInput.CheatCode.B.performed += ctx => AddInput("B");
        KInput.CheatCode.A.performed += ctx => AddInput("A");

    }

    private void OnDisable()
    {
        KInput.Disable();

        KInput.CheatCode.Up.performed -= ctx => AddInput("U");
        KInput.CheatCode.Down.performed -= ctx => AddInput("D");
        KInput.CheatCode.Left.performed -= ctx => AddInput("L");
        KInput.CheatCode.Right.performed -= ctx => AddInput("R");
        KInput.CheatCode.B.performed -= ctx => AddInput("B");
        KInput.CheatCode.A.performed -= ctx => AddInput("A");
    }

    private void Update()
    {
        if (Time.time - LastInputTime > ResetTime && CurrentSeq.Length > 0)
            CurrentSeq = "";
    }

    private void AddInput(string Input)
    {
        CurrentSeq += Input;
        LastInputTime = Time.time;

        if (CurrentSeq.Length > KCS.Length)
            CurrentSeq = "";

        if (CurrentSeq == KCS)
        {
            ACT = true;

            CurrentSeq = "";
        }
    }
}
