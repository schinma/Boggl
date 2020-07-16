using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trie
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> children;
        public bool endOfWord;

        public TrieNode()
        {
            children = new Dictionary<char, TrieNode>();
            endOfWord = false;
        }
    }

    TrieNode root;
    
    public Trie()
    {
        root = new TrieNode();
    }

    public void AddWord(string word)
    {
        TrieNode tempRoot = root;

        for (int i = 0; i < word.Length; i++)
        {
            TrieNode newNode;
            if (tempRoot.children.ContainsKey(word[i]))
            {
                tempRoot = tempRoot.children[word[i]];
            } else
            {
                newNode = new TrieNode();
                if (i == word.Length -1)
                {
                    newNode.endOfWord = true;
                }

                tempRoot.children.Add(word[i], newNode);
                tempRoot = newNode;
            }
        }
    }

    public bool FindWord(string word)
    {
        TrieNode tempRoot = root;

        for (int i = 0; i < word.Length; i++)
        {
            if (tempRoot.children.ContainsKey(word[i]))
            {
                tempRoot = tempRoot.children[word[i]];

                if (i == word.Length - 1)
                {
                    if (tempRoot.endOfWord == true)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public void AddWordList(string[] words)
    {
       foreach(string word in words)
       {
            AddWord(word);
       }
    }
}
