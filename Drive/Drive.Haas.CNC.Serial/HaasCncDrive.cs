using BetterIOT.Common;
using BetterIOT.Common.Base;
using BetterIOT.Common.DriveConfig;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Drive.Haas.CNC.Serial
{
    public class HaasCncDrive : IDrive<HaasCncConfig>
    {
        SerialPort serialPort = new SerialPort();
        public override void DeviceConn(HaasCncConfig config)
        {
            SetPort();
            serialPort.Open();
        }

        private void SetPort()
        {
            //打开
            serialPort.PortName = DriveConfig.PortName;
            serialPort.BaudRate = DriveConfig.BaudRate;
            switch (DriveConfig.Parity)
            {
                case "NONE":
                    serialPort.Parity = System.IO.Ports.Parity.None;
                    break;
                case "EVEN":
                    serialPort.Parity = System.IO.Ports.Parity.Even;
                    break;
                case "ODD":
                    serialPort.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "MARK":
                    serialPort.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case "SPACE":
                    serialPort.Parity = System.IO.Ports.Parity.Space;
                    break;
            }
            serialPort.DataBits = DriveConfig.DataBits;
            switch (DriveConfig.StopBits)
            {
                case "1":
                    serialPort.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    serialPort.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    break;
                case "2":
                    serialPort.StopBits = System.IO.Ports.StopBits.Two;
                    break;
            }
            serialPort.ReadTimeout = DriveConfig.Timeout;
        }

        public override void DeviceDiscnn()
        {
            serialPort?.Close();
        }

        private List<string> readData(string cmd)
        {
            try
            {
                // Open serial port connection
                if (serialPort.IsOpen == false)
                {
                    serialPort.Open();
                }
                // Write the command and newline to serialPort
                serialPort.Write(cmd + "\r\n");

                Thread.Sleep(1000);
                // Read response
                string response = serialPort.ReadExisting();

                // Split string and return array of response values
                var list = new List<string>();
                string[] values = response.Split(new char[] { ',', ' ' });
                for (var x = 1; x <= values.Length - 1; x++)
                {
                    string val = values[x].Trim();
                    if (!String.IsNullOrEmpty(val)) list.Add(val);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendCommand() :: Exception :: " + ex.Message);
                serialPort?.Open();
            }
            finally
            {
                // Close serialPort connection
                //if (serialPort.IsOpen) serialPort.Close();
            }
            return null;
        }

        public override IEnumerable<IOTData> GetData()
        {
            List<string> response = readData("Q100");
            List<IOTData> Ds = new List<IOTData>();
            string strGroup = Guid.NewGuid().ToString("N");
            Ds.Add(new IOTData
            {
                DataCode = "Avail",
                DataValue = ((response != null && response.Count > 1)? "AVAILABLE" : "UNAVAILABLE"),
                GTime = DateTime.Now,
                DriveType = DriveConfig.DriveType,
                DriveCode = DriveConfig.DriveCode,
                Unit = "-",
                DataName = "Avail"
            });
            response = readData("Q104");
            if (response != null && response.Count > 1)
            {
                var tmpdata = new IOTData
                {
                    DataCode = "Mode",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "模式"
                };
                switch (response[1])
                {
                    case "(MDI)": tmpdata.DataValue = "MANUAL_DATA_INPUT"; break;
                    case "(JOG)": tmpdata.DataValue = "MANUAL"; break;
                    case "(ZERO RET)": tmpdata.DataValue = "MANUAL"; break;
                    default: tmpdata.DataValue = "AUTOMATIC"; break;
                }
                Ds.Add(tmpdata);
                var tmpZeroRet = new IOTData
                {
                    DataCode = "ZeroRet",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "ZeroRet"
                };
                switch (response[1])
                {
                    case "(ZERO RET)": tmpZeroRet.DataValue =  "NO ZERO X"; break;
                    default: tmpZeroRet.DataValue = "NORMAL"; break;
                }
                Ds.Add(tmpZeroRet);
                response = readData("Q500");
                if (response != null && response.Count > 1)
                {
                    if (response[0] == "PROGRAM")
                    {
                        if (response[1] != "MDI")
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Program",
                                DataValue = response[1],
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "程序"
                            });
                        }

                        if (response[2] == "IDLE")
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Execution",
                                DataValue = "READY",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "运行状态"
                            });
                        }
                        else if (response[2] == "FEED HOLD")
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Execution",
                                DataValue = "INTERRUPTED",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "运行状态"
                            });
                        }

                        if (response[2] == "ALARM ON")
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Execution",
                                DataValue = "STOPPED",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "运行状态"
                            });
                            Ds.Add(new IOTData
                            {
                                DataCode = "System",
                                DataValue = "Alarm on indicator",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "系统"
                            });
                        }
                        else
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Estop",
                                DataValue = "ARMED",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "停止"
                            });
                            Ds.Add(new IOTData
                            {
                                DataCode = "System",
                                DataValue = "NORMAL",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "系统"
                            });
                        }

                    }
                    else if (response[0] == "STATUS")
                    {
                        if (response[1] == "BUSY")
                        {
                            Ds.Add(new IOTData
                            {
                                DataCode = "Execution",
                                DataValue = "ACTIVE",
                                GTime = DateTime.Now,
                                DriveType = DriveConfig.DriveType,
                                DriveCode = DriveConfig.DriveCode,
                                Unit = "-",
                                DataName = "运行状态"
                            });
                        }
                    }
                }
                    // Check to make sure the correct variable is returned
                Ds.Add(new IOTData
                {
                    DataCode = "Xact",
                    DataValue = GetVariable(5041)??"",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "X坐标"
                });
                Ds.Add(new IOTData
                {
                    DataCode = "Yact",
                    DataValue = GetVariable(5042) ?? "",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "Y坐标"
                }); 
                Ds.Add(new IOTData
                {
                    DataCode = "Zact",
                    DataValue = GetVariable(5043) ?? "",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "Z坐标"
                });
                Ds.Add(new IOTData
                {
                    DataCode = "SpindleSpeed",
                    DataValue = GetVariable(3027) ?? "",
                    GTime = DateTime.Now,
                    DriveType = DriveConfig.DriveType,
                    DriveCode = DriveConfig.DriveCode,
                    Unit = "-",
                    DataName = "转速"
                });
                return Ds;
            }
        }

        private string GetVariable(int variable)
        {
            List<string> response = readData("Q600 " + variable);
            if (response != null && response.Count > 2)
            {
                // Check to make sure the correct variable is returned
                if (response[1] == variable.ToString())
                {
                    return response[2];
                }
            }

            return null;
        }
    }
}
