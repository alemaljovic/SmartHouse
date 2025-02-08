using System;
using System.Collections.Generic;
using System.Linq;

public class Room
{
    private string name;
    private Dictionary<int, SmartDevice> devices;

    public Room()
    {
        this.name = "";
        this.devices = new Dictionary<int, SmartDevice>();
    }

    public Room(string name)
    {
        this.name = name;
        this.devices = new Dictionary<int, SmartDevice>();
    }

    public Room(Room room)
    {
        this.name = room.GetName();
        this.devices = room.GetDevices();
    }
    public long GetTotalDevices() => this.devices.Count;
    
    public string GetName() => this.name;
    
    public void SetName(string name) => this.name = name;
    
    public void SetDevices(Dictionary<int, SmartDevice> input)
    {
        foreach (var entry in input)
        {
            if (entry.Value != null) this.devices[entry.Key] = entry.Value.Clone();
        }
    }
    
    public Dictionary<int, SmartDevice> GetDevices()
    {
        var clonedDevices = new Dictionary<int, SmartDevice>();
        foreach (var entry in this.devices)
        {
            if (entry.Value != null) clonedDevices[entry.Key] = entry.Value.Clone();
        }
        return clonedDevices;
    }
    
    public bool DeviceExists(int id) => this.devices.ContainsKey(id);
    
    public bool DeviceExists(SmartDevice device) => this.devices.Values.Any(d => d.Equals(device));
    
    public void InsertDevice(SmartDevice device)
    {
        if (this.devices.ContainsKey(device.GetDeviceId()))
            throw new Exception($"Device ID ({device.GetDeviceId()}) already exists in room {this.name}");
        else
            this.devices[device.GetDeviceId()] = device;
    }
    
    public void SetAllDevicesState(SmartDevice.State state)
    {
        foreach (var device in this.devices.Values)
        {
            device.SetDeviceState(state);
        }
    }
    
    public void SetDeviceState(SmartDevice.State state, int deviceId)
    {
        if (!this.devices.TryGetValue(deviceId, out SmartDevice device))
            throw new Exception($"Device ({deviceId}) does not exist.");
        
        device.SetDeviceState(state);
    }
    
    public double GetRoomConsumption(double baseCost, double tax, string formula, long days)
    {
        double totalRoomCost = 0;
        foreach (var device in this.devices.Values)
        {
            if (device.GetDeviceState() == SmartDevice.State.On)
            {
                double deviceConsumption = device.GetConsumptionPerDay();
                string parsedFormula = formula.Replace("x", baseCost.ToString())
                                             .Replace("y", deviceConsumption.ToString())
                                             .Replace("z", tax.ToString());
                
                double result = EvaluateFormula(parsedFormula);
                totalRoomCost += result * days;
            }
        }
        return totalRoomCost;
    }
    
    public double GetPowerConsumption(long days)
    {
        return this.devices.Values
                   .Where(d => d.GetDeviceState() == SmartDevice.State.On)
                   .Sum(d => d.GetConsumptionPerDay() * days);
    }
        private double EvaluateFormula(string formula)
    {
        try
        {
            return Convert.ToDouble(new System.Data.DataTable().Compute(formula, null));
        }
        catch
        {
            return 0;
        }
    }
        public override bool Equals(object obj)
    {
        if (obj == this) return true;
        if (obj == null || obj.GetType() != this.GetType()) return false;

        Room room = (Room)obj;
        return this.name == room.GetName() &&
               this.devices.SequenceEqual(room.devices);
    }
    
    public override int GetHashCode() => HashCode.Combine(name, devices);
    
    public override string ToString()
    {
        return $"Room{{name='{name}', devices={devices.Count}}}";
    }
    
    public Room Clone() => new Room(this);
}
