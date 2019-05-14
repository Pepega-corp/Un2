namespace Unicon2.Fragments.Configuration.Infrastructure.Keys
{
    public static class ConfigurationKeys
    {

        public static class Settings
        {
            public const string CONFIGURATION_SETTINGS = "ConfigurationSettings";


            public const string ACTIVATION_CONFIGURATION_SETTING = "ActivationConfigurationSetting";

        }

        public static class Matrix
        {
            public const string LIST_MATRIX_TEMPLATE = "ListMatrixTemplate";
            public const string BOOL_MATRIX_TEMPLATE = "BoolMatrixTemplate";

        }




        public const string WRITING_CONFIGURATION_QUERY = "WritingConfigurationQuery";

        public const string READING_CONFIGURATION_QUERY = "ReadingConfigurationQuery";
        public const string ACTIVATION_CONFIGURATION_QUERY = "ActivationConfigurationQuery";


        public const string DEFAULT_ITEM_GROUP = "DefaultItemGroup";


        public const string DEFAULT_PROPERTY = "DefaultProperty";
        public const string DEPENDENT_PROPERTY = "DependentProperty";


        public const string COMPLEX_PROPERTY = "ComplexProperty";
        public const string SUB_PROPERTY = "SubProperty";


        public const string RUNTIME_DEFAULT_ITEM_GROUP = "RuntimeDefaultItemGroup";
        public const string RUNTIME_DEFAULT_PROPERTY = "RuntimeDefaultProperty";

        public const string RUNTIME = "Runtime";
        public const string DEPENDANCY_CONDITION = "DependancyCondition";

        
        public const string APPOINTABLE_MATRIX = "AppointableMatrix";
        public const string MATRIX_VARIABLE = "MatrixVariable";



        /// <summary>
        ///    строка перенос значений из устройства на компьютер
        /// </summary>
        public const string TRANSFER_FROM_DEVICE_TO_LOCAL_STRING_KEY =
            "TransferFromDeviceToLocal";



        /// <summary>
        ///    строка перенос значений из компьютера на устройство
        /// </summary>
        public const string WRITE_LOCAL_VALUES_TO_DEVICE_STRING_KEY =
            "WriteLocalValuesToDevice";

        /// <summary>
        ///    строка сохранения конфигурации
        /// </summary>
        public const string SAVE_CONFUGURATION_STRING_KEY =
            "SaveConfiguration";

        /// <summary>
        ///    строка сохранения конфигурации
        /// </summary>
        public const string LOAD_FROM_FILE_STRING_KEY =
            "LoadFromFile";


        /// <summary>
        ///    строка развернуть уровень
        /// </summary>
        public const string EXPAND_LEVEL_STRING_KEY =
            "ExpandLevel";

        /// <summary>
        ///    строка развернуть уровень
        /// </summary>
        public const string COLLAPSE_LEVEL_STRING_KEY =
            "CollapseLevel";
    }
}