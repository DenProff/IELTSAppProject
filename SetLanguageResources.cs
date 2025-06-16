using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IELTSAppProject
{
    public class SetLanguageResources
    {
        public static void SetLanguageResourcesMethod(string languageCode,/* string[] resourcesKeysArray,*/ object sender)
        {
            // Получение ресурсов для одного из языков
            ResourceManager resourceManager = new ResourceManager("IELTSAppProject.Resources.Resources", Assembly.GetExecutingAssembly());

            CultureInfo culture = new CultureInfo(languageCode);
            using (ResourceSet resourceSet = resourceManager.GetResourceSet(culture, true, true))
            {
                // Перебор всех ключей и обновление ресурсов в контроле или странице
                foreach (DictionaryEntry entry in resourceSet)
                {
                    string key = entry.Key.ToString();
                    string value = resourceManager.GetString(key, culture);

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

            //// Получение из ресурсов текстов по заданному ключу
            //foreach (string key in resourcesKeysArray)
            //{
            //    if (sender is Page)
            //    {
            //        ((Page)sender).Resources[key] = resourceManager.GetString(key, new CultureInfo(languageCode));
            //    }
            //    else
            //    {
            //        ((UserControl)sender).Resources[key] = resourceManager.GetString(key, new CultureInfo(languageCode));
            //    }
            //}
        }
    }
}
