using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoSa.Land.Parcel
{
    public class TextNeighbors
    {

        private static string[] letters = new string[]
        {
            "А", "Б", "В", "Г", "Д", "Е", "Є", "Ж", "З", "И",
            "І", "Ї", "Й", "К", "Л", "М", "Н", "О", "П", "Р",
            "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ю", "Я"
        };

        private int currentLette;


        private Dictionary<string, string> mapText;

        public int Count
        {
            get
            {
                return (this.currentLette + 1);
            }
        }

        public TextNeighbors(int indexLetters)
        {
            this.currentLette = indexLetters;
            this.mapText = new Dictionary<string, string>();
        }

        public TextNeighbors()
        {
            this.currentLette = -1;
            this.mapText = new Dictionary<string, string>();
        }

        public void addText(string value)
        {
            this.currentLette += 1;
            int index = this.currentLette / 31;

            string key = letters[this.currentLette - index * 31];

            if (index > 0) { key += index.ToString();  }

            mapText.Add(key, value);
        }

        public List<string> ToListValue()
        {
            if (mapText.Count < 0) return null;

            List<string> listLit = mapText.Keys.ToList();
            List<string> listTxt = mapText.Values.ToList();

            List<string> list = new List<string>();

            int indexTxt = 0;
            foreach (KeyValuePair<string, string> entry in mapText)
            {
                indexTxt++;
                if (indexTxt < mapText.Count )
                {
                    list.Add( entry.Key + "-" + listLit[indexTxt] + "\t" + entry.Value);
                }
                else if (indexTxt == mapText.Count)
                {
                    list.Add(entry.Key + "-" + listLit[0] + "\t" + entry.Value);
                }
            }
            return list;
        }

        public KeyValuePair<string,string> GetLastText()
        {
            return mapText.LastOrDefault(); ;
        }

    }
}
