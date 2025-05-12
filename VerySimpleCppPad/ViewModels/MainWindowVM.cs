using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VerySimpleCppPad.Services;

namespace VerySimpleCppPad.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly ICppCompileAndRunService cppCompileAndRunService;
        private readonly ISettingService settingService;
        private readonly IMessageBoxService messageBoxService;
        private readonly IProgramFileManageService programFileManageService;
        [ObservableProperty]
        private string compilerPath;

        [ObservableProperty]
        private string editorPath;

        [ObservableProperty]
        private ObservableCollection<string> programFiles;

        [ObservableProperty]
        private string selectedProgramFile;

        [ObservableProperty]
        private string? currentProgramFile = null;


        [ObservableProperty]
        private string font = "Consolas";

        [ObservableProperty]
        private string outputContent;

        [ObservableProperty]
        private string inputContent;

        partial void OnCompilerPathChanged(string value) { this.settingService.Setting.CompilerPath = value; this.settingService.Save(); }
        partial void OnEditorPathChanged(string value) { this.settingService.Setting.EditorPath = value; this.settingService.Save(); }
        partial void OnFontChanged(string value) { this.settingService.Setting.Font = value; this.settingService.Save(); }

        public MainWindowVM(ICppCompileAndRunService cppCompileAndRunService, ISettingService settingService, IMessageBoxService messageBoxService, IProgramFileManageService programFileManageService)
        {
            this.cppCompileAndRunService = cppCompileAndRunService;
            this.settingService = settingService;
            this.messageBoxService = messageBoxService;
            this.programFileManageService = programFileManageService;
            ProgramFiles = new(programFileManageService.GetAllPrograms().Select(x => x.Key));
            this.CompilerPath = settingService.Setting.CompilerPath;
            this.EditorPath = settingService.Setting.EditorPath;
            this.Font = settingService.Setting.Font;
        }

        [RelayCommand]
        void OpenCurrentFile()
        {
            CurrentProgramFile = SelectedProgramFile;
        }

        [RelayCommand]
        void CreateNewFile()
        {
            string? path = messageBoxService.ShowInputBox("Please enter a file name:", "Input");
            if (path is null) return;
            // 确认 result 是否是合法的文件路径
            if (string.IsNullOrEmpty(path) || path.ToCharArray().Intersect(Path.GetInvalidFileNameChars()).Any())
            {
                messageBoxService.Show("The file name is invalid.", "Message");
                return;
            }

            path = programFileManageService.CreateNewProgram($"{path}.cpp", programFileManageService.GetTemplate());

            ProgramFiles = new(programFileManageService.GetAllPrograms().Select(x => x.Key));

            CurrentProgramFile = path;
        }

        [RelayCommand]
        void OpenInEditor()
        {
            if (!File.Exists(CurrentProgramFile))
            {
                messageBoxService.Show("You should open a file first.", "Message");
                return;
            }
            RunProcess(string.Format(settingService.Setting.EditorPath, CurrentProgramFile));
        }

        [RelayCommand]
        void OpenTemplate()
        {
            RunProcess(string.Format(settingService.Setting.EditorPath, programFileManageService.GetTemplatePath()));
        }

        [RelayCommand]
        void StopProgram()
        {
            cppCompileAndRunService.StopRunningProcess();
        }

        [RelayCommand]
        async Task RunProgramAsync()
        {
            if (CurrentProgramFile is null)
            {
                messageBoxService.Show("You should open a file first.", "Message");
                return;
            }
            OutputContent = "Running...";

            var result = await cppCompileAndRunService.CompileAndRunAsync(CurrentProgramFile, InputContent, $"{CurrentProgramFile}.exe", settingService.Setting.CompilerPath);
            if (result.Result != Models.CompileAndRunResult.CompileResultType.Success)
            {
                OutputContent = result.Message;
                return;
            }

            OutputContent = result.Message;
        }

        private void RunProcess(string path)
        {
            int firstSpaceIndex = path.IndexOf(' ');
            if (firstSpaceIndex == -1)
            {
                System.Diagnostics.Process.Start(path);
                return;
            }
            string left = path.Substring(0, firstSpaceIndex);
            string right = path.Substring(firstSpaceIndex + 1);
            System.Diagnostics.Process.Start(left, right);
        }

    }
}
