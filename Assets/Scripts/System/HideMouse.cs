using System.Collections;
using UnityEngine;

public class HideMouse : MonoBehaviour
{
    bool Hide;

    private void Update()
    {
        if (Input.mousePositionDelta.x != 0 || Input.mousePositionDelta.y != 0 || Input.GetMouseButton(0))
        {
            Hide = false;
        }

        else if (!Hide && Input.mousePositionDelta.x == 0 && Input.mousePositionDelta.y == 0)
        {
            Hide = true;
            StartCoroutine(HidingMouse());
        }

        if (!Hide)
        {
            StopCoroutine(HidingMouse());
            Cursor.visible = true;
        }
    }
    private IEnumerator HidingMouse()
    {
        yield return new WaitForSeconds(3);
        Cursor.visible = false;
    }
}
