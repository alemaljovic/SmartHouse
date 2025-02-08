using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseNamespace
{
    [Serializable]
    public class House
    {
        private string ownerName;
        private int ownerNIF;
        private EnergyProvider energyProvider;
        private readonly List<Room> rooms;
        public House()
        {
            this.ownerName = "Unknown";
            this.ownerNIF = 0;
            this.rooms = new List<Room>();
            this.energyProvider = new EnergyProvider();
        }

        public House(string owner, int nif, List<Room> rooms, EnergyProvider provider)
        {
            this.ownerName = owner;
            this.ownerNIF = nif;
            this.rooms = rooms.Select(r => r.Clone()).ToList();
            this.energyProvider = provider;
        }

        public House(House other)
        {
            this.energyProvider = other.GetEnergyProvider();
            this.rooms = other.GetRooms().Select(r => r.Clone()).ToList();
            this.ownerName = other.GetOwnerName();
            this.ownerNIF = other.GetOwnerNIF();
        }
        public string GetOwnerName() => ownerName;
        public void SetOwnerName(string ownerName) => this.ownerName = ownerName;

        public int GetOwnerNIF() => ownerNIF;
        public void SetOwnerNIF(int ownerNIF) => this.ownerNIF = ownerNIF;

        public List<Room> GetRooms() => rooms.Select(r => r.Clone()).ToList();
        public void AddRoom(Room room) => this.rooms.Add(room.Clone());

        public EnergyProvider GetEnergyProvider() => energyProvider;
        public void SetEnergyProvider(EnergyProvider provider) => this.energyProvider = provider;

        public long GetTotalDevices() => rooms.Sum(room => room.GetTotalDevices());
        public int GetTotalRooms() => rooms.Count;

        public Room GetRoom(string roomName)
        {
            return rooms.FirstOrDefault(room => room.GetName() == roomName) ?? throw new NonexistentRoomException($"The room {roomName} does not exist in the house: {this.ownerNIF}");
        }
        public void SetRoomOn(string roomName, SmartDevice.State state)
        {
            var currentRoom = rooms.FirstOrDefault(room => room.GetName() == roomName);
            if (currentRoom == null)
                throw new NonexistentRoomException($"The room {roomName} does not exist in the house: {this.ownerNIF}");

            currentRoom.SetAllDevicesState(state);
        }

        public void SetDeviceStateOnRoom(int deviceId, SmartDevice.State state)
        {
            foreach (var room in rooms)
                if (room.DeviceExists(deviceId)) room.SetDeviceState(state, deviceId);
        }

        public void SetDeviceStateByRoomName(int deviceId, string roomName, SmartDevice.State state)
        {
            var room = rooms.FirstOrDefault(r => r.GetName() == roomName);
            if (room != null)
                room.SetDeviceState(state, deviceId);
            else
                throw new NonexistentDeviceException($"Device {deviceId} not found in room {roomName}.");
        }

        public double CalculateBill(long days)
        {
            return rooms.Sum(r => r.GetRoomConsumption(
                energyProvider.GetBaseCost(),
                energyProvider.GetTaxMargin(),
                energyProvider.GetFormula(), days));
        }

        public double CalculatePowerUsed(long days)
        {
            return rooms.Sum(room => room.GetPowerConsumption(days));
        }
        public override bool Equals(object obj)
        {
            return obj is House house &&
                   energyProvider.Equals(house.GetEnergyProvider()) &&
                   ownerNIF == house.GetOwnerNIF() &&
                   ownerName == house.GetOwnerName() &&
                   rooms.SequenceEqual(house.GetRooms());
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ownerName, ownerNIF);
        }

        public override string ToString()
        {
            return $"[House] Owner Name: {ownerName}, NIF: {ownerNIF}, Energy Provider: {energyProvider}\n";
        }

        public House Clone()
        {
            return new House(this);
        }
    }
}
