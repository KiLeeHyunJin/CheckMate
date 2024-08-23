using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour , IPointerClickHandler
// 아이템 설명창
{
    public enum Own //아이템 설명창을 불러오는 부모
    { 
        NULL,Player, Feild,
    }
    
        public TextMeshProUGUI Name;
        public TextMeshProUGUI DescriptionText;
        public Own Getter { get; set; }
        RectTransform m_RectTransform;
        CanvasScaler m_CanvasScaler;
        Canvas m_Canvas;
        public InventoryUI Owner { get; set; }
        //public FeildStorageUI Owner2 { get; set; }
        void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_CanvasScaler = GetComponentInParent<CanvasScaler>();
            m_Canvas = GetComponentInParent<Canvas>();
        }

        void OnEnable()
        {
            //UpdatePosition();
        }

        void Update()
        {
            //UpdatePosition();
        }

    public void UpdatePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
    
        Vector3[] corners = new Vector3[4];    
        m_RectTransform.GetWorldCorners(corners);
    
        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;
    
        if (width + 16 < Screen.width - mousePosition.x)
        {
            m_RectTransform.transform.position = mousePosition + Vector3.right * 16;
        }
        else
        {
            m_RectTransform.transform.position = mousePosition + Vector3.left * (width + 16);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch(Getter)
        {
            //case Own.Feild:
            //    Owner2.ObjectHoverExited();
            //    Owner2.ToolTipActive = false;
            //    break;
            case Own.Player:
                Owner.ObjectHoverExited();
                Owner.ToolTipActive = false;
                break;
        }
        Getter = Own.NULL;

        return;
    }
}
