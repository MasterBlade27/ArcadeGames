using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [SerializeField]
    private float speed = 100f;

    [SerializeField]
    private float direction, moveDirection;

    private float rotmove;

    private PaddleMove pInput;

    private bool GameOver;

    private void OnEnable()
    {
        pInput = new PaddleMove();
        pInput.Enable();
    }

    private void Update()
    {
        if (GameOver)
            gameObject.SetActive(false);

        direction = pInput.Movement.Move.ReadValue<Vector2>().x;

        moveDirection = direction * Time.deltaTime * speed;
        rotmove = moveDirection + transform.rotation.eulerAngles.z;

        transform.rotation = Quaternion.Euler(0, 0, rotmove);
    }


}
