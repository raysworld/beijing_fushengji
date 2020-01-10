using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Life40Days
{
    public class MyInventory
    {
        public List<InventoryGoods> MyGoods { get; set; }

        public Int32 MaxCapacity { get; private set; }
        public Int32 CurCapacity
        {
            get
            {
                Int32 sum = 0;
                MyGoods.ForEach(t => sum += t.GoodsCount);                
                return sum;
            }
        }
        public Int32 LeftCapacity => MaxCapacity - CurCapacity;

        public delegate void InventoryGoodsSellHandler(Goods goods);
        public event InventoryGoodsSellHandler InventoryGoodsSold;
        public void OnInventoryGoodsSold(Goods goods) => InventoryGoodsSold?.Invoke(goods);

        public MyInventory(Int32 max = 50)
        {
            MyGoods = new List<InventoryGoods>();
            MaxCapacity = max;
        }


        public override string ToString() => $"{CurCapacity} / {MaxCapacity}";

        public Boolean AddGoods(MarketGoods goods, Int32 count)
        {
            if (goods.GoodsCount == -1 || goods.GoodsCount > count) // enough goods to add
            {
                if (count <= LeftCapacity) // enough room to place the goods
                {
                    if (MyGoods.Exists(t=>t.GoodsName==goods.GoodsName))
                    {
                        var g = MyGoods.Find(t => t.GoodsName == goods.GoodsName);
                        g.InventoryGoodsAdd(goods, count);
                    }
                    else
                    {
                        var g = new InventoryGoods(goods, count);
                        MyGoods.Add(g);
                    }
                    return true;
                }
            }
            // no enough goods or no enough space
            return false;
        }

        public Boolean RemoveGoods(InventoryGoods goods, Int32 count)
        {
            // check if the goods in inventory
            if (MyGoods.Exists(t => t.GoodsName == goods.GoodsName))
            {
                // check if goods count > count
                InventoryGoods ig = MyGoods.Find(t => t.GoodsName == goods.GoodsName);
                if (ig.GoodsCount > count)
                {
                    ig.InventoryGoodsSellOut(count);
                    OnInventoryGoodsSold(goods);
                    return true;
                }
                else if (ig.GoodsCount == count)
                {
                    MyGoods.Remove(ig);
                    OnInventoryGoodsSold(goods);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // no such goods or no enough goods to take out
                return false;
            }
        }
    }

    public class Market
    {
        public List<MarketGoods> AllGoods { get; private set; }

        public Market()
        {
            AllGoods = new List<MarketGoods>();

            AllGoods.Add(new MarketGoods("进口香烟", 100, 350));
            AllGoods.Add(new MarketGoods("走私汽车", 15000, 15000));
            AllGoods.Add(new MarketGoods("盗版VCD、游戏", 5, 50));
            AllGoods.Add(new MarketGoods("假白酒（剧毒！）", 1000, 2500, -1, 10));
            AllGoods.Add(new MarketGoods("《上海小宝贝》（禁书）", 5000, 9000, -1, 7));
            AllGoods.Add(new MarketGoods("进口玩具", 250, 600));
            AllGoods.Add(new MarketGoods("水货手机", 750, 750));
            AllGoods.Add(new MarketGoods("伪劣化妆品", 65, 180));
        }
        public void MarketPriceFloat()
            => AllGoods.ForEach(t => t.MarketGoodsChangePrices());

        // Override the ToString method.
        public override string ToString()
        {
            String price_list = "";
            AllGoods.ForEach(t => price_list += t.ToString());

            return price_list;
        }
    }

    public class Location
    {
        public BJLocations LocationName { get; private set; } = BJLocations.Nowhere;
        public List<MarketGoods> GoodsList { get; private set; } = new List<MarketGoods>();

        public void GoodsOnLocation(List<MarketGoods> all_goods, int leaveout = 0)
        {
            var rand = new Random();
            var list = new List<MarketGoods>();

            GoodsList.Clear();

            for (int i = 0; i < leaveout; i++)
            {
                var t = all_goods[rand.Next(0, all_goods.Count - 1)];
                list.Add(t);
            }

            list.Distinct().ToList().ForEach(t => GoodsList.Add(t));
        }

        public void ChangeLocation(BJLocations name)
        {
            LocationName = name;
            GoodsList.ForEach(t => t.MarketGoodsChangeCount());
            GoodsList.ForEach(t => t.MarketGoodsChangePrices());
        }
    }
}
