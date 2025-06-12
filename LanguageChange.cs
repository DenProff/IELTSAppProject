using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IELTSAppProject
{
    public class LanguageChange
    {
        public static event Action LanguageChanged;

        public static void SetLanguage(string languageCode) // languageCode: ru, en, es - и другие обозначения языков
        {
            // Сохранение языка в свойствах проекта
            Properties.Settings.Default.Language = languageCode; // Language - свойство проекта, по умолчанию поставлено одним из разработчиков "ru",
                                                                 // его можно изменить в Settings.settings проекта
            Properties.Settings.Default.Save(); // При запуске приложения будет запущен язык с последней сессии

            // Обновление культуры приложения для работы ResourceManager
            CultureInfo culture = new CultureInfo(languageCode);
            CultureInfo.CurrentUICulture = culture;

            LanguageChanged?.Invoke(); // Генерация события
        }
    }
}
