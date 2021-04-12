// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

#if OPENILSVER
[assembly: AssemblyTitle("OpenSilver.Controls.Toolkit")]
#elif BRIDGE
[assembly: AssemblyTitle("CSHTML5.Controls.Toolkit")]
#endif
[assembly: AssemblyDescription("Silverlight Toolkit Common Controls.Toolkit")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("© Copyright 2021.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: ComVisible(false)]
[assembly: XmlnsPrefix("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "toolkit")]

#if MIGRATION
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "System.Windows.Controls")]
#else
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation/toolkit", "Windows.UI.Xaml.Controls")]
#endif