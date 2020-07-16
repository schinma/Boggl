using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVLoader
{
    public TextAsset csvfile;
    public string lineSeparator = "\n|\r|\r\n";
    public string fieldSeparator = "\",\"";
    public char surround = '"';

    public void loadCSV()
    {
        csvfile = Resources.Load<TextAsset>("localisation");
    }

    public Dictionary<string, string> ParseCSVFile(string attributeId)
    {
        Dictionary<string, string> dictionnary = new Dictionary<string, string>();

        string[] lines = Regex.Split(csvfile.text, lineSeparator);

        string[] headers = Regex.Split(lines[0], fieldSeparator);

        int attributeIndex = -1;

        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(attributeId))
            {
                attributeIndex = i;
                break;
            }
        }

        for (int i= 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] fields = Regex.Split(line, ",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            for (int j= 0; j < fields.Length; j++)
            {
                fields[j] = fields[j].TrimStart(' ', surround);
                fields[j] = fields[j].TrimEnd(surround);
            }

            if (fields.Length > attributeIndex)
            {
                string key = fields[0];

                if (dictionnary.ContainsKey(key))
                {
                    continue;
                }

                string value = fields[attributeIndex];
                dictionnary.Add(key, value);
            }

        }
        return dictionnary;
    }
}
