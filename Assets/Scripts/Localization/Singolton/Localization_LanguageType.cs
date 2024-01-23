using UnityEngine;

public partial class Localization
{
    public class LanguageType
    {
        public int Id { get; private set; }
        public string CodeISO639_1 { get; private set; }
        public string Name { get; private set; }
        public string File { get; private set; }
        public Sprite Sprite { get; private set; }
        private const string _path = "Banners/";


        public LanguageType(int id, string codeISO639_1, string name, string file)
        {
            Id = id;
            CodeISO639_1 = codeISO639_1;
            Name = name;
            File = file;
            Sprite = Resources.Load<Sprite>(_path + File);
        }
    }
}
