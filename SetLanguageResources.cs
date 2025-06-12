using System;
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
        public static void SetLanguageResourcesMethod(string languageCode, string[] resourcesKeysArray, object sender)
        {
            // Получение ресурсов для одного из языков
            ResourceManager resourceManager = new ResourceManager("IELTSAppProject.Resources.Resources", Assembly.GetExecutingAssembly());

            // Получение из ресурсов текстов по заданному ключу
            foreach (string key in resourcesKeysArray)
            {
                ((Page)sender).Resources[key] = resourceManager.GetString(key, new CultureInfo(languageCode));
            }
        }
    }
}
