using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dronee_Chan_2.Discord_Bot.Objects.UserObjects
{
    public class Item
    {
        public int ID { get; private set; }  // Unique item ID
        public string Name { get; private set; }   // Item name
        public string Icon { get; private set; }  // Item icon name
        public int BuyPrice { get; private set; }  // Buy price
        public int SellPrice { get; private set; }   // Sell price
        public string Description { get; private set; }   // Item description

        public Item(int iD, string name, string icon, int buyPrice, int sellPrice, string description)
        {
            ID = iD;
            Name = name;
            Icon = icon;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
            Description = description;
        }
    }
}
