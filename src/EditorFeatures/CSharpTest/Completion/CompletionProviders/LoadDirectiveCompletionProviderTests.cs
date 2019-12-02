﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Editor.CSharp.Completion.FileSystem;
using Microsoft.CodeAnalysis.Editor.UnitTests.Workspaces;
using Microsoft.CodeAnalysis.Test.Utilities;
using Microsoft.VisualStudio.Language.Intellisense.AsyncCompletion.Data;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Completion.CompletionProviders
{
    [Trait(Traits.Feature, Traits.Features.Completion)]
    public class LoadDirectiveCompletionProviderTests : AbstractCSharpCompletionProviderTests
    {
        public LoadDirectiveCompletionProviderTests(CSharpTestWorkspaceFixture workspaceFixture) : base(workspaceFixture)
        {
        }

        internal override Type GetCompletionProviderType()
        {
            return typeof(LoadDirectiveCompletionProvider);
        }

        protected override IEqualityComparer<string> GetStringComparer()
        {
            return StringComparer.OrdinalIgnoreCase;
        }

        private protected override Task VerifyWorkerAsync(
            string code, int position, string expectedItemOrNull, string expectedDescriptionOrNull,
            SourceCodeKind sourceCodeKind, bool usePreviousCharAsTrigger, bool checkForAbsence,
            int? glyph, int? matchPriority, bool? hasSuggestionItem, string displayTextSuffix,
            string inlineDescription = null, List<CompletionFilter> matchingFilters = null)
        {
            return BaseVerifyWorkerAsync(
                code, position, expectedItemOrNull, expectedDescriptionOrNull,
                sourceCodeKind, usePreviousCharAsTrigger, checkForAbsence,
                glyph, matchPriority, hasSuggestionItem, displayTextSuffix,
                inlineDescription, matchingFilters);
        }

        [Fact]
        public async Task IsCommitCharacterTest()
        {
            var commitCharacters = new[] { '"', '\\' };
            await VerifyCommitCharactersAsync("#load \"$$", textTypedSoFar: "", validChars: commitCharacters, sourceCodeKind: SourceCodeKind.Script);
        }

        [Theory]
        [InlineData("#load \"$$/")]
        [InlineData("#load \"$$\\")]
        [InlineData("#load \"$$,")]
        [InlineData("#load \"$$A")]
        [InlineData("#load \"$$!")]
        [InlineData("#load \"$$(")]
        public void IsTextualTriggerCharacterTest(string markup)
        {
            VerifyTextualTriggerCharacter(markup, shouldTriggerWithTriggerOnLettersEnabled: true, shouldTriggerWithTriggerOnLettersDisabled: true, SourceCodeKind.Script);
        }
    }
}
