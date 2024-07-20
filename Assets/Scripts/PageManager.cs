using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour
{
    public static PageManager Instance;

    public Text pageNum;
    public Transform parentTrans;
    public GameObject PageBtnObj;
    public PageGrid PageGrid;
    public Dictionary<int, List<PageData>> pageInfo = new Dictionary<int, List<PageData>>();
    public List<GameObject> pageBtnList = new List<GameObject>();

    public void Awake()
    {
        Instance = this;

        pageInfo.Clear();
        pageBtnList.Clear();

        for (int i = 1; i <= PageGrid.maxPageIndex; i++)
        {
            List<PageData> list = new List<PageData>();
            for (int j = 1; j <= 12; j++)
            {
                PageData page = new PageData();
                page.id = j;
                page.isUse = false;
                list.Add(page);
            }

            pageInfo.Add(i, list);
        }
    }

    public void InitPageBtn(int pageId)
    {
        pageNum.text = "第" + pageId + "页";
        List<PageData> list = pageInfo[pageId];
        if (pageBtnList.Count != 0)
        {
            for (int i = 0; i < pageBtnList.Count; i++)
            {
                PageBtn pageBtn = pageBtnList[i].GetComponent<PageBtn>();
                pageBtn.InitPageData((list[i]));
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                GameObject obj = GameObject.Instantiate(PageBtnObj);
                obj.SetActive(true);
                obj.transform.SetParent(parentTrans);
                obj.transform.localScale = Vector3.one;
                PageBtn pageBtn = obj.GetComponent<PageBtn>();
                pageBtn.InitPageData((list[i]));

                pageBtnList.Add(obj);
            }
        }
    }
}