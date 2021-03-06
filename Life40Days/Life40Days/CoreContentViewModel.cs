﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MicroMvvm;
using System.Windows.Media;

namespace Life40Days
{
    public class IconButton
    {
        // Fontawesome cheatsheet:
        // https://fontawesome.com/cheatsheet?from=io
        public String Glyph { get; }
        public String Text { get; }
        public IconButton(String glyph, String text)
        { Glyph = glyph; Text = text; }
    }
    public class CoreContentViewModel : ObservableObject
    {        
        public IconButton BankButton { get; } = new IconButton("\uf3d1", "Bank");
        public IconButton PostButton { get; } = new IconButton("\uf674", "Post");
        public IconButton ClinicButton { get; } = new IconButton("\uf47d", "Clinic");
        public IconButton HAgencyButton { get; } = new IconButton("\uf015", "House Agency");
        public IconButton NetBarButton { get; } = new IconButton("\uf6ff", "Net Coffee");

        private ObservableCollection<MarketGoods> _blackMarketList;
        public ObservableCollection<MarketGoods> BlackMarketList
        {
            get => _blackMarketList;
            set => Set(ref _blackMarketList, value);
        }

        private ObservableCollection<InventoryGoods> _inventoryList;
        public ObservableCollection<InventoryGoods> InventoryList
        {
            get => _inventoryList;
            set => Set(ref _inventoryList, value);
        }

        private UInt16 _myHealth;
        public UInt16 MyHealth
        {
            get => _myHealth;
            set => Set(ref _myHealth, value);
        }

        private UInt16 _myFame;
        public UInt16 MyFame
        {
            get => _myFame;
            set => Set(ref _myFame, value);
        }

        private Double _myCash;
        public Double MyCash
        {
            get => _myCash;
            set => Set(ref _myCash, value);
        }

        private Double _myBank;
        public Double MyBank
        {
            get => _myBank;
            set => Set(ref _myBank, value);
        }

        private Double _myDebt;
        public Double MyDebt
        {
            get => _myDebt;
            set => Set(ref _myDebt, value);
        }

        private String _inventoryLabel;
        public String InventoryLable
        {
            get => _inventoryLabel;
            set => Set(ref _inventoryLabel, value);
        }

        private String _daysLeftLabel;
        public String DaysLeftLabel
        {
            get => _daysLeftLabel;
            set => Set(ref _daysLeftLabel, value);
        }



        private MyStatus _myStatus;
        private Market _market;
        private Location _location;

        public MyStatus MyStatus 
        {
            get => _myStatus;
            set => Set(ref _myStatus, value);
        }
        public Market Market
        {
            get => _market;
            set => Set(ref _market, value);
        }
        public Location Location
        {
            get => _location;
            set => Set(ref _location, value);
        }

        public CoreContentViewModel()
        {
            // initialize mystatus
            MyStatus = new MyStatus();

            // initialize market
            Market = new Market();

            Location = new Location();
            Location.GoodsOnLocation(Market.AllGoods, 3);
            Location.ChangeLocation(BJLocations.Beijingzhan);

            OnDataChanged();
        }

        public void OnDataChanged()
        {
            BlackMarketList = new ObservableCollection<MarketGoods>(Location.GoodsList);
            InventoryList = new ObservableCollection<InventoryGoods>(MyStatus.MyInventory.MyGoods);

            MyHealth = MyStatus.MyHealth.HP;
            MyFame = MyStatus.MyFame.Value;
            MyCash = MyStatus.MyFinance.MyCash;
            MyBank = MyStatus.MyFinance.MyBank;
            MyDebt = MyStatus.MyFinance.MyDebt;

            InventoryLable = $"My Inventory: {MyStatus.MyInventory.ToString()}";
            DaysLeftLabel = $"Life in Beijing (Day {41 - MyStatus.MyDaysLeft}/40)";
        }

        public ICommand ChangeLocationCommand { get => new RelayCommand<BJLocations>(place => UpdateLocationExecute(place), CanUpdateLocationExecute); }

        private bool CanUpdateLocationExecute(BJLocations param) => true;

        private void UpdateLocationExecute(BJLocations place)
        {
            if (place == Location.LocationName) return;

            Location.GoodsOnLocation(Market.AllGoods, 3);
            Location.ChangeLocation(place);

            MyStatus.UpdateMyStatus(place);

            OnDataChanged();
        }

        public ICommand BuyGoodsCommand { get => new RelayCommand<MarketGoods>(goods => BuyGoodsExecute(goods), CanBuyGoodsExecute); }
        private bool CanBuyGoodsExecute(MarketGoods obj) => true;
        private void BuyGoodsExecute(MarketGoods goods)
        {
            if (goods == null) return;

            // TODO: input goods_to_buy from UI
            // assume to buy 10 products
            var tg = new TradePage(
                "Buy from Black market", "How many do you want to buy:",
                10, 0, goods.GoodsCount,
                "Buy", "I changed my mind");
            if (tg.ShowDialog() ?? true) return;
            if (!tg.TVM.IsConfirm ?? false) return;

            Int32 goods_to_buy = (Int32)tg.TVM.Value;

            // no enough goods to buy
            if (goods.GoodsCount < goods_to_buy) return;

            // no enough cash to buy
            if (MyStatus.MyFinance.MyCash < goods.GoodsPrice * goods_to_buy) return;
            

            if( !goods.MarketGoodsBuyIn(goods_to_buy) ||
                !MyStatus.MyInventory.AddGoods(goods, goods_to_buy) ||
                !MyStatus.MyFinance.CashUse(goods.GoodsPrice * goods_to_buy)) return;

            OnDataChanged();
        }

        public ICommand SellGoodsCommand { get => new RelayCommand<InventoryGoods>(goods => SellGoodsExecute(goods), CanSellGoodsExecute); }
        private bool CanSellGoodsExecute(InventoryGoods obj) => true;
        private void SellGoodsExecute(InventoryGoods goods)
        {
            // TODO: input goods_to_buy from UI
            // assume to sell 10 products
            Int32 goods_to_sell = 10;

            if (goods == null) return;

            // no such goods in market
            if (!Location.GoodsList.Exists(t => t.GoodsName == goods.GoodsName)) return;

            // no enough goods to sell
            if (goods.GoodsCount < goods_to_sell) return;

            if (!MyStatus.MyInventory.RemoveGoods(goods, goods_to_sell)) return;

            // how much should I get?
            var g = Location.GoodsList.Find(t => t.GoodsName == goods.GoodsName);
            var market_price = g.GoodsPrice;

            MyStatus.MyFinance.CashIncrease(market_price * goods_to_sell);

            OnDataChanged();
        }

    }

}
