using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class UI_Menu_ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite sprite;
    public Sprite spriteHover;
    private Image ImageBox;

    void Start() {
        GetComponent<Animator>().Play("Hover off");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Animator>().Play("UI_Menu_AnimationButton");

        Debug.Log("hover");
        transform.GetComponent<Image>().sprite = spriteHover;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Animator>().Play("UI_Menu_AnimationButton_Exit");

        Debug.Log("NO hover");
        transform.GetComponent<Image>().sprite = sprite;
    }

    //void OnMouseOver() {
        //Debug.Log("hover");
        //transform.GetComponent<Image>().sprite = spriteHover;
        //ImageBox.sprite = spriteHover;
    //}

    //void OnMouseExit() {
        //transform.GetComponent<Image>().sprite = sprite;
    //}
}
