using CommandLine;

namespace CLITester
{

    /// <summary>
    /// Sample1 Test: --host rad-em35x-17 --test "m25p20 test 6 \"c\"" --retry_count 5
    /// Sample2 CmdRegxWait: -h rad-em35x-17 -c "cu cs5480_pinfo" -e "CS5480 Config Reg 2 0x10020A\nCS5480 Config Info Complete"
    /// </summary>
    class Options
    {
        [Option('h', "host", Required = true, HelpText = "Debug adapter address")]
        public string Host { get; set; }

        [Option('p', "port", Required = false, HelpText = "Debug adapter port", DefaultValue = 4900)]
        public int Port { get; set; }

        [Option('t', "test", Required = false, HelpText = "Test command to run")]
        public string TestCmd { get; set; }

        [Option('e', "exp", Required = false, HelpText = "Expected Test result")]
        public string ExpVal { get; set; }

        [Option('r', "retry_count", Required = false, HelpText = "Test retry count", DefaultValue = (uint)0)]
        public uint RetryCount { get; set; }

        [Option('c', "command", Required = false, HelpText = "Command to run")]
        public string Cmd { get; set; }


    }
}
