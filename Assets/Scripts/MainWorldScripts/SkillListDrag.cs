// Taken from https://discussions.unity.com/t/how-to-drag-ui-object-around-canvas-with-ray-interactor/919797

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class SkillListDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    bool dragging;
    Vector3 beginMousePoint;
    Vector3 beginListPos;
    RectTransform userInterfaceRect;
    RectTransform skillListRect;

    void Start() {
        userInterfaceRect = GameObject.Find("User Interface").GetComponent<RectTransform>();
        skillListRect = gameObject.GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        beginMousePoint = userInterfaceRect.InverseTransformPoint(eventData.position);
        beginListPos = skillListRect.anchoredPosition;
        dragging = true;
        Debug.Log("dragging");
    }
     public void OnDrag(PointerEventData eventData)
    {
        // Keep this empty to satisfy Unity's EventSystem
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        Debug.Log("not dragging");
    }

    void Update()
    {
        if (dragging) {
            Vector3 currentMousePoint = userInterfaceRect.InverseTransformPoint(Input.mousePosition);
            skillListRect.anchoredPosition = beginListPos - (beginMousePoint - currentMousePoint);
        }
    }
}