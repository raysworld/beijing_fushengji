using MicroMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Life40Days
{
    public enum BJLocations
    {
        Nowhere = -1,
        Jianguomen = 1,
        Beijingzhan = 2,
        Xizhimen = 3,
        Chongwenmen = 4,
        Dongzhimen = 5,
        Fuxingmen = 6,
        Jishuitan = 7,
        Changchunjie = 8,
        Gongzhufen = 9,
        Pingguoyuan = 10
    }

    public class MyStatus
    {
        public Finance MyFinance { get; private set; }
        public UInt16 MyHealth { get; private set; }
        public UInt16 MyFame { get; private set; }
        public UInt16 MyDaysLeft { get; private set; }
        public BJLocations MyLocation { get; private set; }
        public MyInventory MyInventory { get; private set; }

        public MyStatus()
        {
            MyFinance = new Finance(200000, 5000, 0);
            MyInventory = new MyInventory(50);

            MyHealth = 100;
            MyFame = 80;

            MyDaysLeft = 40;

            MyLocation = BJLocations.Nowhere;
        }

        public void UpdateMyStatus(BJLocations place)
        {
            if (MyLocation == place)
            {
                // already there, do not update
            }
            else
            {
                MyLocation = place;
                MyFinance.HandleBankAndDebt();

                //TODO: update my health and fame
                --MyHealth;
                --MyFame;

                --MyDaysLeft;
            }            
        }
    }

    public class Finance
    {
        public Double MyCash { get; private set; }
        public Double MyDebt { get; private set; }
        public Double MyBank { get; private set; }

        private Double bankRate;
        private Double debtRate;

        public Finance(Double cash, Double debt, Double bank, 
                       Double b_rate = 0.01, Double d_rate = 0.10)
        {
            MyCash = cash;
            MyDebt = debt;
            MyBank = bank;

            bankRate = b_rate;
            debtRate = d_rate;
        }

        public void GetCash(Double value) => MyCash += value;
        public Boolean UseCash(Double value)
        {
            if (MyCash > value)
            {
                MyCash -= value;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public void HandleBankAndDebt()
        {
            MyBank += MyBank * bankRate;
            MyDebt += MyDebt * debtRate;
        }

        public Boolean BankWithdraw(Double money)
        {
            if (MyBank > money)
            {
                MyBank -= money;
                MyCash += money;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean BankDeposit(Double money)
        {
            if (MyCash > money)
            {
                MyCash -= money;
                MyBank += money;
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean DebtPay(Double money)
        {
            if (MyCash > money)
            {
                MyCash -= money;
                MyDebt -= money;

                if (MyDebt < 0)
                {
                    MyCash -= MyDebt;
                    MyDebt = 0;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }



}
