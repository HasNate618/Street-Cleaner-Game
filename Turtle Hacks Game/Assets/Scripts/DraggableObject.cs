using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public string wasteType;
    [SerializeField] Vector3 lift;
    private Vector3 mOffset, mObjectOffset, prevPos;
    private float mZCoord;
    private LineRenderer lineRenderer;
    public bool complete = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void OnMouseDown()
    {
        // Pick up object
        ItemSpawner.draggedItem = transform;
        prevPos = transform.position;
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mObjectOffset = transform.position - GetMouseWorldPos();
        mOffset = lift;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        // Move object
        //transform.position = GetMouseWorldPos() + mObjectOffset + mOffset;
        transform.position = GetMouseWorldPos() + mOffset;
    }

    private void OnMouseUp()
    {
        //Release object
        Vector3 rayOrigin = transform.position;
        Vector3 rayDirection = Vector3.down;
        float maxDistance = 2f;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, maxDistance))
        {
            string hitTag = hitInfo.collider.tag;

            if (hitTag == wasteType) GameManager.instance.UpdateScore(wasteType);
            else
            {
                string actualName = name.Substring(0, name.Length - 7);
                GameManager.instance.OnMistakeMade(actualName);
            }

            complete = true;
            Destroy(gameObject);
        }
        else
        {
            transform.position = prevPos;
        }

        ItemSpawner.draggedItem = null;
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position + Vector3.down * 5.5f);
    }
}
