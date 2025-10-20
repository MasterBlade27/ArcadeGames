using UnityEngine;

public class MoveInput : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 15f;

    //pInput = Paddle/Player Input
    private PaddleMove pInput;
    [SerializeField]
    private float direction, moveDirection;
    public float clamping;

    public bool GameOver;

    private void OnEnable()
    {
        pInput = new PaddleMove();
        //Enable the Movement Script
        pInput.Enable();
        clamping = 7.5f;
    }

    private void OnDisable()
    {
        pInput.Disable();
    }

    private void Update()
    {
        if (!GameOver)
        {
            if (FindAnyObjectByType<MouseMobile>() != null)
            {
                if (direction == 0)
                    FindAnyObjectByType<MouseMobile>().enabled = true;
                else
                    FindAnyObjectByType<MouseMobile>().enabled = false;
            }
        }

        //Reads Player's Input
        direction = pInput.Movement.Move.ReadValue<Vector2>().x;

        //Normalize and Multiply the Direction of the Paddle
        moveDirection = direction * Time.deltaTime * speed;
        //Moves the Paddle
        transform.position += new Vector3(moveDirection, 0, 0);
        //Clamp the Paddle's Position

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -clamping, clamping),
            transform.position.y,
            transform.position.z
            );
    }
}
