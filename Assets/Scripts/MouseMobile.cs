using UnityEngine;

public class MouseMobile : MonoBehaviour
{
    [SerializeField]
    private GameObject PaddleGO;

    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float direction;

    private float MousePos, objectPosition;

    private MoveInput MI;
    [SerializeField]
    private float clamping;

    private bool GameOver;

    private void OnEnable()
    {
        MI = FindAnyObjectByType<MoveInput>();
        PaddleGO = MI.gameObject;
    }

    private void Update()
    {
        clamping = MI.clamping;
        GameOver = MI.GameOver;
        PaddleGO.transform.position = new Vector3(
            Mathf.Clamp(PaddleGO.transform.position.x, -clamping, clamping), 
            PaddleGO.transform.position.y, 
            PaddleGO.transform.position.z
            );

        if (GameOver)
            gameObject.SetActive(false);
    }

    private void OnMouseDrag()
    {
        if(!GameOver)
            MI.enabled = false;

        float diff;

        MousePos = Input.mousePosition.x;

        objectPosition = Camera.main.ScreenToWorldPoint(new Vector3(MousePos, 0f, 0f)).x;

        diff = objectPosition - PaddleGO.transform.position.x;

        direction = diff * Time.deltaTime * speed;

        PaddleGO.transform.position += new Vector3(Mathf.Clamp(direction, -0.10f, 0.10f), 0, 0);

        if (direction > 0)
            PaddleGO.transform.position = new Vector3(
                Mathf.Clamp(PaddleGO.transform.position.x, PaddleGO.transform.position.x, objectPosition),
                PaddleGO.transform.position.y,
                PaddleGO.transform.position.z
                );
        else if (direction < 0)
            PaddleGO.transform.position = new Vector3(
                Mathf.Clamp(PaddleGO.transform.position.x, objectPosition, PaddleGO.transform.position.x),
                PaddleGO.transform.position.y,
                PaddleGO.transform.position.z
                );
    }

    private void OnMouseUp()
    {
        if(!GameOver)
            MI.enabled = true;
    }
}
