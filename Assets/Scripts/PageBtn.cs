using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageBtn : MonoBehaviour
{
    public PageData PageData;
    private Text text;
    private string name;
    private Button btn;

    public void Awake()
    {
        this.text = GetComponentInChildren<Text>();
        btn=  GetComponent<Button>();
    }

    public void InitPageData(PageData PageData)
    {
        this.PageData = PageData;
        name ="C:" + this.PageData.id;
        text.text = "C:" + this.PageData.id;

        if (this.PageData.isUse)
        {
            btn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            btn.GetComponent<Image>().color = Color.white; 
        }


        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(OnClick);
    }


    public void OnClick()
    {
        Debug.Log("PageBtn OnClick:"+name);
        
        
        //@TOO send MSG
        
        PageData.isUse = true;
        btn.GetComponent<Image>().color = Color.green;
    }
}
