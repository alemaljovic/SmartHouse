using System;

namespace Devices
{
    public class SmartBulb : SmartDevice
    {
        public enum Tone
        {
            NEUTRAL,
            COLD,
            WARM
        }

        private Tone color;
        private double size;

        // Constructors
        public SmartBulb() : base()
        {
            this.color = Tone.NEUTRAL;
            this.size = 5.0;
        }

        public SmartBulb(int id, string name, State state, double baseCost, Tone tone, double size)
            : base(id, name, state, baseCost)
        {
            this.color = tone;
            this.size = size;
        }

        public SmartBulb(SmartBulb other) : base(other)
        {
            this.color = other.color;
            this.size = other.size;
        }
        public void SetColor(Tone newTone) => this.color = newTone;
        public void SetSize(double newSize) => this.size = newSize;

        public Tone GetColor() => this.color;
        public double GetSize() => this.size;

        public override double GetConsumptionPerDay()
        {
            return color switch
            {
                Tone.NEUTRAL => (GetBaseCost() * 0.3 + size / 10),
                Tone.COLD => (GetBaseCost() * 0.5 + size / 10),
                Tone.WARM => (GetBaseCost() * 0.7 + size / 10),
                _ => 0
            };
        }
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            if (!base.Equals(obj)) return false;

            SmartBulb that = (SmartBulb)obj;
            return this.color == that.color && this.size == that.size;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), color, size);
        }

        public override string ToString()
        {
            return $"SmartBulb{{ color={color}, size={size} }}";
        }

        public override SmartDevice Clone()
        {
            return new SmartBulb(this);
        }
    }
}
