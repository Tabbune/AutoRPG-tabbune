using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TacticEditor : MonoBehaviour
{
    private GameObject parentList;
    public GameObject swapButton;
    private Image _image;
    public Vector3 _oldPosition;

    private void Awake()
    {
        parentList = this.gameObject.transform.parent.gameObject;
        _image = swapButton.GetComponent<Image>();
        _oldPosition = _image.rectTransform.localPosition;
    }
    public void OnDeleteClickButton()
    {
        if(parentList.transform.childCount <= 1)
        {
            Debug.Log("Cannot delete last tactic");
            return;
        }
        //Debug.Log("Test");
        Destroy(this.gameObject);
    }
    public void OnClickUpButton()
    {
        int m_IndexNumber = transform.GetSiblingIndex();
        if (m_IndexNumber <= 0)
        {
            return;
        }
        //GameObject previousObject = parentList.transform.GetChild(m_IndexNumber - 1).gameObject;
        //previousObject.transform.SetSiblingIndex(m_IndexNumber + 1);

        m_IndexNumber -= 1;
        transform.SetSiblingIndex(m_IndexNumber);
    }

    public void OnClickDownButton()
    {

        int m_IndexNumber = transform.GetSiblingIndex();
        if (m_IndexNumber >= parentList.transform.childCount - 1)
        {
            return;
        }

        //GameObject nextObject = parentList.transform.GetChild(m_IndexNumber + 1).gameObject;
        //nextObject.transform.SetSiblingIndex(m_IndexNumber - 1);

        m_IndexNumber += 1;
        transform.SetSiblingIndex(m_IndexNumber);
    }
}
