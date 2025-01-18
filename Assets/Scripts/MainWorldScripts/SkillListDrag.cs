// Taken from https://discussions.unity.com/t/how-to-drag-ui-object-around-canvas-with-ray-interactor/919797

using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class SkillListDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    static bool dragging;
    static Vector3 beginMousePoint;
    static Vector3 beginListPos;
    static RectTransform skillListCanvasRect;
    static RectTransform skillListRect;

    void Start() {
        skillListCanvasRect = GameObject.Find("Skill List Canvas").GetComponent<RectTransform>();
        skillListRect = gameObject.GetComponent<RectTransform>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
       ReadyDrag(eventData);
    }
     public void OnDrag(PointerEventData eventData)
    {
        // Keep this empty to satisfy Unity's EventSystem
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    public static void ReadyDrag(PointerEventData eventData) {
        dragging = true;
        beginMousePoint = skillListCanvasRect.InverseTransformPoint(eventData.position);
        beginListPos = skillListRect.anchoredPosition;
    }
    public static void EndDrag() {
        dragging = false;
    }

    void Update()
    {
        if (dragging) {
            Vector3 currentMousePoint = skillListCanvasRect.InverseTransformPoint(Input.mousePosition);
            skillListRect.anchoredPosition = beginListPos - (beginMousePoint - currentMousePoint);
        }
    }
}