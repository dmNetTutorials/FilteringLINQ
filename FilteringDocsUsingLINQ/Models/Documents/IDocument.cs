namespace FilteringDocsUsingLINQ.Models.Documents
{
    /// <summary>
    /// Загальна обов’язкова структура будь-якого електронного документа
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Ідентифікаційний номер документа
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Назва документа
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Дата створення документа
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Версії документа для відслідковування змін
        /// </summary>
        IEnumerable<string> Versions { get; set; }

    }
}
