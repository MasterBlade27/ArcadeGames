using UnityEngine;

public class MoveInput : MonoBehaviour
{
    //Speed for the Paddle, 10 seems like good speed
    [SerializeField]
    private float speed = 10f;

    //pInput = Paddle/Player Input
    private PaddleMove pInput;
    [SerializeField]
    private float direction, moveDirection;

    private void Start()
    {
        pInput = new PaddleMove();
        //Enable the Movement Script
        pInput.Enable();
    }

    private void Update()
    {
        //Reads Player's Input
        direction = pInput.Movement.Move.ReadValue<Vector2>().x;

        //Normalize and Multiply the Direction of the Paddle
        moveDirection = direction * Time.deltaTime * speed;
        //Moves the Paddle
        transform.position += new Vector3(moveDirection, 0, 0);
        //Clamp the Paddle's Position
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7.5f, 7.5f), transform.position.y, transform.position.z);
    }
}
