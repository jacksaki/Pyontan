using Livet;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis;
using Microsoft.Xaml.Behaviors.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyontan.Models
{
    public class CSharpExecutor : NotificationObject
    {
        public CSharpExecutor()
        {
            this.Logs.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(Logs));
            };
        }
        public async Task ExecuteAsync(DbProject project, ProjectSettings settings)
        {
            this.Logs.Clear();

            var scriptOptions = ScriptOptions.Default.
                AddReferences(settings.SelectedGlobalAssemblies.Select(x => x.Assembly)).
                AddReferences(settings.AdditionalAssemblies.Select(x => x.Assembly)).
                AddImports(settings.ImportList);
            var sb = new System.Text.StringBuilder();
            if (!string.IsNullOrWhiteSpace(settings.DbContextSource))
            {
                sb.AppendLine(settings.DbContextSource);
            }

            var script = CSharpScript.Create(sb.ToString(), scriptOptions);
            script = script.ContinueWith(project.Source);
            var compilation = script.GetCompilation();
            var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error);
            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    Logs.Add(new LogItem($"{error.Location} {error.GetMessage()}", LogType.Error));
                }
                return;
            }


            using (var rd = new OutputRedirector())
            {
                rd.StandardOutputWritten += (sender, e) =>
                {
                    Logs.Add(new LogItem(e.Text, LogType.Info));
                };
                rd.StandardErrorWritten += (sender, e) =>
                {
                    Logs.Add(new LogItem(e.Text, LogType.Error));
                };
                try
                {
                    var state = await script.RunAsync();
                }
                catch (CompilationErrorException ce)
                {
                    foreach (var msg in ce.Diagnostics)
                    {
                        Logs.Add(new LogItem($"{msg.Location} {msg.GetMessage()}", LogType.Error));
                    }
                }
                catch (Exception ex)
                {
                    Logs.Add(new LogItem(ex.Message, LogType.Error));
                    Logs.Add(new LogItem(ex.StackTrace, LogType.Error));
                }
            }
        }
        public DispatcherCollection<LogItem> Logs
        {
            get;
        } = new DispatcherCollection<LogItem>(DispatcherHelper.UIDispatcher);
    }
}
