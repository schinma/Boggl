using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;


public class Grid : MonoBehaviour
{
    private const int DICE_NUMBER = 16;


    public Dice[] dices = new Dice[DICE_NUMBER];
    public GridDisplay gridDisplay;
    public AudioManager audio;
    public WordListDisplay wordDisplay;
    public GameObject gridBlocker;
   
    public TextAsset[] dictionaryTexts;
    private Trie dictionary;

    private PhotonView PV;

    private Dice.Letter[] letters = new Dice.Letter[DICE_NUMBER];

    private bool startedWord = false;
    private OrderedDictionary currentWord = new OrderedDictionary();
    private List<string> wordFoundList = new List<string>();
    private bool letterActivated;
    private GameManager.Langage lastLoadedLangage = GameManager.Langage.NONE;
    private int lasDiceIndex = -1;

    private void Start()
    {
        //LoadDictionnary(GameManager.Langage.FR);
        gridDisplay.WriteWarning("");
        PV = GetComponent<PhotonView>();
    }

    public void LoadDictionnary(GameManager.Langage lan)
    {
        TextAsset dictionnaryList = null;
        Debug.Log(lastLoadedLangage + " " + lan);
        if (lastLoadedLangage != lan)
        {
            dictionary = new Trie();
            Debug.Log("Loading dictionnary");
            switch (lan)
            {
                case GameManager.Langage.ENG:
                    dictionnaryList = dictionaryTexts[1];
                    lastLoadedLangage = GameManager.Langage.ENG;
                    break;
                case GameManager.Langage.FR:
                    dictionnaryList = dictionaryTexts[0];
                    lastLoadedLangage = GameManager.Langage.FR;
                    break;
            }
            dictionary.AddWordList(Regex.Split(dictionnaryList.text, "\n|\r|\r\n"));
        }
    }

    public void DesactivateLetters()
    {
        letterActivated = false;
    }

    public void ActivateLetters()
    {
        letterActivated = true;
    }

    public void ShakeLetters()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        List<int> list = new List<int>(Enumerable.Range(0, 16));

        int count = list.Count;
        int last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            int tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }

        for (int i = 0; i < DICE_NUMBER; i++)
        {
            letters[list[i]] = dices[i].faces[Random.Range(0, 5)];
        }

        PV.RPC("RPC_SendLetters", RpcTarget.OthersBuffered, LetterArrayToString());

        gridDisplay.DisplayLetters(letters); 
        audio.PlayShuffle();
    }

    [PunRPC]
    private void RPC_SendLetters(string newLetters)
    {
        StringToLetterArray(newLetters);
        gridDisplay.DisplayLetters(letters);
        audio.PlayShuffle();
    }

    private string LetterArrayToString()
    {
        string result = string.Empty;

        foreach(Dice.Letter letter in letters) {
            result += letter + ",";
        }

        return result;
    }

    private void StringToLetterArray(string str)
    {
        Debug.Log("received " + str);
        string[] splits = str.Split(',');

        for (int i = 0; i < DICE_NUMBER; i++) {
            letters[i] = Dice.CharToLetter(splits[i][0]);
        }

        foreach(Dice.Letter letter in letters) {
            Debug.Log(letter);
        }
    }

    public void StartWord(int diceIndex)
    {
        if (!startedWord)
        {
            //Debug.Log("Starting word at dice " + diceIndex + " with the letter " + letters[diceIndex]);
            startedWord = true;
            currentWord.Add(diceIndex, letters[diceIndex]);
            gridDisplay.RedLetter(diceIndex);
            lasDiceIndex = diceIndex;
        }
    }

    public void ContinueWord(int diceIndex)
    {
        if (CheckDice(diceIndex, lasDiceIndex) && startedWord && !currentWord.Contains(diceIndex))
        {
            //Debug.Log("Pointer entered " + diceIndex + " with the letter " + letters[diceIndex]);
            gridDisplay.RedLetter(diceIndex);
            currentWord.Add(diceIndex, letters[diceIndex]);
            lasDiceIndex = diceIndex;
        }
    }

    public void FinishWord(int diceIndex)
    {
        if (startedWord)
        {
            string word ="";
            foreach(DictionaryEntry letter in currentWord)
            {
                word += letter.Value;
                gridDisplay.WhiteLetter((int) letter.Key);
            }
            //Debug.Log("Complete word : " + word);

            if (CheckWord(word))
            {
                wordFoundList.Add(word);
                wordDisplay.AddWordToList(word);
                audio.PlayWordFound();
                gridDisplay.WriteWarning("");
            }
            startedWord = false;
            currentWord.Clear();
        }
    }

    //Check if the dice selected is adjacent from the previous dice
    private bool CheckDice(int diceIndex, int prevIndex)
    {
        int dY = diceIndex / 4;
        int dX = diceIndex % 4;

        int pY = prevIndex / 4;
        int pX = prevIndex % 4;

        if (dY == pY && (dX + 1 == pX || dX - 1 == pX))
        {
            return true;
        }
        if (dX == pX && (dY + 1 == pY || dY - 1 == pY))
        {
            return true;
        }
        if (dX + 1 == pX && dY + 1 == pY)
        {
            return true;
        }
        if (dX - 1 == pX && dY - 1 == pY)
        {
            return true;
        }
        if (dX + 1 == pX && dY - 1 == pY)
        {
            return true;
        }
        if (dX - 1 == pX && dY + 1 == pY)
        {
            return true;
        }
        return false;
    }

    private bool CheckWord(string word)
    {
        if (word.Length < 3)
        {
            //Debug.Log("Word not valid in length");
            gridDisplay.WriteWarning("Le mot doit faire plus de 2 lettres");
            return false;
        }
        if (!dictionary.FindWord(word))
        {
            //Debug.Log("Word not in dictionary : " + word);
            gridDisplay.WriteWarning("Le mot "+ word + " n'est pas dans le dictionnaire");
            return false;
        }
        if (wordFoundList.Contains(word))
        {
            //Debug.Log("Word already found : " + word);
            gridDisplay.WriteWarning("Le mot " + word + " a déjà été trouvé");
            return false;
        }

        return true;
    }

    public List<string> GetWordFound()
    {
        return wordFoundList;
    }

    public void ClearWordList()
    {
        wordFoundList.Clear();
        wordDisplay.ClearList();
    }

    public void BlockGrid()
    {
        gridBlocker.SetActive(true);
    }

    public void UnblockGrid()
    {
        gridBlocker.SetActive(false);
    }

    public void ClearWarning()
    {
        gridDisplay.WriteWarning("");
    }
}
