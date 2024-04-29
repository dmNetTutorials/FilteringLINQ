namespace FilteringDocsUsingLINQ.Models.Documents
{
    public abstract class DocumentBase : IDocument
    {
        public long Id { get; protected set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string Type { get; protected set; }
        public IEnumerable<string> Versions { get; set; }

        protected DocumentBase(string title,
                               string type)
        {
            Title = title ?? String.Empty;
            Id = (new Random()).Next(1, 10);
            CreationDate = GetRandomDate();
            Type = type;
            Versions = GetVersions();
        }

        /// <summary>
        /// Згенерувати випадковий перелік 
        /// версій документа
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetVersions()
        {
            var rnd = new Random();
            var versions = new List<string>();

            for (short version = 0; version < rnd.Next(1, 3); version++)
            {
                versions.Add($"v.{version + 1}.0");
            }

            return versions;

        }

        /// <summary>
        /// Оновити ідентифіктор документа
        /// </summary>
        /// <param name="newId"></param>
        public void UpdateId(long newId)
        {
            this.Id = newId;
        }

        /// <summary>
        /// Згенерувати випадкову дату
        /// </summary>
        /// <returns></returns>
        private DateTime GetRandomDate()
        {
            var rnd = new Random();
            // Отримати сьогоднішню дату
            var today = DateTime.Now;
            // Додати випадкову кількість днів, 
            // до сьогоднішьної дати
            return today.AddDays(rnd.Next(0, DateTime.DaysInMonth(today.Year,
                                                                  today.Month)));
        }

        protected abstract IDocument Convert(IDocument document);

        /// <summary>
        ///  Порівняння документів відбувається 
        ///  за їх ідентифікаторами
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>
        /// Якщо індентифікатори обох документів
        /// однакові, то вони однакові,
        /// в усіх інших випадках - різні
        /// </returns>
        public override bool Equals(object obj)
        {
            return this?.Id == (obj as IDocument)?.Id;
        }

        /// <summary>
        /// Отримати хєш ідентифікатора документа
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Виведення метаданих документа
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Type} документ №:\t{Id}" +
                   $"{Environment.NewLine}{Title}" +
                   $"{Environment.NewLine}" +
                   $"від\t{CreationDate.ToString("dd.MM.yyyy")}";
        }

    }
}
