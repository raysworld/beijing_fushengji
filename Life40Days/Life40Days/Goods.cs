using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Life40Days
{

    public class Goods : IEquatable<Goods>, ICloneable
    {
        public String GoodsName { get; }
        public Double GoodsPrice { get; protected set; }
        public Int32 GoodsCount { get; protected set; }

        public Goods(String name, Double price, Int32 count)
        {
            GoodsName = name;
            GoodsPrice = price;
            GoodsCount = count;
        }

        public override string ToString() 
            => $"物品名：{GoodsName}，当前售价：{GoodsPrice}\n";

        public bool Equals([AllowNull] Goods other)
            => String.Equals(this.GoodsName, other.GoodsName);

        public object Clone() 
            => MemberwiseClone();
    }

    public class MarketGoods : Goods
    {
        private const Int32 goodsMaxCount = 9999;
        private readonly Double goodsBasicPrice;
        private readonly Double goodsFloatPrice;

        public MarketGoods(String name, Double b_price, Double f_price, Int32 count = -1)
            : base(name, b_price, count)
        {
            goodsBasicPrice = b_price;
            goodsFloatPrice = f_price;
        }

        public void MarketGoodsChangePrices()
            => GoodsPrice = goodsBasicPrice + new Random().NextDouble() * goodsFloatPrice;
        
        public void MarketGoodsChangeCount()
            => GoodsCount = new Random().Next(0, goodsMaxCount);

        public Boolean MarketGoodsBuyIn(Int32 count)
        {
            if (GoodsCount >= count)
            {
                GoodsCount -= count;
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class InventoryGoods : Goods
    {
        public InventoryGoods(String name, Double price, Int32 count)
            :base(name, price, count) { }

        public Boolean InventoryGoodsAdd(MarketGoods goods, Int32 count)
        {
            if (GoodsName != goods.GoodsName) return false; // not the same goods
            if (count > goods.GoodsCount) return false; // not enough goods

            GoodsPrice = (GoodsPrice * GoodsCount + goods.GoodsPrice * count) / (GoodsCount + count);
            GoodsCount = GoodsCount + count;

            return true;
        }

        public Boolean InventoryGoodsSellOut(Int32 count)
        {
            if (GoodsCount >= count)
            {
                GoodsCount -= count;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
