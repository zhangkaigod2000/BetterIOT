using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Siemens.PLC.PPIOverTcp
{
    public class SiemensPPIOverTcpDrive : IDrive<SiemensPPIOverTcpConfig>
    {
        SiemensPPIOverTcp siemensPPI = null;
        public override void DeviceConn(SiemensPPIOverTcpConfig config)
        {
            siemensPPI = new SiemensPPIOverTcp(config.IP, config.Port);
            siemensPPI.Station = config.StationNo;
            OperateResult connect = siemensPPI.ConnectServer();
            if (!connect.IsSuccess)
            {
                throw new Exception("Connect Failed");
            }
        }

        public override void DeviceDiscnn()
        {
            siemensPPI?.ConnectClose();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();

            foreach (SiemensPPIOverTcpResult result in DriveConfig.Results)
            {
                try
                {
                    string sResult;
                    switch (result.DataType.ToUpper())
                    {
                        case "BOOL":
                            sResult = siemensPPI.ReadBool(result.DB).Content.ToString();
                            break;
                        case "STRING":
                            sResult = siemensPPI.ReadString(result.DB, Convert.ToUInt16(result.Len)).Content;
                            break;
                        case "INT":
                            sResult = siemensPPI.ReadInt32(result.DB).Content.ToString();
                            break;
                        case "FLOAT":
                            sResult = siemensPPI.ReadFloat(result.DB).Content.ToString(result.Format);
                            break;
                        case "DOUBLE":
                            sResult = siemensPPI.ReadDouble(result.DB).Content.ToString(result.Format);
                            break;
                        case "BYTE":
                            sResult = siemensPPI.ReadByte(result.DB).Content.ToString();
                            break;
                        case "SHORT":
                            sResult = siemensPPI.ReadInt16(result.DB).Content.ToString();
                            break;
                        case "USHORT":
                            sResult = siemensPPI.ReadUInt16(result.DB).Content.ToString();
                            break;
                        case "UINT":
                            sResult = siemensPPI.ReadUInt32(result.DB).Content.ToString();
                            break;
                        case "LONG":
                            sResult = siemensPPI.ReadInt64(result.DB).Content.ToString();
                            break;
                        case "ULONG":
                            sResult = siemensPPI.ReadUInt64(result.DB).Content.ToString();
                            break;
                        default:
                            sResult = siemensPPI.ReadString(result.DB, 1).Content;
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
