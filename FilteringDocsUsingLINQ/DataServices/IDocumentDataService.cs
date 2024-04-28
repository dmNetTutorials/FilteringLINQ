using FilteringDocsUsingLINQ.Models.Documents;

namespace FilteringDocsUsingLINQ.DataServices
{
    /// <summary>
    /// Управління документами
    /// Приклад, реалізації шаблону 
    /// Repository
    /// </summary>
    public interface IDocumentDataService
    {

        public ICollection<IDocument> GetAllArchiveDocuments();
        public ICollection<IDocument> GetAllRecentDocuments();
    }

}
