using CommandLine;
using System;

namespace CLITester
{
    class Program
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static int Main(string[] args)
        {
            _logger.Info("App started");

            var options = new Options();
            var parser = new CommandLine.Parser(s => { s.MutuallyExclusive = false; });

            var isValid = parser.ParseArguments(args, options);
            if (!isValid)
            {
                string msg = CommandLine.Text.HelpText.AutoBuild(options).ToString();
                Console.WriteLine(msg);
                return -1;
            }



            CLITester tester = new CLITester(options.Host, options.Port);
            tester.TestRetryCount = options.RetryCount;

            try
            {
                _logger.Info("Run Test: " + options.TestCmd);
                string data = tester.Test(options.TestCmd);
                _logger.Info(data);

            }catch(TestException ex)
            {
                _logger.Error(ex.Message);
                return -1;
            }
            catch(Exception ex)
            {
                _logger.Error(ex);
                return -2;
            }

            return 0;
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            throw new NotImplementedException();
        }
    }
}
