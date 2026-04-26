using GoogleLibrary.Custom.Events;
using Gradient.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace GoogleLibrary.Test.Custom.Events
{
    /// <summary>
    /// Keeps docs/spreadsheet-schema.md in sync with EnumEventFieldType. The test verifies that the
    /// auto-generated regions (delimited by &lt;!-- BEGIN AUTO: name --&gt; / &lt;!-- END AUTO: name --&gt;)
    /// match what reflection currently produces; on drift it fails with a hint.
    ///
    /// Run with UPDATE_SCHEMA_DOCS=1 to write the regenerated content back to the file:
    ///     UPDATE_SCHEMA_DOCS=1 dotnet test --filter SchemaDocsAreUpToDate
    /// </summary>
    [TestClass]
    public class TestSchemaDocsGeneration
    {
        private const string DocRelativePath = "docs/spreadsheet-schema.md";
        private const string FieldsBegin = "<!-- BEGIN AUTO: fields -->";
        private const string FieldsEnd = "<!-- END AUTO: fields -->";
        private const string UpdateEnvVar = "UPDATE_SCHEMA_DOCS";

        [TestMethod]
        public void SchemaDocsAreUpToDate()
        {
            var path = Path.Combine(FindRepoRoot(), DocRelativePath);
            Assert.IsTrue(File.Exists(path), $"Expected schema docs at {path}.");

            var existing = File.ReadAllText(path);
            var generated = ReplaceMarkerRegion(existing, FieldsBegin, FieldsEnd, GenerateFieldsTable());

            if (Environment.GetEnvironmentVariable(UpdateEnvVar) == "1")
            {
                if (existing != generated)
                    File.WriteAllText(path, generated);
                return;
            }

            Assert.AreEqual(
                generated,
                existing,
                $"docs/spreadsheet-schema.md is stale. Re-run with {UpdateEnvVar}=1 to refresh.");
        }

        private static string GenerateFieldsTable()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine("| Field | Aliases |");
            sb.AppendLine("|---|---|");
            foreach (EnumEventFieldType value in Enum.GetValues<EnumEventFieldType>())
            {
                var aliases = value.Aliases().ToList();
                var aliasCol = aliases.Count > 0
                    ? string.Join(", ", aliases.Select(a => $"`{a}`"))
                    : "_(none)_";
                sb.AppendLine($"| `{value}` | {aliasCol} |");
            }
            return sb.ToString();
        }

        private static string ReplaceMarkerRegion(string text, string beginMarker, string endMarker, string newRegion)
        {
            var beginIdx = text.IndexOf(beginMarker, StringComparison.Ordinal);
            var endIdx = text.IndexOf(endMarker, StringComparison.Ordinal);
            if (beginIdx < 0 || endIdx < 0 || endIdx < beginIdx)
                throw new InvalidOperationException($"Markers '{beginMarker}' / '{endMarker}' not found in expected order in the doc.");
            var afterBegin = beginIdx + beginMarker.Length;
            return text.Substring(0, afterBegin) + newRegion + text.Substring(endIdx);
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Google.sln")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new InvalidOperationException("Could not find Google.sln walking up from test directory.");
        }
    }
}
