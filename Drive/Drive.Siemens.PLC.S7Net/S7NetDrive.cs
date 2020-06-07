using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drive.Siemens.PLC.S7Net
{
    /// <summary>
    /// 西门子S7系列的网络通讯驱动
    /// 包含S1200,S400,S300,S1500,S200,S200Smart 这6个型号
    /// </summary>
    public class S7NetDrive : IDrive<S7NetConfig>
    {

        static int rack = 0;
        static int slot = 0;
        SiemensS7Net siemensTcpNet = null;
        SiemensPLCS siemensPLCSelected = SiemensPLCS.S1200;
        OperateResult connect;
        public override void DeviceConn(S7NetConfig config)
        {
            Connect();
        }

        public int Connect()
        {
            SiemensPLCS siemensPLCS;
            switch (DriveConfig.PLCModel.ToUpper())
            {
                case "S1200":
                    siemensPLCS = SiemensPLCS.S1200;
                    rack = 0;
                    slot = 0;
                    break;
                case "S1500":
                    siemensPLCS = SiemensPLCS.S1500;
                    rack = 0;
                    slot = 0;
                    break;
                case "S200":
                    siemensPLCS = SiemensPLCS.S200;
                    break;
                case "S200SMART":
                    siemensPLCS = SiemensPLCS.S200Smart;
                    break;
                case "S300":
                    siemensPLCS = SiemensPLCS.S300;
                    rack = 0;
                    slot = 2;
                    break;
                case "S400":
                    siemensPLCS = SiemensPLCS.S400;
                    rack = 0;
                    slot = 3;
                    break;
                default:
                    siemensPLCS = SiemensPLCS.S1200;
                    rack = 0;
                    slot = 0;
                    break;
            }
            siemensPLCSelected = siemensPLCS;
            siemensTcpNet = new SiemensS7Net(siemensPLCS);

            if (!System.Net.IPAddress.TryParse(DriveConfig.IP, out System.Net.IPAddress address))
            {
                throw new Exception("IpAddress input wrong");
            }

            siemensTcpNet.IpAddress = DriveConfig.IP;
            siemensTcpNet.Port = DriveConfig.Port;
            try
            {
                if (siemensPLCSelected != SiemensPLCS.S200Smart)
                {
                    siemensTcpNet.Rack = (byte)rack;
                    siemensTcpNet.Slot = (byte)slot;
                }
                OperateResult connect = siemensTcpNet.ConnectServer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public override void DeviceDiscnn()
        {
            siemensTcpNet.ConnectClose();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();
            if (connect.IsSuccess)
            {
                foreach (S7NetResult result in DriveConfig.Results)
                {
                    Byte[] bts = new Byte[result.Len];
                    try
                    {
                        string sResult;
                        switch (result.DataType.ToUpper())
                        {
                            case "BOOL":
                                sResult = siemensTcpNet.ReadBool(result.DB).Content.ToString();
                                break;
                            case "STRING":
                                sResult = siemensTcpNet.ReadString(result.DB).Content;
                                break;
                            case "INT":
                                sResult = siemensTcpNet.ReadInt32(result.DB).Content.ToString();
                                break;
                            case "FLOAT":
                                sResult = siemensTcpNet.ReadFloat(result.DB).Content.ToString(result.Format);
                                break;
                            case "DOUBLE":
                                sResult = siemensTcpNet.ReadDouble(result.DB).Content.ToString(result.Format);
                                break;
                            case "BYTE":
                                sResult = siemensTcpNet.ReadByte(result.DB).Content.ToString();
                                break;
                            case "SHORT":
                                sResult = siemensTcpNet.ReadInt16(result.DB).Content.ToString();
                                break;
                            case "USHORT":
                                sResult = siemensTcpNet.ReadUInt16(result.DB).Content.ToString();
                                break;
                            case "UINT":
                                sResult = siemensTcpNet.ReadUInt32(result.DB).Content.ToString();
                                break;
                            default:
                                sResult = siemensTcpNet.ReadString(result.DB).Content;
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
            }
            return iOTs;
        }
    }
}
