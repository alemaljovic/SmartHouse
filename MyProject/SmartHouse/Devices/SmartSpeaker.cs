using System;

public class SmartSpeaker : SmartDevice
{
    public const int MAX_VOLUME = 100;
    private int speakerVolume;
    private string channel;
    private string speakerBrand;

    public SmartSpeaker() : base()
    {
        this.speakerVolume = 50;
        this.channel = "";
        this.speakerBrand = "";
    }

    public SmartSpeaker(int deviceId, string deviceName, State state, double baseCost, int speakerVolume, string channel, string speakerBrand) 
        : base(deviceId, deviceName, state, baseCost)
    {
        this.channel = channel;
        this.speakerBrand = speakerBrand;
        this.SpeakerVolume = speakerVolume;
    }

    public SmartSpeaker(SmartSpeaker other) : base(other)
    {
        this.speakerVolume = other.SpeakerVolume;
        this.channel = other.Channel;
        this.speakerBrand = other.SpeakerBrand;
    }

    public int SpeakerVolume 
    {
        get => speakerVolume;
        set => speakerVolume = Math.Clamp(value, 0, MAX_VOLUME);
    }

    public string SpeakerBrand 
    {
        get => speakerBrand;
        set => speakerBrand = value;
    }

    public string Channel 
    {
        get => channel;
        set => channel = value;
    }

    public override double GetConsumptionPerDay()
    {
        return BaseCost + speakerVolume;
    }

    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;
        if (!base.Equals(obj)) return false;

        SmartSpeaker other = (SmartSpeaker)obj;
        return speakerVolume == other.speakerVolume &&
               channel == other.channel &&
               speakerBrand == other.speakerBrand;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(speakerVolume, channel, speakerBrand);
    }

    public override string ToString()
    {
        return $"SmartSpeaker{{ speakerVolume={speakerVolume}, channel='{channel}', speakerBrand='{speakerBrand}' }}";
    }

    public override SmartDevice Clone()
    {
        return new SmartSpeaker(this);
    }
}
