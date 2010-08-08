using System.Linq;
using NUnit.Framework;
using SharpDiff.FileStructure;

namespace SharpDiff.Tests
{
    [TestFixture]
    public class RealWorldDiffTests
    {
        const string fileOneContent = @"using System;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Diagnostics
{
    public class when_registering_types_with_diagnostics_enabled
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Diagnostics
                .Enable()
                .RegisterListener(new StubListener(x => results = x));
        };

        Because of = () =>
        {
            model.AddMappingsFromSource(new StubTypeSource(typeof(First), typeof(FirstMap), typeof(SecondMap), typeof(ThirdMap), typeof(CompMap)));
            model.BuildMappings();
        };

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_register_each_ClassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(FirstMap), typeof(SecondMap));

        It should_register_each_SubclassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(ThirdMap));

        It should_register_each_ComponentMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(CompMap));

        It should_not_register_non_fluent_mapping_types = () =>
            results.FluentMappings.ShouldNotContain(typeof(First));
        
        static FluentNHibernate.PersistenceModel model;
        static DiagnosticResults results;
    }
}";
        const string fileTwoContent = @"using System;
using FluentNHibernate.Diagnostics;
using FluentNHibernate.Mapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Diagnostics
{
    public class when_registering_types_with_diagnostics_enabled
    {
        Establish context = () =>
        {
            var despatcher = new DefaultDiagnosticMessageDespatcher();
            despatcher.RegisterListener(new StubListener(x => results = x));

            model = new FluentNHibernate.PersistenceModel();
            model.SetLogger(new DefaultDiagnosticLogger(despatcher));
        };

        Because of = () =>
        {
            model.AddMappingsFromSource(new StubTypeSource(typeof(First), typeof(FirstMap), typeof(SecondMap), typeof(ThirdMap), typeof(CompMap)));
            model.BuildMappings();
        };

        It should_produce_results_when_enabled = () =>
            results.ShouldNotBeNull();

        It should_register_each_ClassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(FirstMap), typeof(SecondMap));

        It should_register_each_SubclassMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(ThirdMap));

        It should_register_each_ComponentMap_type_and_return_them_in_the_results = () =>
            results.FluentMappings.ShouldContain(typeof(CompMap));

        It should_not_register_non_fluent_mapping_types = () =>
            results.FluentMappings.ShouldNotContain(typeof(First));
        
        static FluentNHibernate.PersistenceModel model;
        static DiagnosticResults results;
    }
}";

        [Test]
        public void should_have_one_chunk()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);

            Assert.AreEqual(1, diff.Chunks.Count);
        }

        [Test]
        public void chunk_should_have_correct_new_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(10, chunk.NewRange.StartLine);
            Assert.AreEqual(11, chunk.NewRange.LinesAffected);
        }

        [Test]
        public void chunk_should_have_correct_old_range()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(10, chunk.OriginalRange.StartLine);
            Assert.AreEqual(10, chunk.OriginalRange.LinesAffected);
        }

        [Test]
        public void chunk_should_have_correct_number_of_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();

            Assert.AreEqual(5, chunk.Snippets.Count());
        }

        [Test]
        public void chunk_should_have_correct_snippets()
        {
            var diff = Differ.Compare("one", fileOneContent, "two", fileTwoContent);
            var chunk = diff.Chunks.First();
            var snippets = chunk.Snippets;

            var firstContext = snippets.ElementAtOrDefault(0) as ContextSnippet;

            Assert.IsNotNull(firstContext);
            Assert.AreEqual(3, firstContext.OriginalLines.Count());
            Assert.AreEqual("    {", firstContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("        Establish context = () =>", firstContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("        {", firstContext.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, firstContext.ModifiedLines.Count());

            var firstAddition = snippets.ElementAtOrDefault(1) as AdditionSnippet;

            Assert.IsNotNull(firstAddition);
            Assert.AreEqual(0, firstAddition.OriginalLines.Count());
            Assert.AreEqual(3, firstAddition.ModifiedLines.Count());
            Assert.AreEqual("            var despatcher = new DefaultDiagnosticMessageDespatcher();", firstAddition.ModifiedLines.ElementAt(0).Value);
            Assert.AreEqual("            despatcher.RegisterListener(new StubListener(x => results = x));", firstAddition.ModifiedLines.ElementAt(1).Value);
            Assert.AreEqual("", firstAddition.ModifiedLines.ElementAt(2).Value);

            var secondContext = snippets.ElementAtOrDefault(2) as ContextSnippet;

            Assert.IsNotNull(secondContext);
            Assert.AreEqual(1, secondContext.OriginalLines.Count());
            Assert.AreEqual("            model = new FluentNHibernate.PersistenceModel();", secondContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual(0, secondContext.ModifiedLines.Count());

            var modification = snippets.ElementAtOrDefault(3) as ModificationSnippet;

            Assert.IsNotNull(modification);
            Assert.AreEqual(3, modification.OriginalLines.Count());
            Assert.AreEqual("            model.Diagnostics", modification.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("                .Enable()", modification.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("                .RegisterListener(new StubListener(x => results = x));", modification.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(1, modification.ModifiedLines.Count());
            Assert.AreEqual("            model.SetLogger(new DefaultDiagnosticLogger(despatcher));", modification.ModifiedLines.ElementAt(0).Value);

            var thirdContext = snippets.ElementAtOrDefault(4) as ContextSnippet;

            Assert.IsNotNull(thirdContext);
            Assert.AreEqual(3, thirdContext.OriginalLines.Count());
            Assert.AreEqual("        };", thirdContext.OriginalLines.ElementAt(0).Value);
            Assert.AreEqual("", thirdContext.OriginalLines.ElementAt(1).Value);
            Assert.AreEqual("        Because of = () =>", thirdContext.OriginalLines.ElementAt(2).Value);
            Assert.AreEqual(0, thirdContext.ModifiedLines.Count());
        }
    }
}