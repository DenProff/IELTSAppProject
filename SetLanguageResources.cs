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
using System.Windows.Controls;

namespace IELTSAppProject
{
    public class SetLanguageResources
    {
        private static ResourceManager ResourceManager
        {
            get => new ResourceManager("IELTSAppProject.Resources.Resources", Assembly.GetExecutingAssembly());
        }
            

        public static void SetLanguageResourcesMethod(string languageCode,/* string[] resourcesKeysArray,*/ object sender)
        {
            CultureInfo culture = new CultureInfo(languageCode);
            using (ResourceSet resourceSet = ResourceManager.GetResourceSet(culture, true, true))
            {
                // Перебор всех ключей и обновление ресурсов в контроле или странице
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
                }
            }
        }

        public static string GetString(string languageCode, string key)
        {
            CultureInfo culture = new CultureInfo(languageCode);
            using (ResourceSet resourceSet = ResourceManager.GetResourceSet(culture, true, true))
            {
                // По ключу и установленной через LanguageChange.SetLanguage культуре из ресурсов возвращается значение
                return ResourceManager.GetString(key, culture) ?? key; // Если из ресурсов возвращается null, то выводится
            }                                                                               // ключ - иначе были бы исключения
        }
    }
}
