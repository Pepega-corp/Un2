namespace Unicon2.Infrastructure
{
    /// <summary>
    /// строки, общие для всего приложения
    /// </summary>
    public static class ApplicationGlobalNames
    {
        public static class UiGroupingStrings
        {
            /// <summary>
            ///    строка file
            /// </summary>
            public const string FILE_STRING_KEY = "File";
            /// <summary>
            ///   строка, обозначающая устройство
            /// </summary>
            public const string DEVICE_STRING_KEY = "Device";

            /// <summary>
            ///   строка, обозначающая устройство
            /// </summary>
            public const string TREE_STRING_KEY = "Tree";
        }


        public static class UiCommandStrings
        {
            /// <summary>
            ///    строка edit
            /// </summary>
            public const string EDIT_STRING_KEY = "Edit";

            /// <summary>
            ///    строка read
            /// </summary>
            public const string READ_STRING_KEY = "Read";
  
            public const string SAVE_FOR_PRINT = "SaveForPrint";
            public const string COMMAND = "CommandMenuItem";

            public const string OPEN_PROJECT = "OpenProject";
            public const string NEW_PROJECT = "NewProject";
            public const string SAVE_PROJECT = "Save";
            public const string SAVE_AS_PROJECT = "SaveAs";
            public const string EXIT = "Exit";
            public const string ADD = "Add";
            public const string OPEN_DEVICE_EDITOR = "OpenDeviceEditor";
            public const string OPEN_OSC = "OpenOscillogram";
            public const string OPEN_WEB_SYNC = "OpenWebSync";

        }

        public static class WindowsStrings
        {
            /// <summary>
            ///    строка notifications
            /// </summary>
            public const string NOTIFICATIONS_STRING_KEY = "Notifications";

            /// <summary>
            ///    строка notifications
            /// </summary>
            public const string PROJECT_STRING_KEY = "Project";
        }
        
        public static class CommonInjectionStrings
        {
            /// <summary>
            /// строка вью-модель
            /// </summary>
            public const string VIEW_MODEL = "ViewModel";

            /// <summary>
            /// Строка Editor
            /// </summary>
            public const string EDITOR = "Editor";

            /// <summary>
            /// строка  EditorViewModel
            /// </summary>
            public const string EDITOR_VIEWMODEL = "EditorViewModel";

            /// <summary>
            /// строка DataTemplate
            /// </summary>
            public const string DATATEMPLATE = "DataTemplate";

            /// <summary>
            /// строка Editable
            /// </summary>
            public const string EDITABLE = "Editable";
        }

        
        public static class DialogStrings
        {
            public static string SAVING = "Saving";


            public static string REPLACE_EXISTING_ITEM_QUESTION = "ReplaceExistingItemQuestion";


            public static string DELETE_SELECTED_ITEM_QUESTION = "DeleteSelectedItemQuestion";

            /// <summary>
            ///  строка буффер
            /// </summary>
            public const string BUFFER = "Buffer";

            /// <summary>
            ///  строка буффер пуст
            /// </summary>
            public const string BUFFER_EMPTY_MESSAGE = "BufferEmpty";

            /// <summary>
            ///  строка в буффере неверный тип данных
            /// </summary>
            public const string BUFFER_INVALID_TYPE_MESSAGE = "BufferInvalidType";

            /// <summary>
            ///  строка обработка значений
            /// </summary>
            public const string PROCESSING_VALUES = "ProcessingValues";
            
            /// <summary>
            ///  строка выход
            /// </summary>
            public const string EXIT = "Exit";

            /// <summary>
            ///  строка вопроса пользователю о подтверждении выхода
            /// </summary>
            public const string EXIT_QUESTION = "ExitQuestion";
            
            /// <summary>
            ///  строка удаления
            /// </summary>
            public const string DELETE = "Delete";

            /// <summary>
            ///  строка да
            /// </summary>
            public const string YES = "Yes";

            /// <summary>
            ///  строка нет
            /// </summary>
            public const string NO = "No";

            /// <summary>
            ///  строка подтверждение
            /// </summary>
            public const string SUBMIT = "Submit";
            /// <summary>
            ///  строка отмена
            /// </summary>
            public const string CANCEL = "Cancel";
        }


        public static class StatusMessages
        {
            /// <summary>
            /// ошибка невозможности проверить динамические значения
            /// </summary>
            public const string DYNAMIC_VALUES_CHECKING_IMPOSSIBLE = "DynamicValuesCheckingImpossible";

            /// <summary>
            ///  строка сообщения об ошибке порта
            /// </summary>
            public const string PORT_ERROR_MESSAGE = "PortErrorMessage";

            /// <summary>
            ///  строка сообщения об ошибке формата
            /// </summary>
            public const string FORMAT_ERROR = "FormatError";

            /// <summary>
            ///  строка сообщения об ошибке формата
            /// </summary>
            public const string DATE_FORMAT = "DateFormat";
            /// <summary>
            ///  строка сообщения о неверном диапазоне
            /// </summary>
            public const string INVALID_RANGE_ERROR = "InvalidRangeError";

            /// <summary>
            ///  строка сообщения об ошибке
            /// </summary>
            public const string ERROR = "Error";

            /// <summary>
            ///  строка сообщения о повторении в коллекции
            /// </summary>
            public const string DUBLICATE_VALUES_MESSAGE = "DublicateValuesMessage";

            /// <summary>
            ///  строка сообщения о пустом или null значении
            /// </summary>
            public const string NULL_OR_EMPTY_MESSAGE = "NullOrEmptyMesasage";

            /// <summary>
            /// сообщение о невыбранном устройстве
            /// </summary>
            public static string SELECTED_DEVICE_NULL_MESSAGE = "SelectedDeviceNullMessage";

            /// <summary>
            /// сообщение о невыбранном подключении
            /// </summary> 
            public static string SELECTED_CONNECTION_NULL_MESSAGE = "SelectedConnectionNullMessage";

            /// <summary>
            /// сообщение о несоответствии значению его пределов
            /// </summary> 
            public static string VALUE_OUT_OF_RANGE_MESSAGE_KEY = "ValueOutOfRange";

            /// <summary>
            /// сообщение о несоответствии значению его пределов
            /// </summary> 
            public static string JOURNAL_READING_ERROR = "JournalReadingError";

            /// <summary>
            /// сообщение об успехе сохранения в файл
            /// </summary> 
            public static string FILE_EXPORT_SUCCESSFUL = "FileExportSuccessful";

            /// <summary>
            /// сообщение об ошибке чтения устройства
            /// </summary> 
            public static string DEVICE_READING_ERROR = "DeviceReadingError";
        }

        public static class QueriesNames
        {
            /// <summary>
            ///    Название обмена для мр-сети (отображения памяти)
            /// </summary>
            public const string MODBUS_MEMORY_QUERY_KEY = "ModbusMemoryQuery";

            /// <summary>
            ///    Название обмена для мр-сети (отображения памяти)
            /// </summary>
            public const string WRITE_MODBUS_MEMORY_QUERY_KEY = "WriteModbusMemoryQuery";

            /// <summary>
            ///    Название обмена для мр-сети (отображения памяти)
            /// </summary>
            public const string WRITE_CONFIGURATION_QUERY_KEY = "WriteConfigurationQuery";

            /// <summary>
            ///    Название единичного обмена для мр-сети (отображения памяти)
            /// </summary>
            public const string MODBUS_SINGLE_MEMORY_QUERY_KEY = "ModbusSingleMemoryQuery";

            /// <summary>
            ///    Название обмена для мр-сети (отображения памяти)
            /// </summary>
            public const string READING_PROPERTY_QUERY = "ReadingPropertyQuery";
        }


        public static class ViewModelNames
        {
            /// <summary>
            ///    Имя вью-модели главного окна
            /// </summary>
            public const string SHELL_VIEW_MODEL_NAME = "ShellViewModelName";

            /// <summary>
            ///    Имя вью-модели главного окна
            /// </summary>
            public const string RANGE_VIEW_MODEL_NAME = "RangeViewModel";

            /// <summary>
            ///    Имя вью-модели для вьюхи редактирования устройств
            /// </summary>
            public const string DEVICEEDITING_VIEW_MODEL_NAME = "DeviceEditingViewModelName";
        }

        public static class ViewNames
        {
            /// <summary>
            ///    Имя вью редактирования устройств
            /// </summary>
            public const string DEVICEEDITING_VIEW_NAME = "DeviceEditingView";

            /// <summary>
            ///    Имя вью редактира устройств
            /// </summary>
            public const string DEVICEEDITOR_VIEW_NAME = "DeviceEditorView";

            /// <summary>
            ///    Имя региона браузера
            /// </summary>
            public const string BROWSER_PANEL_REGION_NAME = "BrowserPanelRegion";


            /// <summary>
            ///    Имя региона flyout-а редактирования устройств 
            /// </summary>
            public const string DEVICE_EDITING_FLYOUT_REGION_NAME = "DeviceEditingFlyOutRegion";
        }

        public static class DefaultStringsForUi
        {
            /// <summary>
            /// строка NewDevice
            /// </summary>
            public static string NEW_DEVICE_STRING = "NewDevice";

            /// <summary>
            /// строка All
            /// </summary>
            public static string ALL_STRING_KEY = "All";

            /// <summary>
            /// строка-ключ recent notifications (новые уведомления)
            /// </summary>
            public static string RECENT_NOTIFICATIONS_STRING_KEY = "RecentNotifications";
        }
        

        public static class FragmentInjectcionStrings
        {
            /// <summary>
            /// строка ModbusMemory
            /// </summary>
            public static string MODBUSMEMORY = "ModbusMemory";

            /// <summary>
            /// строка Configuration
            /// </summary>
            public static string CONFIGURATION = "Configuration";

            /// <summary>
            /// строка Configuration Editor
            /// </summary>
            public static string CONFIGURATION_EDITOR = "ConfigurationEditor";
            
            /// <summary>
            /// строка Configuration Editor datatemplate
            /// </summary>
            public static string CONFIGURATION_EDITOR_DATATEMPLATE = "ConfigurationEditorDataTemplate";
            /// <summary>
            /// строка ConfigurationViewModel
            /// </summary>
            public static string CONFIGURATION_VIEWMODEL = "ConfigurationViewModel";
            /// <summary>
            /// строка RuntimeConfigurationViewModel
            /// </summary>
            public static string RUNTIME_CONFIGURATION_VIEWMODEL = "RuntimeConfigurationViewModel";
            /// <summary>
            /// строка ConfigurationViewModel
            /// </summary>
            public static string CONFIGURATION_VIEWMODEL_DATATEMPLATE = "ConfigurationViewModelDataTemplate";
        }
        
        /// <summary>
        /// папка с устройствами по умолчаню
        /// </summary>
        public const string DEFAULT_DEVICES_FOLDER_PATH = "Devices";

        public const string QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING = "QuickAccessMemoryConfigurationSetting";

        /// <summary>
        ///   Режим редактирования
        /// </summary>
        public const string EDITING_MODE = "EditingMode";

        /// <summary>
        ///   Режим добавления
        /// </summary>
        public const string ADDING_MODE = "AddingMode";

        /// <summary>
        ///   Название фабрики для создания соединения по ModBusRtu
        /// </summary>
        public const string EMPTY_DEVICE = "EmptyDevice";

        /// <summary>
        ///   Название фабрики для создания offline соединения
        /// </summary>
        public const string OFFLINE_CONNECTION_FACTORY_NAME = "OfflineConnectionFactory";

        /// <summary>
        ///   Строка ссылка
        /// </summary>
        public const string REFERENCE = "Reference";
        
        /// <summary>
        ///   Строка форматтер
        /// </summary>
        public const string FORMATTER = "Formatter";

        /// <summary>
        ///  строка запроса пользователю на удаление элементов с дочерними элементами
        /// </summary>
        public const string REALLY_WANT_TO_DELETE_ELEMENT_WITH_CHILD_REQUEST_STRING = "ReallyWantToDeleteWithChildRequest";

        /// <summary>
        ///  строка удаление
        /// </summary>
        public const string DELETING = "Deleting";

        /// <summary>
        ///  строка загружена 
        /// </summary>
        public const string IS_LOADED = "IsLoaded";

        /// <summary>
        ///    строка general (общее)
        /// </summary>
        public const string GENERAL = "General";
    }
}