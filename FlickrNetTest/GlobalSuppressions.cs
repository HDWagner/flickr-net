// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;
[assembly: SuppressMessage("Assertion", "NUnit2045:Use Assert.Multiple", Scope = "module")]
[assembly: SuppressMessage("Interoperability", "CA1416:Plattformkompatibilität überprüfen", Justification = "Tests are supposed to run under Windows only", Scope = "type", Target = "~T:FlickrNetTest.TestData")]
[assembly: SuppressMessage("Info Code Smell", "S1135:Track uses of \"TODO\" tags",  Scope = "module")]
