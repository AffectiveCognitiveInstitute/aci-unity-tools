using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DynamicTextMesh : MonoBehaviour
{
    private static string regexPattern = "\\$\\(\\d+?\\)";

    private TextMeshProUGUI textMesh;

    private object[] values;

    public string indexMatcher(Match match)
    {
        string target = match.Value.Substring(2, match.Length - 3);
        int index = int.Parse(target);

        if (index < values.Length)
            target = values[index].ToString();
        
        return target;
    }

    public void UpdateDynamicContent(params object[] content)
    {
        if (textMesh == null)
            textMesh = GetComponent<TextMeshProUGUI>();

        values = content;

        textMesh.text = Regex.Replace(textMesh.text, regexPattern, indexMatcher);
    }
}
