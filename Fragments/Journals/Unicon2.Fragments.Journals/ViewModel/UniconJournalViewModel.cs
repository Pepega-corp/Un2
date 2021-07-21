using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Unicon2.Fragments.Journals.Infrastructure.Export;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.Helpers;
using Unicon2.Fragments.Journals.MemoryAccess;
using Unicon2.Fragments.Journals.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.ApplicationSettingsService;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.SharedResources.Behaviors;
using Unicon2.SharedResources.Icons;
using Unicon2.Unity.Commands;
using Unicon2.Unity.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Journals.ViewModel
{
	public class UniconJournalViewModel : ViewModelBase, IUniconJournalViewModel, IFragmentViewModel,
		IFragmentOpenedListener, IFragmentFileExtension
	{
		private readonly ILocalizerService _localizerService;
		private readonly IApplicationGlobalCommands _applicationGlobalCommands;
		private readonly ITypesContainer _typesContainer;
		private readonly ILogService _logService;
		private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IJournalLoaderProvider _journalLoaderProvider;
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private List<string> _journalParametersNameList;
		private IUniconJournal _uniconJournal;
		private DynamicDataTable _table;
		private bool _canExecuteJournalLoading;

        public UniconJournalViewModel(ILocalizerService localizerService,
            IFragmentOptionsViewModel fragmentOptionsViewModel,
            Func<IFragmentOptionGroupViewModel> fragmentOptionGroupViewModelgetFunc,
            Func<IFragmentOptionCommandViewModel> fragmentOptionCommandViewModelgetFunc,
            IApplicationGlobalCommands applicationGlobalCommands, ITypesContainer typesContainer, ILogService logService
            , IApplicationSettingsService applicationSettingsService, IJournalLoaderProvider journalLoaderProvider,
            IValueViewModelFactory valueViewModelFactory)

        {
            _localizerService = localizerService;
            _applicationGlobalCommands = applicationGlobalCommands;
            _typesContainer = typesContainer;
            _logService = logService;
            _applicationSettingsService = applicationSettingsService;
            _journalLoaderProvider = journalLoaderProvider;
            _valueViewModelFactory = valueViewModelFactory;
            IFragmentOptionGroupViewModel fragmentOptionGroupViewModel = fragmentOptionGroupViewModelgetFunc();
            fragmentOptionGroupViewModel.NameKey = "Device";
            IFragmentOptionCommandViewModel fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Load";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconInboxIn;
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);
            LoadCommand = new RelayCommand(OnLoadJournal, CanLoadExecute);
            fragmentOptionCommandViewModel.OptionCommand = LoadCommand;
            fragmentOptionsViewModel.FragmentOptionGroupViewModels.Add(fragmentOptionGroupViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Open";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscUpload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteLoadJournal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = "Save";
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconDiscDownload;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteSaveJournal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            fragmentOptionCommandViewModel = fragmentOptionCommandViewModelgetFunc();
            fragmentOptionCommandViewModel.TitleKey = ApplicationGlobalNames.UiCommandStrings.SAVE_FOR_PRINT;
            fragmentOptionCommandViewModel.IconKey = IconResourceKeys.IconPrintText;
            fragmentOptionCommandViewModel.OptionCommand = new RelayCommand(OnExecuteExportJournal);
            fragmentOptionGroupViewModel.FragmentOptionCommandViewModels.Add(fragmentOptionCommandViewModel);

            _loaderHooks = new LoaderHooks(
                () => { Table.Values.Clear(); }, list =>
                {
                    Table.AddFormattedValueViewModel(list.Select(formattedValue =>
                        _valueViewModelFactory.CreateFormattedValueViewModel(formattedValue)).ToList());
                });
            FragmentOptionsViewModel = fragmentOptionsViewModel;
            CanExecuteJournalLoading = true;
        }

        private async void OnExecuteExportJournal()
		{
			var nameForUiLocalized = NameForUiKey;
			_localizerService.TryGetLocalizedString(NameForUiKey, out nameForUiLocalized);
			var sfd = new SaveFileDialog
			{
				Filter = " HTML файл (*html)|*html" + "|Все файлы (*.*)|*.* ",
				DefaultExt = ".html",
				FileName = $"{nameForUiLocalized} {DeviceContext.DeviceName}"
			};
			if (sfd.ShowDialog() == true)
			{
				try
				{
					File.WriteAllText(sfd.FileName,
						await _typesContainer
							.Resolve<IHtmlRenderer<IUniconJournalViewModel, JournalExportSelector>>()
							.RenderHtmlString(this, new JournalExportSelector()));
					_logService.LogMessage(ApplicationGlobalNames.StatusMessages.FILE_EXPORT_SUCCESSFUL);

				}
				catch (Exception e)
				{
					_logService.LogMessage(e.Message + Environment.NewLine + e.StackTrace, LogMessageTypeEnum.Error);
				}

			}
		}

		private void OnExecuteSaveJournal()
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = " UJR файл (*.ujr)|*.ujr" + "|Все файлы (*.*)|*.* ";
			sfd.DefaultExt = ".ujr";
			sfd.FileName = NameForUiKey + " " + DeviceContext.DeviceName;
			if (sfd.ShowDialog() == true)
			{
				_typesContainer.Resolve<ISerializerService>().SerializeInFile(
					_uniconJournal,
					sfd.FileName);
			}
		}

		public async Task LoadJournal()
		{
			try
			{
				CanExecuteJournalLoading = false;
				Table = new DynamicDataTable(JournalParametersNameList, null, true);
				RaisePropertyChanged(nameof(Table));
				RaisePropertyChanged(nameof(JournalParametersNameList));
                if (_loadingTask == null)
                {
                    _loadingTask = _journalLoaderProvider
                        .GetJournalLoader( DeviceContext, _uniconJournal, _loaderHooks)
                        .Load();
                }

                await _loadingTask;
				_wasLoadedOnce = true;
			}
			catch (Exception e)
			{
				_applicationGlobalCommands.ShowErrorMessage(e.ToString(), this);
				_applicationGlobalCommands.ShowErrorMessage(ApplicationGlobalNames.StatusMessages.JOURNAL_READING_ERROR,
					this);
			}
			finally
			{
				_loadingTask = null;
			}

			CanExecuteJournalLoading = true;
		}

        private void OnExecuteLoadJournal()
        {
            _applicationGlobalCommands.SelectFileToOpen(_localizerService.GetLocalizedString("OpenJournal"),
                " UJR файл (*.ujr)|*.ujr" + "|Все файлы (*.*)|*.* ").OnSuccess(info =>
            {
                var loadedJournal = _typesContainer.Resolve<ISerializerService>()
                    .DeserializeFromFile<IUniconJournal>(info.FullName);
                if (!JournalStructureHelper.IsJournalStructureSimilar(_uniconJournal, loadedJournal))
                {
                    if (!_applicationGlobalCommands.AskUserGlobal(
                        _localizerService.GetLocalizedString(ApplicationGlobalNames.StatusMessages
                            .JOURNAL_STRUCTURE_WARNING_MESSAGE),
                        _localizerService.GetLocalizedString("Warning")))
                    {
                        return;
                    }
                }

                Table = new DynamicDataTable(JournalParametersNameList, null, true);
                RaisePropertyChanged(nameof(Table));
                RaisePropertyChanged(nameof(JournalParametersNameList));
                _journalLoaderProvider
                    .GetJournalLoader(DeviceContext, _uniconJournal, _loaderHooks)
                    .LoadFromReadyModelList(loadedJournal.JournalRecords);
                _wasLoadedOnce = true;
                _logService.LogMessage(_localizerService
                    .GetLocalizedString("JournalOpened") + " " + info.FullName);
            });
        }

        private bool CanLoadExecute()
		{
			return CanExecuteJournalLoading;
		}

		private Task _loadingTask = null;

		private async void OnLoadJournal()
		{
			await LoadJournal();
		}


		public string StrongName =>
			JournalKeys.UNICON_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


		public List<string> JournalParametersNameList
		{
			get { return _journalParametersNameList; }
			set
			{
				_journalParametersNameList = value;
				RaisePropertyChanged();
			}
		}

		public DynamicDataTable Table
		{
			get { return _table; }
			set
			{
				_table = value;
				RaisePropertyChanged();
			}
		}

		public ICommand LoadCommand { get; set; }

		public bool CanExecuteJournalLoading
		{
			get { return _canExecuteJournalLoading; }
			set
			{
				_canExecuteJournalLoading = value;
				RaisePropertyChanged();
			}
		}

		public string NameForUiKey => _localizerService.GetLocalizedString(JournalKeys.UNICON_JOURNAL) + "(" +
		                              _uniconJournal.Name + ")";

		public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

		public async Task<Result> Initialize(IDeviceFragment deviceFragment)
		{
			IUniconJournal uniconJournal = deviceFragment as IUniconJournal;
			_uniconJournal = uniconJournal;
			JournalParametersNameList = new List<string>();
			foreach (IJournalParameter journalParameter in uniconJournal.RecordTemplate.JournalParameters)
			{
				if (journalParameter is IComplexJournalParameter)
				{
					foreach (ISubJournalParameter subJournalParameter in (journalParameter as IComplexJournalParameter)
						.ChildJournalParameters)
					{
						JournalParametersNameList.Add(subJournalParameter.Name);
					}
				}
				else
				{
					JournalParametersNameList.Add(journalParameter.Name);
				}
			}

			Table = new DynamicDataTable(JournalParametersNameList, null, true);
			RaisePropertyChanged(nameof(Table));
			RaisePropertyChanged(nameof(JournalParametersNameList));
            return Result.Create(true);
		}

		public DeviceContext DeviceContext { get; set; }

		public IUniconJournal UniconJournal => _uniconJournal;

		private bool _isOpened = false;

		public async Task SetFragmentOpened(bool isOpened)
		{
			if (isOpened && !_isOpened && !_wasLoadedOnce)
			{
				if (_applicationSettingsService.IsFragmentAutoLoadEnabled)
					LoadCommand?.Execute(null);

			}

			_isOpened = isOpened;
		}

		private bool _wasLoadedOnce = false;
        private LoaderHooks _loaderHooks;
        public string FileExtension => "ujr";

	}
}