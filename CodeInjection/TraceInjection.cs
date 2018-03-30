using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;

// Altered https://www.codeproject.com/Articles/445858/Tracing-with-Code-Injection

namespace CodeInjection
{
	public class TraceInjection
	{
		private bool InjectTracingLine(string assemblyPath, string outputDirectory)
		{
			// New assembly path
			// moved to new overload
			//string outputDirectory = ConfigurationManager.AppSettings["OutputDirectory"].ToString();
			var fileName = Path.GetFileName(assemblyPath);

			var newPath = outputDirectory + @"\" + fileName;

			// Check if Output directory already exists, if not, create one
			if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);

			// Load assembly and symbols
			var asmDef = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters
			{
				ReadSymbols = true
			});

			foreach (var modDef in asmDef.Modules)
			{
				foreach (var typDef in modDef.Types)
				{
					foreach (var metDef in typDef.Methods)
					{
						// Method has the desired attribute set, edit IL for method
						Trace.WriteLine("Found method " + metDef);

						// Get ILProcessor
						var ilProcessor = metDef.Body.GetILProcessor();

						// Load fully qualified method name as string (we will log this)
						Instruction i1 = ilProcessor.Create(OpCodes.Ldstr, metDef.ToString());
						ilProcessor.InsertBefore(metDef.Body.Instructions[0], i1);

						// Call the method which would write the above info
						Instruction i2 = ilProcessor.Create(
							OpCodes.Call,
							metDef.Module.ImportReference(typeof(Console).GetMethod("WriteLine", new[] {typeof(string)})));

						ilProcessor.InsertAfter(i1, i2);
					}
				}
			}

			// Save modified assembly
			asmDef.Write(newPath, new WriterParameters
			{
				WriteSymbols = true
			});
			return true;
		}

		public bool InjectTracingLine(string assemblyPath)
		{
			// New assembly path
			string outputDirectory = ConfigurationManager.AppSettings["OutputDirectory"];
			return InjectTracingLine(assemblyPath, outputDirectory);
		}
	}
}