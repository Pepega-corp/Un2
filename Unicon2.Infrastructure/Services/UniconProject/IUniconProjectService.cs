using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Services.UniconProject
{
    public interface IUniconProjectService
    {
	    /// <summary>
	    /// Создать новый проект 
	    /// </summary>
	    /// <param name="shellViewModel"></param>
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
        ProjectSaveCheckingResultEnum CheckIfProjectSaved();
        /// <summary>
        /// Строка файла текущего проекта для вывода пользователю
        /// </summary>
        string CurrentProjectString { get; }
        /// <summary>
        /// Открыть проект
        /// </summary>
        /// <param name="lastProjectString">Необязательная строка, обозначающая предыдущий проект</param>
        void OpenProject();

        /// <summary>
        /// Установить контекст для вывода диалоговых сообщений
        /// </summary>
        /// <param name="obj"></param>
        void SetDialogContext(object obj);


        /// <summary>
        /// Получить путь к проекту в файловой системе
        /// </summary>
        /// <returns>путь к проекту</returns>
        string GetProjectTitle();

        void LoadDefaultProject();
        Task LoadProject(string path);
    }

}