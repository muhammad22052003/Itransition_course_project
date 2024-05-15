using CourseProject_backend.Enums.Packages;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseProject_backend.Packages
{
    public class LanguagePackSingleton
    {
        private static LanguagePackSingleton _instance = null;

        private LanguagePackSingleton()
        {
            Packages = new Dictionary<string, IDictionary<string, string>>();
        }

        public static LanguagePackSingleton GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LanguagePackSingleton();
            }

            return _instance;
        }

        public IDictionary<string, string> GetLanguagePack(AppLanguage language)
        {
            if (!Packages.ContainsKey(language.ToString()))
            {
                StringBuilder json = new StringBuilder();

                json = new StringBuilder(File.ReadAllText($"{language.ToString()}Pack.json"));

                var jsonDocument = JsonDocument.Parse(json.ToString());

                Packages.Add(key: language.ToString(),
                             value: JsonSerializer
                             .Deserialize<IDictionary<string, string>>(jsonDocument));
            }

            return Packages[language.ToString()];
        }

        private IDictionary<string, IDictionary<string, string>> Packages { get; set; }
    }
}
