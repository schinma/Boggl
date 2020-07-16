using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordListDisplay : MonoBehaviour
{
    [Header("Text Parameters")]
    public Font font;
    public Color color;
    public int fontSize = 14;

    public void AddWordToList(string word)
    {
        //wordList.text += word + "\n";
        GameObject newWord = new GameObject("WordText");
        Text newWordText = newWord.AddComponent<Text>();
        newWordText.text = word;
        newWordText.font = font;
        newWordText.color = color;
        newWordText.fontSize = fontSize;

        newWord.transform.SetParent(this.transform, false);
    }

    public void RemoveWord(string word)
    {
        Text[] words = this.GetComponentsInChildren<Text>();

        for (int i = 0; i < words.Length; i++) {
            if (words[i].text == word) {
                Destroy(words[i].gameObject);
            }
        }
    }

    public void ClearList()
    {
        foreach(Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
