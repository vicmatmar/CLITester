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
                if (options.TestCmd != null)
                {
                    _logger.Info("Run Test: " + options.TestCmd);
                    string data = tester.Test(options.TestCmd);
                    _logger.Info(data);
                }
                else if(options.Cmd != null)
                {
                    _logger.Info("Run Cmd: " + options.Cmd);
                    _logger.Info("Exp Val: " + options.ExpVal);
                    string data = tester.CmdWaitRegx(options.Cmd, options.ExpVal);
                    _logger.Info(data);

                }

            }
            catch(TestException ex)
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

    }
}
