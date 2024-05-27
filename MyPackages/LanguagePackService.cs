using CourseProject_backend.Enums.Packages;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseProject_backend.Packages
{
    public class LanguagePackService
    {
        private static LanguagePackService _instance = null;

        private LanguagePackService()
        {
            Packages = new Dictionary<string, IDictionary<string, string>>();
        }

        public static LanguagePackService GetInstance()
        {
            if(_instance == null)
            {
                _instance = new LanguagePackService();
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

        public KeyValuePair<string, IDictionary<string, string>> GetLanguagePackPair(AppLanguage language)
        {
            IDictionary<string, string> langPack = GetLanguagePack(language);

            return new KeyValuePair<string, IDictionary<string, string>>(key: language.ToString(),
                                                                         value: langPack);

        }

        private IDictionary<string, IDictionary<string, string>> Packages { get; set; }
    }
}
