namespace Test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Method1();
		}

		private static void Method1()
		{
			Method2();
		}

		private static void Method2()
		{
			new Sample().SampleMethod();
			new Program().Method3();
		}

		private void Method3()
		{
			
		}
	}
}
