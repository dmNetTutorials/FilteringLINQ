using FilteringDocsUsingLINQ.Models.Documents;

namespace FilteringDocsUsingLINQ.DataServices
{
    public class DocumentDataServiceMock : IDocumentDataService
    {
        /// <summary>
        /// Отримати перелік всіх документів
        /// </summary>
        /// <returns></returns>
        public ICollection<IDocument> GetAllArchiveDocuments()
        {
            return new List<IDocument>() {
                  Document.CreateExternal("Постанова з міністерства"),
                  ArchivedDocument.ArchiveDocument("Про дохід"),
                  Document.CreateExternal("Наказ з міністерства"),
                  ArchivedDocument.ArchiveDocument("Соціальні виплати"),
                  Document.CreateInternal("Внутрішнє розпорядження"),
                  ArchivedDocument.ArchiveDocument("Оплата контракту"),
            };
        }
        /// <summary>
        /// Отримати перелік всіх звітів
        /// </summary>
        /// <returns></returns>
        public ICollection<IDocument> GetAllRecentDocuments()
        {
            return new List<IDocument>() {
                  Document.CreateExternal($"Розпорядження з міністерства"),
                  ArchivedDocument.ArchiveDocument($"Витрати на конференції"),
                  Document.CreateInternal($"Наказ декана"),
                  ArchivedDocument.ArchiveDocument($"Річні витрати на зарплатню"),
                  Document.CreateInternal($"Наказ ректора"),
                  ArchivedDocument.ArchiveDocument($"Річні витрати на господарську частину"),
            };
        }
    }

}
