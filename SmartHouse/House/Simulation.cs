using System;
using System.Collections.Generic;
using System.Linq;

namespace HouseNamespace
{
    [Serializable] 
    public class Simulation
    {
        private readonly Dictionary<string, EnergyProvider> energyProviders;
        private readonly Dictionary<int, House> houses;
        private DateTime currentDate;
        public Simulation(List<EnergyProvider> providers, List<House> houses)
        {
            this.energyProviders = new Dictionary<string, EnergyProvider>();
            foreach (var e in providers) this.energyProviders[e.NameId] = e;

            this.houses = new Dictionary<int, House>();
            foreach (var h in houses) this.houses[h.OwnerNIF] = h;

            this.currentDate = DateTime.Now;
        }
        public DateTime CurrentDate => this.currentDate;

        public void SetDeviceToStateByHouse(int houseOwnerNIF, string roomName, int deviceId, SmartDevice.State setState)
        {
            if (!houses.ContainsKey(houseOwnerNIF))
                throw new NonExistentHouseException("House id (NIF) was not found or is not valid.\n");

            var house = houses[houseOwnerNIF];

            if (deviceId == 0) 
                house.SetRoomOn(roomName, setState);
            else
                house.SetDeviceStateByRoomName(deviceId, roomName, setState);
        }
        public void Simulate(DateTime jumpTo)
        {
            if (jumpTo < currentDate)
                throw new InvalidDateIntervalException("Given date is prior to current date.\n Make sure that the given date is after the current date.\n");

            long daysDifference = (long)(jumpTo - currentDate).TotalDays;

            foreach (var house in houses.Values)
            {
                var houseBill = new Bill
                {
                    DeviceNum = house.GetTotalDevices(),
                    HouseOwner = house.OwnerNIF,
                    FromDate = currentDate,
                    IssueDate = jumpTo,
                    TotalCost = house.CalculateBill(daysDifference),
                    PowerUsed = house.CalculatePowerUsed(daysDifference)
                };

                energyProviders[house.GetEnergyProvider().NameId].AddBill(houseBill);
            }

            currentDate = jumpTo;
        }
        public string ListAll()
        {
            var sb = new StringBuilder("Available Houses (Showing Rooms):\n");

            foreach (var h in houses.Values)
            {
                sb.AppendLine($"{h.OwnerName} - {h.OwnerNIF}");
                foreach (var r in h.GetRooms())
                {
                    sb.AppendLine($"   * {r.GetName()}");
                    foreach (var d in r.GetDevices().Values)
                        sb.AppendLine($"      + {d.DeviceId} {d.GetType().Name} {d.DeviceState}");
                }
            }

            return sb.ToString();
        }

        public string ListHouses()
        {
            var sb = new StringBuilder("Available Houses (id:total_rooms total_devices):\n");

            foreach (var h in houses.Values)
                sb.AppendLine(h.ToString());

            return sb.ToString();
        }

        public string ListHousesRooms()
        {
            var sb = new StringBuilder("Available Houses (Showing Rooms):\n");

            foreach (var h in houses.Values)
            {
                sb.AppendLine($"{h.OwnerName} - {h.OwnerNIF}");
                foreach (var r in h.GetRooms())
                    sb.AppendLine($"   * {r.GetName()}");
            }

            return sb.ToString();
        }

        public string ListEnergyProviders()
        {
            var sb = new StringBuilder("Available Energy Providers:\n");

            foreach (var e in energyProviders.Values)
                sb.AppendLine($"{e.NameId} - {e.BaseCost} - {e.TaxMargin} - {e.Formula}");

            return sb.ToString();
        }

        public string ListBillsFromProvider(string providerName)
        {
            if (energyProviders.ContainsKey(providerName))
                return energyProviders[providerName].GetBillsAsString();
            else
                return $"Provider name does not exist: {providerName}\n";
        }
        public string GetAvailableHousesAsString()
        {
            var sb = new StringBuilder("[?] Please choose from one of the houses below:\n");

            foreach (var house in houses.Values)
                sb.AppendLine($"   [*] {house.OwnerName} {house.OwnerNIF}");

            sb.Append("[>] House NIF (int): ");
            return sb.ToString();
        }

        public string GetAvailableRoomsFromHouseAsString(int houseIdentification)
        {
            if (!houses.ContainsKey(houseIdentification))
                throw new NonExistentHouseException("House id (NIF) was not found or is not valid.\n");

            var sb = new StringBuilder("[?] Please enter a room name:\n");
            var currentHouse = houses[houseIdentification];

            foreach (var room in currentHouse.GetRooms())
                sb.AppendLine($"   [*] {room.GetName()}");

            sb.Append("[>] Room name (string): ");
            return sb.ToString();
        }

        public string GetAvailableDevicesFromRoom(int houseId, string roomName)
        {
            var currentRoom = houses[houseId].GetRoom(roomName);
            var sb = new StringBuilder("[?] Please enter a device to switch:\n");

            foreach (var device in currentRoom.GetDevices().Values)
                sb.AppendLine($"   [*] {device.DeviceId} - {device.GetType().Name} - {device.DeviceState}");

            sb.Append("[>] Device id (int, if 0 switch the room): ");
            return sb.ToString();
        }

        public string GetAvailableProvidersAsString()
        {
            var sb = new StringBuilder("\n");

            foreach (var provider in energyProviders.Values)
                sb.AppendLine($"   [*] {provider.NameId}");

            sb.Append("[>] Provider name (string): ");
            return sb.ToString();
        }
        public void ChangeBaseCostProvider(string providerName, double baseCost)
        {
            if (!energyProviders.ContainsKey(providerName))
                throw new NonexistentProviderException($"Provider named {providerName} does not exist.\n");

            energyProviders[providerName].BaseCost = baseCost;
        }

        public void ChangeTaxProvider(string providerName, double tax)
        {
            if (!energyProviders.ContainsKey(providerName))
                throw new NonexistentProviderException($"Provider named {providerName} does not exist.\n");

            energyProviders[providerName].TaxMargin = tax;
        }

        public void ChangeFormulaProvider(string providerName, string formula)
        {
            if (!energyProviders.ContainsKey(providerName))
                throw new NonexistentProviderException($"Provider named {providerName} does not exist.\n");

            energyProviders[providerName].Formula = formula;
        }

        public void SetHouseProviderById(int houseId, string energyProvider)
        {
            if (!houses.ContainsKey(houseId))
                throw new NonExistentHouseException($"House with NIF {houseId} does not exist.\n");

            if (!energyProviders.ContainsKey(energyProvider))
                throw new NonexistentProviderException($"Provider {energyProvider} does not exist.\n");

            houses[houseId].SetEnergyProvider(energyProviders[energyProvider]);
        }
        public House QueryHouseSpentMost()
        {
            var houseCostMap = new Dictionary<int, double>();

            foreach (var provider in energyProviders.Values)
            {
                foreach (var bill in provider.GetBills())
                {
                    if (!houseCostMap.ContainsKey(bill.HouseOwner))
                        houseCostMap[bill.HouseOwner] = bill.TotalCost;
                    else
                        houseCostMap[bill.HouseOwner] += bill.TotalCost;
                }
            }

            var maxValueNIF = houseCostMap.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            return houses[maxValueNIF];
        }

        public EnergyProvider GetProviderMostBills()
        {
            var providersSumValuesMap = new Dictionary<string, double>();

            foreach (var provider in energyProviders.Values)
            {
                double sumTotals = provider.GetBills().Sum(bill => bill.TotalCost);
                providersSumValuesMap[provider.NameId] = sumTotals;
            }

            var providerName = providersSumValuesMap.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            return energyProviders[providerName];
        }

        public Dictionary<int, double> LargestConsumerOnTimeInterval(DateTime start, DateTime finish)
        {
            if (start < currentDate || finish < start)
                throw new InvalidDateIntervalException("Date interval is not valid, either the start is before the current date or finish is before start.\n");

            var housesCosts = new Dictionary<int, double>();

            foreach (var provider in energyProviders.Values)
            {
                foreach (var bill in provider.GetBills())
                {
                    if (bill.IsBetweenDates(start, finish))
                    {
                        if (!housesCosts.ContainsKey(bill.HouseOwner))
                            housesCosts[bill.HouseOwner] = bill.TotalCost;
                        else
                            housesCosts[bill.HouseOwner] += bill.TotalCost;
                    }
                }
            }

            var sortedMap = housesCosts.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return sortedMap;
        }

        public string DisplayQueryResultOne(House resultHouse)
        {
            return $"[!] Results for query 'House that spent the most until now.':\n" +
                   $"   House:\n" +
                   $"     * Owner: {resultHouse.OwnerName}\n" +
                   $"     * NIF: {resultHouse.OwnerNIF}\n";
        }

        public string DisplayQueryResultTwo(EnergyProvider provider)
        {
            return $"[!] Results for query 'Provider that cashed more.':\n" +
                   $"   Energy Provider:\n" +
                   $"      * Name: {provider.NameId}\n";
        }

        public string DisplayQueryResultThree(Dictionary<int, double> resultMap)
        {
            var sb = new StringBuilder("[!] Results for query 'List of consumers ordered by most spent.':\n");

            int counter = 1;
            foreach (var entry in resultMap)
            {
                sb.AppendLine($"   [{counter}º] House of: {houses[entry.Key].OwnerName} - {entry.Value}€");
                counter++;
            }

            return sb.ToString();
        }
    }
}