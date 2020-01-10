using System;
using System.Collections.Generic;
using System.Text;

namespace Life40Days
{
    public enum HealthState { Perfect, Good, Sick, Dead }
    public class Health
    {
        private class HealthEvents
        {
            public UInt16 Frequency { get; }
            public String Msg { get; }
            public UInt16 Harm { get; }
            public String Sound { get; }

            public HealthEvents(UInt16 freq, String msg, UInt16 harm, String sound = "")
            {
                Frequency = freq;
                Msg = msg;
                Harm = harm;
                Sound = sound;
            }
        }

        private readonly HealthEvents[] _health_events =
        {
            new HealthEvents(117, "大街上两个流氓打了俺！", 3,"kill.wav"),
            new HealthEvents(157, "俺在过街地道被人打了蒙棍！", 20,"death.wav"),
            new HealthEvents(21,  "工商局的追俺超过三个胡同。", 1,"dog.wav"),
            new HealthEvents(100, "北京拥挤的交通让俺心焦！", 1,"harley.wav"),
            new HealthEvents(35,  "开小巴的打俺一耳光！", 1,"hit.wav"),
            new HealthEvents(313, "一群民工打了俺！", 10,"flee.wav"),
            new HealthEvents(120, "附近胡同的一个小青年砸俺一砖头！", 5,"death.wav"),
            new HealthEvents(29,  "附近写字楼一个假保安用电棍电击俺！",3,"el.wav"),
            new HealthEvents(43,  "北京臭黑的小河熏着我了！",1,"vomit.wav"),
            new HealthEvents(45,  "守自行车的王大婶嘲笑俺没北京户口！",1,"level.wav"),
            new HealthEvents(48,  "北京高温40度！俺热……",1,"lan.wav"),
            new HealthEvents(33,  "申奥添了新风景，北京又来沙尘暴！",1,"breath.wav")
        };

        public UInt16 HP { get; private set; }
        public String Message { get; private set; }
        public Boolean Hit { get; set; }
        public HealthState State
        {
            get
            {
                if (HP >= 80) return HealthState.Good;
                else if (HP > 0) return HealthState.Sick;
                else return HealthState.Dead;
            }
        }

        public delegate void HealthStateChangeHandler();
        public event HealthStateChangeHandler HealthStateChanged;

        public Health(UInt16 hp = 100) => HP = hp;

        public void Recover(UInt16 value)
        {
            HP += value;
            if (HP > 100) HP = 100;
        }

        public void UpdateHealthEvent()
        {
            var old_state = State;

            foreach (var e in _health_events)
            {
                //if (!(RandomNum(1000) % random_event[i].freq))
                Hit = new Random().Next(0, 1000) % e.Frequency == 0;
                if (Hit)
                {
                    // show health message
                    Message = $"{e.Msg}俺的健康减少了{e.Harm}点。";

                    HP -= e.Harm;

                    if (State != old_state) HealthStateChanged?.Invoke();

                    break;
                }
            }
        }
    }
}
