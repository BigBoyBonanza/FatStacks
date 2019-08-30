using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [System.Serializable]
    public struct Credit
    {
        public string name;
        public string title;
    };

    public Credit[] Credits = new Credit[0];
    public Text text;
    public float end;
    public RectTransform rectTransform;
    public float speed;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        foreach (Credit credit in Credits)
        {
            text.text += string.Format("<size=30>{0}</size>\n{1}\n\n", credit.title, credit.name);
        }
        yield return new WaitForSeconds(1f);
        while(rectTransform.localPosition.y < end)
        {
            rectTransform.localPosition += Vector3.up * speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(5f);
        Application.Quit();
    }
}
