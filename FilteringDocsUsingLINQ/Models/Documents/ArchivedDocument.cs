namespace FilteringDocsUsingLINQ.Models.Documents
{
    /// <summary>
    /// Архівний документ
    /// </summary>
    public class ArchivedDocument : DocumentBase
    {

        public const string ArchivedTypeTitle = "Архівний";

        private ArchivedDocument(string title,
                                 string type)
                                 : base(title,
                                        type)
        { }

        /// <summary>
        /// Фабричний метод для створення 
        /// архівного документу
        /// </summary>
        /// <returns>  Архівний документ
        /// </returns>
        public static IDocument ArchiveDocument(string Title = default)
        {
            return new ArchivedDocument(Title, ArchivedTypeTitle);
        }

        protected override IDocument Convert(IDocument document)
        {
            var newArchivedDocument = new ArchivedDocument(document?.Title,
                                                           ArchivedTypeTitle);

            newArchivedDocument.Id = document.Id;
            newArchivedDocument.CreationDate = document.CreationDate;
            newArchivedDocument.Versions = document.Versions;

            return newArchivedDocument;
        }

        /// <summary>
        /// Зробити зі звичайного документа архівний
        /// </summary>
        /// <param name="doc"></param>
        public static explicit operator ArchivedDocument(Document doc)
        {
            if (doc != null)
            {
                var archived = new ArchivedDocument(doc?.Title, ArchivedTypeTitle);

                return archived?.Convert(doc) as ArchivedDocument;
            }
            else return null;
        }

    }


}
