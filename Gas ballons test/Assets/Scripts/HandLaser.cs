using UnityEngine;
using UnityEngine.UI;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class HandLaser : SteamVR_LaserPointer
{
    private float visibleThickness = 0.002f;

    public override void OnPointerIn(PointerEventArgs e)
    {
        base.OnPointerIn(e);
        if (e.target.CompareTag("WallNotebook"))
        {
            thickness = visibleThickness;
            Button button = e.target.GetComponent<Button>();
            if (button != null)
            {
                button.image.color = button.colors.highlightedColor;
            }
        }
        else
        {
            thickness = 0f;
        }
    }

    public override void OnPointerClick(PointerEventArgs e)
    {
        base.OnPointerClick(e);
        if (e.target.CompareTag("WallNotebook"))
        {
            Button button = e.target.GetComponent<Button>();
            if (button != null)
            {
                button.image.color = button.colors.pressedColor;
                button.onClick.Invoke();
            }
        }
    }

    public override void OnPointerOut(PointerEventArgs e)
    {
        base.OnPointerOut(e);
        if (e.target.CompareTag("WallNotebook"))
        {
            thickness = 0f;
            Button button = e.target.GetComponent<Button>();
            if (button != null)
            {
                button.image.color = button.colors.normalColor;
            }
        }
    }
}
