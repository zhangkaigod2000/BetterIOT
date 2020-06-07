using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Melsec;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Mitsubishi.PLC.A1EAsciiNet
{
    public class MelsecA1EAsciiNetDrive : IDrive<MelsecA1EAsciiNetConfig>
    {
        MelsecA1EAsciiNet melsec_net = null;
        public override void DeviceConn(MelsecA1EAsciiNetConfig config)
        {
            melsec_net = new MelsecA1EAsciiNet();
            melsec_net.IpAddress = config.IP;
            melsec_net.Port = config.Port;
            if (!System.Net.IPAddress.TryParse(config.IP, out System.Net.IPAddress address))
            {
                throw new Exception("IpAddress input wrong");
            }

            melsec_net.ConnectClose();
            OperateResult connect = melsec_net.ConnectServer();
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            melsec_net.ConnectClose();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();

            foreach (MelsecA1EAsciiNetResult result in DriveConfig.Results)
            {
                try
                {
                    string sResult;
                    switch (result.DataType.ToUpper())
                    {
                        case "BOOL":
                            sResult = melsec_net.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = melsec_net.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = melsec_net.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = melsec_net.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = melsec_net.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "SHORT":
                            sResult = melsec_net.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = melsec_net.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = melsec_net.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = melsec_net.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = melsec_net.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = melsec_net.ReadString(result.DB, 1).Content;
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
