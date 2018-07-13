using CommandLine;

namespace CLITester
{
    class Options
    {
        [Option('h', "host", Required = true, HelpText = "Debug adapter address")]
        public string Host { get; set; }

        [Option('p', "port", Required = false, HelpText = "Debug adapter port", DefaultValue = 4900)]
        public int Port { get; set; }

        [Option('t', "test", Required = false, HelpText = "Test command to run")]
        public string TestCmd { get; set; }

        [Option('e', "exp", Required = false, HelpText = "Expected Test result")]
        public string TestExpVal { get; set; }

        [Option('r', "retry_count", Required = false, HelpText = "Test retry count", DefaultValue = (uint)0)]
        public uint RetryCount { get; set; }

    }
}
