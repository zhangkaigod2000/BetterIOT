using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BetterIOT.Common.Base
{
    public class ConfigBase
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string DriveCode { get; set; }
        /// <summary>
        /// 驱动类型
        /// </summary>
        public string DriveType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string ReMark { get; set; }
        /// <summary>
        /// 循环周期
        /// </summary>
        public int CycleTime { get; set; }

        public static T ReadConfig<T>(string FilePath) where T : ConfigBase
        {
            if (System.IO.File.Exists(FilePath) == false)
            {
                return null;
            }
            else
            {
                string strTmp = System.IO.File.ReadAllText(FilePath);
                T configData = JsonSerializer.Deserialize<T>(strTmp);
                return configData;
            }
        }

        public static void WriteConfigData<T>(T configData, string FilePath) where T : ConfigBase
        {
            try
            {
                if (System.IO.File.Exists(FilePath) == true)
                {
                    System.IO.File.Delete(FilePath);
                }
                string json = JsonSerializer.Serialize(configData);
                System.IO.File.WriteAllText(FilePath, json);
            }
            catch { }
        }
    }
}
