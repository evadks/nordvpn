using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace partycli
{
    public class Program
    {
        private static readonly HttpClient Client = new HttpClient();

        private static void Main(string[] args)
        {
            var currentState = States.none;
            string name = null;
            var argIndex = 1;

            foreach (var arg in args)
            {
                if (currentState == States.none)
                {
                    if (arg == "server_list")
                    {
                        currentState = States.server_list;
                        if (argIndex >= args.Count())
                        {
                            var serverList = GetAllServersListAsync();
                            StoreValue("serverlist", serverList, false);
                            Log("Saved new server list: " + serverList);
                            displayList(serverList);
                        }
                    }
                    if (arg == "config")
                    {
                        currentState = States.config;
                    }
                }
                else if (currentState == States.config)
                {
                    if (name == null)
                    {
                        name = arg;
                    }
                    else
                    {
                        StoreValue(ProccessName(name), arg);
                        Log("Changed " + ProccessName(name) + " to " + arg);
                        name = null;
                    }
                }
                else if (currentState == States.server_list)
                {
                    if (arg == "--local")
                    {
                        if (!String.IsNullOrEmpty(Properties.Settings.Default.serverlist)) {
                            displayList(Properties.Settings.Default.serverlist);
                        } else
                        {
                            Console.WriteLine("Error: There are no server data in local storage");
                        }
                    }
                    else if (arg == "--france")
                    {
                        //france == 74
                        //albania == 2
                        //Argentina == 10
                        var query = new VpnServerQuery(null,74,null,null,null, null);
                        var serverList = GetAllServerByCountryListAsync(query.CountryId.Value); //France id == 74
                        StoreValue("serverlist", serverList, false);
                        Log("Saved new server list: " + serverList);
                        displayList(serverList);
                    }
                    else if (arg == "--TCP")
                    {
                        //UDP = 3
                        //Tcp = 5
                        //Nordlynx = 35
                        var query = new VpnServerQuery(5,null,null,null,null, null);
                        var serverList = getAllServerByProtocolListAsync((int)query.Protocol.Value);
                        StoreValue("serverlist", serverList, false);
                        Log("Saved new server list: " + serverList);
                        displayList(serverList);
                    }
                }
                argIndex = argIndex + 1;
            }

            if(currentState == States.none)
            {
                Console.WriteLine("To get and save all servers, use command: partycli.exe server_list");
                Console.WriteLine("To get and save France servers, use command: partycli.exe server_list --france");
                Console.WriteLine("To get and save servers that support TCP protocol, use command: partycli.exe server_list --TCP");
                Console.WriteLine("To see saved list of servers, use command: partycli.exe server_list --local ");
            }
            Console.Read();
        }

        static void StoreValue(string name, string value, bool writeToConsole = true)
        {
            try {
                var settings = Properties.Settings.Default;
                settings[name] = value;
                settings.Save();
                if (writeToConsole) {
                    Console.WriteLine("Changed " + name + " to " + value);
                }
            }
            catch {
                Console.WriteLine("Error: Couldn't save " + name + ". Check if command was input correctly." );
            }

        }

        static string ProccessName(string name)
        {
            name = name.Replace("-", string.Empty);
            return name;
        }

        private static string GetAllServersListAsync()
        {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://api.nordvpn.com/v1/servers");
                var response = Client.SendAsync(request).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
        }

        private static string GetAllServerByCountryListAsync(int countryId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=35&filters[country_id]=" + countryId);
            var response = Client.SendAsync(request).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        static string getAllServerByProtocolListAsync(int vpnProtocol)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.nordvpn.com/v1/servers?filters[servers_technologies][id]=" + vpnProtocol);
            var response = Client.SendAsync(request).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            return responseString;
        }

        static void displayList(string serverListString)
        {
            var serversList = JsonConvert.DeserializeObject<List<ServerModel>>(serverListString);
            Console.WriteLine("Server list: ");
            for (var index = 0; index < serversList.Count; index++)
            {
                Console.WriteLine("Name: " + serversList[index].Name);
            }
            Console.WriteLine("Total servers: " + serversList.Count);
        }

        static void Log(string action)
        {
            var newLog = new LogModel
            {
                Action = action,
                Time = DateTime.Now
            };
            List<LogModel> currentLog;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.log))
            {
                //currentLog = JsonConvert.DeserializeObject<List<LogModel>>(Properties.Settings.Default.log);
                //currentLog.Add(newLog);
            }
            else
            {
                currentLog = new List<LogModel> { newLog };
            }

            //StoreValue("log", JsonConvert.SerializeObject(currentLog), false);
        }
    }

    internal class VpnServerQuery
    {
             public int? Protocol { get; set; }

            public int? CountryId { get; set;}

            public int? CityId { get; set;}

            public int? RegionId { get; set;}

            public int? SpecificServiceId { get; set;}

            public int? ServerGroupId { get; set;}

            public VpnServerQuery(int? protocol, int? countryId, int? cityId, int? regionId, int? specificServiceId, int? serverGroupId)
            {
                Protocol = protocol;
                CountryId = countryId;
                CityId = cityId;
                RegionId = regionId;
                SpecificServiceId = specificServiceId;
                ServerGroupId = serverGroupId;
            }
    }

    internal class LogModel
    {
        public string Action { get; set; }
        public DateTime Time { get; set; }
    }

    internal class ServerModel
    {
        public string Name { get; set; }
        public int Load { get; set; }
        public string Status { get; set; }

    }

    internal enum States
    {
        none = 0,
        server_list = 1,
        config = 2,
    };
}
