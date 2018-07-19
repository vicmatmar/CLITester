using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLITester
{
    public class TestException : Exception
    {
        public TestException(string msg): base(msg)
        {
        }
    }
}
