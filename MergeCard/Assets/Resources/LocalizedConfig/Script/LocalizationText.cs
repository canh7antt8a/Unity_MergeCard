using UnityEngine;  
using System.Collections;  
using UnityEngine.UI;  
public class LocalizationText : MonoBehaviour   
{  
    public string key = " ";  
    void OnEnable()  
    {
        setValue();
    }

    public void setValue() {
        key = key.Trim();
        var val = LocalizationManager.GetInstance.GetValue(key);
        if (val != null) {
            GetComponent<Text>().text = val;
        }
    }
} 