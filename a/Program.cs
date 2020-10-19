using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.CompilerServices;

namespace a
{
    class Program
    {
	    static void Main(string[] args)
	    {
		    var assm = LoadAssm();
		    GC.Collect ();
		    CreateGarbage ();
		    UseAssm (assm);
	    }

	    [MethodImpl (MethodImplOptions.NoInlining)]
	    public static Assembly LoadAssm ()
	    {
		    var s = Path.Combine (Path.GetDirectoryName (typeof(Program).Assembly.Location), "b.dll");
		    using var stream = File.OpenRead (s);
		    var assm = AssemblyLoadContext.Default.LoadFromStream (stream);
		    return assm;
	    }

	    [MethodImpl (MethodImplOptions.NoInlining)]
	    public static void CreateGarbage ()
	    {
		    var bytes = new byte[1024 * 1024 * 1000];
		    for (int i = 0; i < 1024 * 1024; ++i) {
			    bytes[i] = (byte)'u';
		    }
	    }


	    [MethodImpl (MethodImplOptions.NoInlining)]
	    public static void UseAssm (Assembly assm)
	    {
		    var an = assm.GetName ();
		    Console.WriteLine ($"Name=\"{an.Name}\", Culture=\"{an.CultureName}\"");
		    foreach (var type in assm.GetTypes ()) {
			    Console.WriteLine ($"- {type.FullName}");
		    }
	    }
    }
}
