using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BetterIOT.Common.Base
{
    /// <summary>
    /// 设备驱动列表
    /// </summary>
    public class DriveListConfig
    {
        const string DriveListFileName = "DriveList.json";
        /// <summary>
        /// 驱动品牌
        /// </summary>
        public string DriveBrand { get; set; }
        /// <summary>
        /// 驱动名称
        /// </summary>
        public string DriveName { get; set; }
        /// <summary>
        /// 驱动类型(这个就是驱动的唯一识别号)
        /// </summary>
        public string DriveType { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 驱动目录
        /// </summary>
        public string DrivePath { get; set; }
        /// <summary>
        /// 驱动启动文件
        /// </summary>
        public string DriveStartFile { get; set; }
        /// <summary>
        /// 配置文件类名
        /// </summary>
        public string ConfigClassName { get; set; }

        public static IEnumerable<DriveListConfig> ReadConfig()
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/" + DriveListFileName) == false)
            {
                return null;
            }
            else
            {
                string strTmp = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + DriveListFileName);
                IEnumerable<DriveListConfig> configData = JsonSerializer.Deserialize<IEnumerable<DriveListConfig>>(strTmp);
                return configData;
            }
        }

        public static void WriteConfigData(IEnumerable<DriveListConfig> configData)
        {
            try
            {
                if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/" + DriveListFileName) == true)
                {
                    System.IO.File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/" + DriveListFileName);
                }
                string json = JsonSerializer.Serialize(configData);
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "/" + DriveListFileName, json);
            }
            catch { }
        }
    }
}
