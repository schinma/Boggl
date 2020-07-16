using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridDisplay : MonoBehaviour
{
    public GameObject[] diceEmplacements;

    public Text warningText;

    public void DisplayLetters(Dice.Letter[] letters)
    {
        for(int i=0; i < diceEmplacements.Length; i++)
        {
            diceEmplacements[i].GetComponentInChildren<Text>().text = letters[i].ToString();

            int rotation = Random.Range(0, 100);

            if (rotation < 25)
            {
                diceEmplacements[i].transform.rotation = Quaternion.Euler(0, 0, 90);
            } else if (rotation < 50)
            {
                diceEmplacements[i].transform.rotation = Quaternion.Euler(0, 0, 180);
            } else if (rotation < 75)
            {
                diceEmplacements[i].transform.rotation = Quaternion.Euler(0, 0, 270);
            } else
            {
                diceEmplacements[i].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void RedLetter(int diceIndex)
    {
        diceEmplacements[diceIndex].GetComponent<Image>().color = Color.red;
    }

    public void WhiteLetter(int diceIndex)
    {
        diceEmplacements[diceIndex].GetComponent<Image>().color = Color.white;
    }

    public void WriteWarning(string msg)
    {
        warningText.text = msg;
    }
}
