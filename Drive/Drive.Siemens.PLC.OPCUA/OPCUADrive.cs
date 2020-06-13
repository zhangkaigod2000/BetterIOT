using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using Opc.Ua;
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Drive.Siemens.PLC.OPCUA
{
    public class OPCUADrive : IDrive<OPCUAConfig>
    {
        OpcUaClient m_OpcUaClient = new OpcUaClient();
        public override void DeviceConn(OPCUAConfig config)
        {
            switch(config.ConnType)
            {
                case 0:
                    m_OpcUaClient.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
                    break;
                case 1:
                    m_OpcUaClient.UserIdentity = new UserIdentity(config.UserName, config.Passwd);
                    break;
                case 3:
                    X509Certificate2 certificate = new X509Certificate2(config.OPCUATCPPath, config.CertKey, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                    m_OpcUaClient.UserIdentity = new UserIdentity(certificate);
                    break;
            }
            try
            {
                m_OpcUaClient.ConnectServer(config.OPCUATCPPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connected Failed", ex);
            }
        }

        public override void DeviceDiscnn()
        {
            m_OpcUaClient.Disconnect();
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<IOTData> iOTs = new List<IOTData>();
            foreach(OPCUANode node in DriveConfig.Nodes)
            {
                try
                {
                    IOTData TmpData = new IOTData();
                    DataValue value = m_OpcUaClient.ReadNode(node.NodeID);
                    // 一个数据的类型是不是数组由 value.WrappedValue.TypeInfo.ValueRank 来决定的
                    // -1 说明是一个数值
                    // 1  说明是一维数组，如果类型BuiltInType是Int32，那么实际是int[]
                    // 2  说明是二维数组，如果类型BuiltInType是Int32，那么实际是int[,]
                    if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Int32)
                    {
                        if (value.WrappedValue.TypeInfo.ValueRank == -1)
                        {
                            TmpData.DataValue = ((int)value.WrappedValue.Value).ToString();               // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((int[])value.WrappedValue.Value);           // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((int[,])value.WrappedValue.Value);         // 最终值
                        }
                    }
                    else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.UInt32)
                    {
                        if (value.WrappedValue.TypeInfo.ValueRank == -1)
                        {
                            TmpData.DataValue = ((uint)value.WrappedValue.Value).ToString();               // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((uint[])value.WrappedValue.Value);           // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((uint[,])value.WrappedValue.Value);         // 最终值
                        }
                    }
                    else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.Float)
                    {
                        if (value.WrappedValue.TypeInfo.ValueRank == -1)
                        {
                            TmpData.DataValue = ((float)value.WrappedValue.Value).ToString(node.Format);               // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((float[])value.WrappedValue.Value);           // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((float[,])value.WrappedValue.Value);         // 最终值
                        }
                    }
                    else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.String)
                    {
                        if (value.WrappedValue.TypeInfo.ValueRank == -1)
                        {
                            TmpData.DataValue = (string)value.WrappedValue.Value;               // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((string[])value.WrappedValue.Value);           // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((string[,])value.WrappedValue.Value);         // 最终值
                        }
                    }
                    else if (value.WrappedValue.TypeInfo.BuiltInType == Opc.Ua.BuiltInType.DateTime)
                    {
                        if (value.WrappedValue.TypeInfo.ValueRank == -1)
                        {
                            TmpData.DataValue = ((DateTime)value.WrappedValue.Value).ToString("yyyy-MM-dd HH:mm:ss");               // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 1)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((DateTime[])value.WrappedValue.Value);           // 最终值
                        }
                        else if (value.WrappedValue.TypeInfo.ValueRank == 2)
                        {
                            TmpData.DataValue = System.Text.Json.JsonSerializer.Serialize((DateTime[,])value.WrappedValue.Value);         // 最终值
                        }
                    }
                    TmpData.DataCode = node.Address;
                    TmpData.DataName = node.Name;
                    TmpData.DriveCode = DriveConfig.DriveCode;
                    TmpData.DriveType = DriveConfig.DriveType;
                    TmpData.GTime = DateTime.Now;
                    TmpData.Unit = node.Unit;
                    iOTs.Add(TmpData);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Read Failed", ex);
                }
            }

            return iOTs;
        }
    }
}
