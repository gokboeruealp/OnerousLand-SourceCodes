using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 gunPos = Camera.main.WorldToScreenPoint(transform.position);

        mousePos.x = mousePos.x - gunPos.x;
        mousePos.y = mousePos.y - gunPos.y;

        float gunAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(180, 0, -gunAngle));
            transform.parent.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, gunAngle));
            transform.parent.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
