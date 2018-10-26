//
// MonoNativeInfo.cs
//
// Author:
//       Martin Baulig <mabaul@microsoft.com>
//
// Copyright (c) 2018 Xamarin Inc. (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Xml;

namespace xharness
{
	public enum MonoNativeFlavor
	{
		Compat, Unified
	}

	public class MonoNativeInfo
	{
		public Harness Harness { get; }
		public MonoNativeFlavor Flavor { get; }

		public MonoNativeInfo (Harness harness, MonoNativeFlavor flavor)
		{
			Harness = harness;
			Flavor = flavor;
		}

		public string FlavorSuffix => Flavor == MonoNativeFlavor.Compat ? "-compat" : "-unified";
		public string ProjectName => "mono-native" + FlavorSuffix;
		public string ProjectPath => Path.Combine (Harness.RootDirectory, "mono-native", ProjectName + ".csproj");
		public string TemplatePath => Path.Combine (Harness.RootDirectory, "mono-native", "mono-native.csproj.template");

		public string GetMinimumOSVersion ()
		{
			if (Harness.Mac)
				throw new NotImplementedException ();
			switch (Flavor) {
			case MonoNativeFlavor.Compat:
				return "9.0";
			case MonoNativeFlavor.Unified:
				return "10.0";
			default:
				throw new Exception (string.Format ("Unknown MonoNativeFlavor: {0}", Flavor));
			}
		}

		public string GetMinimumWatchOSVersion ()
		{
			switch (Flavor) {
			case MonoNativeFlavor.Compat:
				return "2.0";
			case MonoNativeFlavor.Unified:
				return "4.0";
			default:
				throw new Exception (string.Format ("Unknown MonoNativeFlavor: {0}", Flavor));
			}
		}

		public void Convert ()
		{
			var inputProject = new XmlDocument ();

			var xml = File.ReadAllText (TemplatePath);
			inputProject.LoadXmlWithoutNetworkAccess (xml);

			switch (Flavor) {
			case MonoNativeFlavor.Compat:
//				inputProject.SetTargetFrameworkIdentifier ("Xamarin.Mac");
//				inputProject.SetTargetFrameworkVersion ("v2.0");
//				inputProject.RemoveNode ("UseXamMacFullFramework");
//				inputProject.AddAdditionalDefines ("MOBILE;XAMMAC");
//				inputProject.AddReference ("Mono.Security");
				break;
			}
			inputProject.SetOutputPath ("bin\\$(Platform)\\$(Configuration)" + FlavorSuffix);
			inputProject.SetIntermediateOutputPath ("obj\\$(Platform)\\$(Configuration)" + FlavorSuffix);
			inputProject.SetAssemblyName (inputProject.GetAssemblyName () + FlavorSuffix);

			Harness.Save (inputProject, ProjectPath);
		}
	}
}
