using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Siemens.PLC.FetchWrite
{
    public class FetchWriteDrive : IDrive<SiemensFetchWriteConfig>
    {
        SiemensFetchWriteNet siemensFWNet = null;
        
        public override void DeviceConn(SiemensFetchWriteConfig config)
        {
            siemensFWNet = new SiemensFetchWriteNet();
            if (!System.Net.IPAddress.TryParse(config.IP, out System.Net.IPAddress address))
            {
                throw new Exception("IpAddress input wrong");
            }
            siemensFWNet.IpAddress = config.IP;
            siemensFWNet.Port = config.Port;
            try
            {
                OperateResult connect = siemensFWNet.ConnectServer();
                if (!connect.IsSuccess)
                {
                    throw new Exception("Connect Failed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void DeviceDiscnn()
        {
            siemensFWNet.ConnectClose();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();

            foreach (SiemensFetchWriteResult result in DriveConfig.Results)
            {
                try
                {
                    string sResult;
                    switch (result.DataType.ToUpper())
                    {
                        case "BOOL":
                            sResult = siemensFWNet.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = siemensFWNet.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = siemensFWNet.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = siemensFWNet.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = siemensFWNet.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "BYTE":
                            sResult = siemensFWNet.ReadByte(result.DB).Content.ToString();
                            break;
                        case "SHORT":
                            sResult = siemensFWNet.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = siemensFWNet.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = siemensFWNet.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = siemensFWNet.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = siemensFWNet.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = siemensFWNet.ReadString(result.DB, 1).Content;
                            break;
                    }
                    iOTs.Add(new IOTData
                    {
                        DataCode = result.Address,
                        DataValue = sResult,
                        DataName = result.Name,
                        DriveCode = DriveConfig.DriveCode,
                        DriveType = DriveConfig.DriveType,
                        GTime = DateTime.Now,
                        Unit = result.Unit
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return iOTs;
        }
    }
}
