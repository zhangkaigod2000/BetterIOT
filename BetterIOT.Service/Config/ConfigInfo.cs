using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BetterIOT.Service.Config
{
    public class ConfigInfo
    {
        const string CardInfoFileName = "Config.json";
        /// <summary>
        /// 是否启用mqtt
        /// </summary>
        public bool MQTTEnable { get; set; } = false;
        public string ClientId { get; set; }
        public string MQTTerver { get; set; }
        public int MQTTPort { get; set; }
        public string MQTTUsr { get; set; }
        public string MQTTPsw { get; set; }
        public string Topic_IOTDATA { get; set; }

        public static ConfigInfo ReadConfig()
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/" + CardInfoFileName) == false)
            {
                return null;
            }
            else
            {
                string strTmp = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + CardInfoFileName);
                ConfigInfo configData = JsonSerializer.Deserialize<ConfigInfo>(strTmp);
                return configData;
            }
        }

        public static void WriteConfigData(ConfigInfo configData)
        {
            try
            {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/" + CardInfoFileName) == true)
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/" + CardInfoFileName);
                }
                string json = JsonSerializer.Serialize(configData);
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + CardInfoFileName, json);
            }
            catch { }
        }
    }
}
