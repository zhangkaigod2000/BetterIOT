using BetterIOT.Common;
using BetterIOT.Common.Bus;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace TestDrive
{
    class Program
    {
        static void Main(string[] args)
        {
            BusClient bus;
            bus = new BusClient();
            while (true)
            {
                List<IOTData> iOTs = new List<IOTData>();
                iOTs.Add(new IOTData()
                    {
                         DataCode = "test",
                         DataName = "test",
                         DataValue = "test",
                         DriveCode = "test",
                         DriveType = "test",
                         GTime = DateTime.Now,
                         ID = Guid.NewGuid(),
                         Sended = false,
                         Unit = ""
                    });
                bus.Publish(BusOption.DATA_OUTPUT, JsonSerializer.Serialize(iOTs));
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
