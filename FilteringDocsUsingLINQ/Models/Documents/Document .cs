namespace FilteringDocsUsingLINQ.Models.Documents
{
    /// <summary>
    /// Новий вхідний Документ
    /// </summary>
    public class Document : DocumentBase
    {

        public const string InternalDocumentTypeTitle = "Внутрішній";
        public const string ExternalDocumentTypeTitle = "Вхідний";

        private Document(string title,
                         string type)
                         : base(title,
                                type)
        { }

        /// <summary>
        /// Фабричний метод для створення 
        /// внутрішнього документа
        /// </summary>
        /// <returns> Внутрішній документ
        /// </returns>
        public static IDocument CreateInternal(string Title = default)
        {
            return new Document(Title, InternalDocumentTypeTitle);
        }

        /// <summary>
        /// Фабричний метод для створення 
        /// вхідного документа
        /// </summary>
        /// <returns> Вхідний документ
        /// </returns>
        public static IDocument CreateExternal(string Title = default)
        {
            return new Document(Title, ExternalDocumentTypeTitle);
        }

        protected override IDocument Convert(IDocument document)
        {
            var newdDocument = new Document(document?.Title,
                                            InternalDocumentTypeTitle);

            newdDocument.Id = document.Id;
            newdDocument.CreationDate = document.CreationDate;
            newdDocument.Versions = document.Versions;

            return newdDocument;
        }

        /// <summary>
        /// Зробити з архівного документа внутрішній
        /// </summary>
        /// <param name="archived"></param>
        public static explicit operator Document(ArchivedDocument archived)
        {
            var doc = new Document(archived?.Title, InternalDocumentTypeTitle);

            return doc?.Convert(archived) as Document;
        }

    }

}
