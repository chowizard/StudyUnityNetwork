using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public abstract class UiFrame : MonoBehaviour
{
    public enum eAnchorType
    {
        Center = 0,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    private Dictionary<eAnchorType, RectTransform> uiAnchors = new Dictionary<eAnchorType, RectTransform>();

    public RectTransform uiAnchorCenter;
    public RectTransform uiAnchorLeft;
    public RectTransform uiAnchorRight;
    public RectTransform uiAnchorTop;
    public RectTransform uiAnchorBottom;
    public RectTransform uiAnchorTopLeft;
    public RectTransform uiAnchorTopRight;
    public RectTransform uiAnchorBottomLeft;
    public RectTransform uiAnchorBottomRight;

    // Use this for initialization
    protected virtual void Start()
    {
        RegisterAnchorPoint(eAnchorType.Center);
        RegisterAnchorPoint(eAnchorType.Left);
        RegisterAnchorPoint(eAnchorType.Right);
        RegisterAnchorPoint(eAnchorType.Top);
        RegisterAnchorPoint(eAnchorType.Bottom);
        RegisterAnchorPoint(eAnchorType.TopLeft);
        RegisterAnchorPoint(eAnchorType.TopRight);
        RegisterAnchorPoint(eAnchorType.BottomLeft);
        RegisterAnchorPoint(eAnchorType.BottomRight);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    private RectTransform RegisterAnchorPoint(eAnchorType type)
    {
        Transform findTransform = transform.Find(type.ToString());
        Debug.Assert(findTransform != null);
        if(findTransform == null)
            return null;

        RectTransform rectTransform = findTransform.GetComponent<RectTransform>();
        Debug.Assert(rectTransform != null);
        if(rectTransform == null)
            return null;

        uiAnchors[type] = rectTransform;

        return rectTransform;
    }
}
