using UnityEngine;

public class MouseHover : MonoBehaviour
{
    public float speed = 5f;

    [SerializeField]
    private GameObject Player;
    private Vector3 ObjectDirect;

    private MoveTest MT;

    private void Start()
    {
        MT = FindAnyObjectByType<MoveTest>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        ObjectDirect.x = Input.mousePositionDelta.x * Time.deltaTime * speed;
        ObjectDirect.z = Input.mousePositionDelta.y * Time.deltaTime * speed;

        Player.transform.position += ObjectDirect;

        if (Input.GetMouseButton(0))
            MT.MSHT = true;
        else
            MT.MSHT = false;
    }
}
