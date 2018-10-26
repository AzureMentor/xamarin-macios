using System;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Mono;

namespace Mono.Native.Tests
{
	[TestFixture]
	public class NativePlatformTest
	{
		[Test]
		public void Test ()
		{
			var type = MonoNativePlatform.GetPlatformType ();
			Assert.That ((int)type, Is.GreaterThan (0), "platform type");

			Console.Error.WriteLine ($"NATIVE PLATFORM TYPE: {type}");

			// var usingCompat = (type & MonoNativePlatformType.MONO_NATIVE_PLATFORM_TYPE_COMPAT) != 0;
			// Assert.AreEqual (MonoNativeConfig.UsingCompat, usingCompat, "using compatibility layer");
		}

		[Test]
		public void TestInitialize ()
		{
			MonoNativePlatform.Initialize ();
		}

		[Test]
		public void MartinTest ()
		{
			MonoNativePlatform.Initialize ();

			var asm = typeof (string).Assembly;
			var type = asm.GetType ("Mono.MonoNativePlatform");
			Console.Error.WriteLine ($"TEST: {type}");

			var method = type.GetMethod ("Initialize", BindingFlags.Static | BindingFlags.Public);
			method.Invoke (null, null);
			Console.Error.WriteLine ($"CALLED INITIALIZE!");

			var method2 = type.GetMethod ("Test", BindingFlags.Static | BindingFlags.Public);
			method2.Invoke (null, null);

			Console.Error.WriteLine ($"CALLED TEST!");
		}
	}
}
