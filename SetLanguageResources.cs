using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IELTSAppProject
{
    public class SetLanguageResources
    {
        private static ResourceManager ResourceManager // Экземпляр класса ResourceManager необходим для работы с ресурсами
        {
            get => new ResourceManager("IELTSAppProject.Resources.Resources", Assembly.GetExecutingAssembly());
        }
            

        public static void SetLanguageResourcesMethod(string languageCode,/* string[] resourcesKeysArray,*/ object sender)
        {
            CultureInfo culture = new CultureInfo(languageCode);
            using (ResourceSet resourceSet = ResourceManager.GetResourceSet(culture, true, true)) // ResourceSet реализует IDisposable, using нужен
                                                                                                  // для освобождения памяти. Сам ResourceSet -
                                                                                                  // загрузка ресурсов конкретной культуры
            {
                // Перебор всех пар ключ-значение и обновление ресурсов в контроле или странице
                foreach (DictionaryEntry entry in resourceSet)
                {
                    string key = entry.Key.ToString();
                    string value = ResourceManager.GetString(key, culture);

                    if (sender is Page page)
                    {
                        page.Resources[key] = value;
                    }
                    else if (sender is UserControl userControl)
                    {
                        userControl.Resources[key] = value;
                    }
                    else if (sender is Window window)
                    {
                        window.Resources[key] = value;
                    }
                }
            }
        }

        public static string GetString(string languageCode, string key) // Функция для использования файлов ресурсов прямо в cs-файлах
        {
            CultureInfo culture = new CultureInfo(languageCode);

            // По ключу и установленной через LanguageChange.SetLanguage культуре из ресурсов возвращается значение
            return ResourceManager.GetString(key, culture) ?? key; // Если из ресурсов возвращается null, то функцией возвращается ключ                                                                           // ключ - иначе были бы исключения
        }
    }
}
