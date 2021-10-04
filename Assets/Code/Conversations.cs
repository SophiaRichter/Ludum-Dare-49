using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code
{
    class Conversations : UnityEngine.MonoBehaviour
    {

        public Conversations()
        { 
            
        }

        public bool loadConversations()
        {

            UnityEngine.PlayerPrefs.DeleteAll();

            string path = @"./Assets/Conversations/Conversations.txt";
            
            if (!File.Exists(path))
            {
                UnityEngine.Debug.Log("Couldnt load conversation file");
            }

            // Open the file to read from.
            string readText = File.ReadAllText(path);
            string[] stringSep = new string[] { Environment.NewLine };
            string[] texts = readText.Split(stringSep, StringSplitOptions.None);

            foreach (string s in texts)
            {
                string[] speakerNtext = s.Split(':');
                if (speakerNtext.Length < 2) continue;
                UnityEngine.PlayerPrefs.SetString(speakerNtext[0],speakerNtext[1]);
            }

            return true;

        }
    }
}
