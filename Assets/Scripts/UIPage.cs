using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPage : EventTrigger
{
    public Image image = null;
    public Image GetImage
    {
        get
        {
            if (image = null)
            {
                image = this.transform.GetChild(0).GetComponent<Image>();
            }
            return image;
        }
        set
        {
            image = value;
        }
    }

    public Text text = null;
    public Text GetText
    {
        get
        {
            if (text = null)
            {
                text = this.transform.GetChild(1).GetComponent<Text>();
            }
            return text;
        }
        set
        {
            text = value;
        }
    }
	

    //点击UI_Page
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.transform.GetChild(1).GetComponent<Text>().text == "..." || this.transform.GetChild(1).GetComponent<Text>().text == "")
        {
            return;
        }

        PageGrid pg = PageGrid.GetInstance;

        //如果点击的是前面几个ui（点击的是1-5）
        if (int.Parse(this.transform.GetChild(1).GetComponent<Text>().text) < PageGrid.GetInstance.uiPageArray.Length)
        {
            string text = this.transform.GetChild(1).GetComponent<Text>().text;

            //更新显示
            PageGrid.GetInstance.UpadateUIPageFromHead();

            UIPage uiPage = PageGrid.GetInstance.FindUIPageWithText(text);
            if (uiPage)
            {
                PageGrid.GetInstance.ActivatUIPageImage(this.gameObject);
            }
        }
        //点击最后几个（点击的是最后4个）
        else if (int.Parse(this.transform.GetChild(1).GetComponent<Text>().text) >= PageGrid.GetInstance.maxPageIndex - 3)
        {
            string text = this.transform.GetChild(1).GetComponent<Text>().text;

            //更新显示
            PageGrid.GetInstance.UpdateUIPageFromEnd();

            UIPage uiPage = PageGrid.GetInstance.FindUIPageWithText(text);
            if (uiPage)
            {
                PageGrid.GetInstance.ActivatUIPageImage(uiPage.gameObject);
            }
        }
        else
        {
            string text = this.transform.GetChild(1).GetComponent<Text>().text;

            //更新显示
            PageGrid.GetInstance.UpdateUIPageFromMiddle(text);

            /*由于数字向后移动，故image显示位置不需要改变*/
        }
    }
}