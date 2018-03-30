using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeInjection
{
	public class Program
	{
		public static void Main(string[] args)
		{
			TraceInjection injection = new TraceInjection();
			injection.InjectTracingLine(@"..\..\..\Library\Input\Test.exe");
		}
	}
}
