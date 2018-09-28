using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContainerHandler : MonoBehaviour
{
    private Container container;

    private int selectedIndex;
    private int maxIndex;
    private bool canGoToNextSlot = true;

    void Start()
    {
    }

    public void Show(Container c)
    {
        container = c;
        maxIndex = c.Items.Count - 1;

        selectedIndex = 0;
        UpdateView();
        UpdateItemTitle();
        UpdateSelection(0);
    }

    public void UpdateView()
    {
        if (container == null || maxIndex == -1)
            return;

        var transform = this.transform.GetChild(0).GetChild(0);

        var ct = 0;
        foreach (var obj in container.Items)
        {
            var child = transform.GetChild(ct);
            child.gameObject.SetActive(true);
            child.GetComponent<Image>().sprite = obj.ReferencedItem.Image;
            ct++;
        }

        for (int i = ct; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    private void UpdateItemTitle()
    {
        if (maxIndex == -1)
            return;

        transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = 
            container.Items[selectedIndex].ReferencedItem.ItemName;
    }

    void Update()
    {
        if (canGoToNextSlot)
        {
            var old = selectedIndex;

            if (Input.GetAxis("Vertical") > 0.0f && selectedIndex > 2)
                selectedIndex -= 3;
            else if (Input.GetAxis("Vertical") < 0.0f && selectedIndex < 6)
                selectedIndex += 3;

            if (Input.GetAxis("Horizontal") < 0.0f && selectedIndex % 3 > 0)
                selectedIndex -= 1;
            else if (Input.GetAxis("Horizontal") > 0.0f && selectedIndex % 3 < 2)
                selectedIndex += 1;

            if (old != selectedIndex)
            {
                if (selectedIndex > maxIndex)
                    selectedIndex = maxIndex;
                
                //selectedIndex = Mathf.Clamp(selectedIndex, 0, maxIndex);

                UpdateSelection(old);
                UpdateItemTitle();

                Input.ResetInputAxes();
                canGoToNextSlot = false;
                StartCoroutine("SkipBetweenSlots");
            }
        }
    }

    private void UpdateSelection(int old)
    {
        if (old < 0 || old > maxIndex)
            return;

        var transform = this.transform.GetChild(0).GetChild(0);

        transform.GetChild(old).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(selectedIndex).GetChild(0).gameObject.SetActive(true);
    }

    private IEnumerator SkipBetweenSlots()
    {
        yield return new WaitForSeconds(0.2f);
        canGoToNextSlot = true;
    }

    public void Close()
    {
        if (maxIndex != -1)
            transform.GetChild(0).GetChild(0).GetChild(selectedIndex).GetChild(0).gameObject.SetActive(false);
    }
}
