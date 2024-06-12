using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public event Action OnRoadPlacement;
    public event Action OnHousePlacement;
    public event Action OnSpecialPlacement;
    public event Action OnBigStructurePlacement;

    public Button placeRoadBtn;
    public Button placeHouseBtn;
    public Button placeSpecialBtn;
    public Button placeBigStructureBtn;

    public Color outlineColor;

    private List<Button> buttons;

    private void Start()
    {
        buttons = new() { placeRoadBtn, placeHouseBtn, placeSpecialBtn, placeBigStructureBtn };

        placeRoadBtn.onClick.AddListener(() =>
        {
            ResetBtnColor();
            ModifyOutline(placeRoadBtn);
            OnRoadPlacement?.Invoke();
        });
        placeHouseBtn.onClick.AddListener(() =>
        {
            ResetBtnColor();
            ModifyOutline(placeHouseBtn);
            OnHousePlacement?.Invoke();
        });
        placeSpecialBtn.onClick.AddListener(() =>
        {
            ResetBtnColor();
            ModifyOutline(placeSpecialBtn);
            OnSpecialPlacement?.Invoke();
        });
        placeBigStructureBtn.onClick.AddListener(() =>
        {
            ResetBtnColor();
            ModifyOutline(placeBigStructureBtn);
            OnBigStructurePlacement?.Invoke();
        });
    }

    private void ResetBtnColor()
    {
        foreach (var btn in buttons)
        {
            var outline = btn.GetComponent<Outline>();
            outline.enabled = false;
        }
    }

    private void ModifyOutline(Button btn)
    {
        var outline = btn.GetComponent<Outline>();
        outline.effectColor = outlineColor;
        outline.enabled = true;
    }
}