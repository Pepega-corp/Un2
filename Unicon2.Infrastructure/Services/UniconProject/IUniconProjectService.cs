using System.Collections.Generic;

namespace Unicon2.Infrastructure.Services.UniconProject
{
    public interface IUniconProjectService
    {
        /// <summary>
        /// Создать новый проект 
        /// </summary>
        void CreateNewProject();
        /// <summary>
        /// Сохранить текущий проект
        /// </summary>
        void SaveProject();
        /// <summary>
        /// Сохранить текущий проект как
        /// </summary>
        void SaveProjectAs();
        /// <summary>
        /// Проверить сохранен ли проект
        /// </summary>
        /// <returns>Результат проверки</returns>
        ProjectSaveCheckingResultEnum CheckIfProjectSaved(object context);
        /// <summary>
        /// Строка файла текущего проекта для вывода пользователю
        /// </summary>
        string CurrentProjectString { get; }
        /// <summary>
        /// Получить список последних проектов
        /// </summary>
        /// <returns></returns>
        List<string> GetLastProjectsList();
        /// <summary>
        /// Открыть проект
        /// </summary>
        /// <param name="lastProjectString">Необязательная строка, обозначающая предыдущий проект</param>
        void OpenProject(string lastProjectString = "");

        /// <summary>
        /// Установить контекст для вывода диалоговых сообщений
        /// </summary>
        /// <param name="obj"></param>
        void SetDialogContext(object obj);


        /// <summary>
        /// Получить путь к проекту в файловой системе
        /// </summary>
        /// <returns>путь к проекту</returns>
        string GetProjectPath();
    }

}