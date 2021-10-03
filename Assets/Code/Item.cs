using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
    class Item
    {
        public String name;
        public String desc;
        public Sprite sprite;

        public Item(string name, string desc, Sprite sprite)
        {
            this.name = name;
            this.desc = desc;
            this.sprite = sprite;
        }
    }
}
