using System.Collections;
using UnityEngine;

public class RespawnBall : MonoBehaviour
{
    [SerializeField]
    Vector3 BallPos, OriPause;
    private PlayerMove pInput;

    private void Start()
    {
        OriPause = transform.position;
    }

    private void OnEnable()
    {
        pInput = new PlayerMove();
        pInput.Enable();
    }

    private void Update()
    {
        if (pInput.Movement.Select.IsPressed())
            StartCoroutine(KILLRESET());
    }

    private IEnumerator KILLRESET()
    {
        BallPos = FindAnyObjectByType<BallMove>().transform.position;
        FindAnyObjectByType<BallMove>().gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        yield return new WaitForEndOfFrame();
        transform.position = BallPos;
        yield return new WaitForSeconds(2);
        transform.position = OriPause;
    }
}
