using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CLITester
{
    public class CLITester
    {
        string s_probeAddr = "";
        int s_probePort = 4900;
        uint s_retries = 0;
        private int _delayOnFailms = 0;

        public string ProbeAddr { get => s_probeAddr; set => s_probeAddr = value; }
        public int ProbePort { get => s_probePort; set => s_probePort = value; }

        public uint TestRetryCount { get => s_retries; set => s_retries = value; }

        public int DelayOnFail { get => _delayOnFailms; set => _delayOnFailms = value; }

        TcpClient _client;
        NetworkStream _stream;
        StreamReader _reader;
        StreamWriter _writer;

        public CLITester(string address, int port)
        {
            ProbeAddr = address;
            ProbePort = port;
        }

        void connect()
        {
            _client = new TcpClient();
            _client.Connect(ProbeAddr, ProbePort);
            _stream = _client.GetStream();
            _reader = new StreamReader(_stream);
            _writer = new StreamWriter(_stream);

            _stream.ReadTimeout = 250;
            _writer.AutoFlush = true;
        }

        void disconnect()
        {
            _stream.Close();
            _client.Close();
        }

        public string Test(string command, string expval = "STATUS 00")
        {
            connect();

            //flush
            while (_stream.DataAvailable)
            {
                try
                {
                    _reader.ReadLine();
                }
                catch (System.IO.IOException) { }
            }

            string data = "";
            try
            {
                for (int i = 0; i <= TestRetryCount; i++)
                {
                    _writer.WriteLine(command);

                    // Wait for data
                    for (int w = 0; w < 5; w++)
                    {
                        if (_stream.DataAvailable)
                            break;
                        Thread.Sleep(100);
                    }

                    if (_stream.DataAvailable)
                    {
                        try
                        {
                            while (!_reader.EndOfStream)
                            {
                                // For some reason reading the first line 
                                // sets _stream.DataAvailable to false 
                                // even when there is more data left
                                // Also checking _reader.EndOfStream just throws exception when at end
                                string line = _reader.ReadLine();
                                data += line + "\n";

                                if (line.Contains(expval))
                                    return data;

                            }
                        }
                        catch (System.IO.IOException ex)
                        {
                            string msg = ex.Message;
                        }
                    }

                }

                throw new TestException($"Test \"${command}\" fail.\r\nResponse: ${data}");
            }
            finally
            {
                disconnect();
            }

        }

        public string CmdWaitRegx(string command, string expRegxVal, RegexOptions regxOpt = RegexOptions.Singleline)
        {
            Regex regx = new Regex(expRegxVal);

            connect();

            //flush
            while (_stream.DataAvailable)
            {
                try
                {
                    _reader.ReadLine();
                }
                catch (System.IO.IOException) { }
            }

            string data = "";
            try
            {
                for (int i = 0; i <= TestRetryCount; i++)
                {
                    _writer.WriteLine(command);

                    // Wait for data
                    for (int w = 0; w < 5; w++)
                    {
                        if (_stream.DataAvailable)
                            break;
                        Thread.Sleep(100);
                    }

                    if (_stream.DataAvailable)
                    {
                        try
                        {
                            while (!_reader.EndOfStream)
                            {
                                // For some reason reading the first line 
                                // sets _stream.DataAvailable to false 
                                // even when there is more data left
                                // Also checking _reader.EndOfStream just throws exception when at end
                                string line = _reader.ReadLine();
                                data += line + "\n";

                                if (regx.Match(data).Success)
                                    return data;

                            }
                        }
                        catch (System.IO.IOException ex)
                        {
                            string msg = ex.Message;
                        }
                    }

                }

                throw new TestException($"Test \"${command}\" fail.\r\nResponse: ${data}");
            }
            finally
            {
                disconnect();
            }

        }

    }
}
