using System;
using System.Collections.Generic;
using System.Text;

namespace Life40Days
{
    public class Fame
    {
        public UInt16 Value { get; private set; }

        public delegate void FameChangeHandler();
        public event FameChangeHandler FameChanged;
        public void OnFameChanged() => FameChanged?.Invoke();

        public Fame(UInt16 value = 80) => Value = value;

        public void Increase(UInt16 value)
        { Value += value; Value = Value > 100 ? (UInt16)100 : Value; OnFameChanged(); }

        public void Decrease(UInt16 value)
        { Value = Value <= value ? (UInt16)0 : (UInt16)(Value - value); OnFameChanged(); }
    }
}
