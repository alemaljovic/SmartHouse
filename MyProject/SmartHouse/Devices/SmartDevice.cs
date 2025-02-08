using System;

public abstract class SmartDevice
{
    public enum State
    {
        ON,
        OFF
    }

    private int deviceId;
    private string deviceName;
    private State deviceState;
    private double baseCost;

    /* Constructors */
    public SmartDevice()
    {
        this.deviceId = 0;
        this.deviceName = "empty";
        this.deviceState = State.OFF;
        this.baseCost = 0;
    }

    public SmartDevice(int id, string name, State state, double baseCost)
    {
        this.deviceState = state;
        this.deviceName = name;
        this.baseCost = baseCost < 0 ? Math.Abs(baseCost) : baseCost;

        if (id < 0)
            throw new ArgumentException("Device id must be a positive integer.");
        else
            this.deviceId = id;
    }

    public SmartDevice(SmartDevice device)
    {
        this.deviceId = device.deviceId;
        this.deviceName = device.deviceName;
        this.deviceState = device.deviceState;
        this.baseCost = device.baseCost;
    }

    public State DeviceState { get => deviceState; set => deviceState = value; }
    public int DeviceId { get => deviceId; set => deviceId = value; }
    public string DeviceName { get => deviceName; set => deviceName = value; }
    public double BaseCost { get => baseCost; set => baseCost = value; }

    public override string ToString()
    {
        return $"SmartDevice{{ deviceId={deviceId}, deviceName='{deviceName}', deviceState={deviceState} }}";
    }

    public override bool Equals(object obj)
    {
        if (this == obj) return true;
        if (obj == null || GetType() != obj.GetType()) return false;

        SmartDevice that = (SmartDevice)obj;
        return deviceId == that.deviceId &&
               deviceName == that.deviceName &&
               deviceState == that.deviceState &&
               baseCost == that.baseCost;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(deviceId, deviceName, deviceState, baseCost);
    }

    public abstract SmartDevice Clone();
    public abstract double GetConsumptionPerDay();
}
