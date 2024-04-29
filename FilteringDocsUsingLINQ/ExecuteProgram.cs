using FilteringDocsUsingLINQ.DataServices;
using FilteringDocsUsingLINQ.Models.Documents;

namespace FilteringDocsUsingLINQ
{
    public static class ExecuteProgram
    {
        enum MenuOptions
        {
            EmptyDocsList = 1,
            DeadlinesFilter,
            RecentDocsFIlter,
            NewestDocsFIlter,
            ExistingDocsFIlter,
            Exit,
        }

        /// <summary>
        /// Колекція архівних документів
        /// </summary>
        private static IEnumerable<IDocument> _archivedDocuments;

        /// <summary>
        /// Коллекція нових документів
        /// </summary>
        private static IEnumerable<IDocument> _newDocuments;

        /// <summary>
        /// Доступ до тестових даних
        /// </summary>
        private static IDocumentDataService _documentsRepository;

        private static string _delimiter;

        static ExecuteProgram()
        {
            _documentsRepository = new DocumentDataServiceMock();
            _archivedDocuments = _documentsRepository?.GetAllArchiveDocuments();
            _newDocuments = _documentsRepository?.GetAllRecentDocuments();

            short index = 2; // Індекс документа в колекції з однаковими датами створення
            // Задати однакові дати створення документів
            _archivedDocuments.ElementAt(index).CreationDate = DateTime.Now;
            _newDocuments.ElementAt(index).CreationDate = DateTime.Now;

            for (short i = 0; i < Console.BufferWidth; i++)
                _delimiter += "-";

        }

        private static void Out<T>(IEnumerable<T> sequence)
        {
            if (sequence != null)
            {
                Console.WriteLine(Environment.NewLine);
                short docNumber = 1;
                foreach (var elem in sequence)
                {
                    Console.WriteLine($"{docNumber}.)\t{elem}");
                    docNumber++;
                }
            }
        }

        /// <summary>
        /// Додати необхідну кількість пробілів
        /// </summary>
        /// <param name="text"></param>
        /// <param name="centerPosition"></param>
        private static void ExtendTitleToTableCenter(string text,
                                                     int centerPosition)
        {
            for (int i = text.Length; i < centerPosition; i++)
            {
                Console.Write($" ");
            }
        }

        /// <summary>
        /// Вивести поточну інформацію про колекції документів
        /// </summary>
        public static void ShowAllDocuments(IEnumerable<IDocument> newDocs = null,
                                            IEnumerable<IDocument> archiveDocs = null,
                                            string leftHeaderText = "Нові документи",
                                            string rightHeaderText = "Архівні документи")
        {

            if (newDocs == null)
                newDocs = _newDocuments;
            if (archiveDocs == null)
                archiveDocs = _archivedDocuments;

            // Центр таблиці
            var center = 40;

            var docs = new List<dynamic>();

            var enum1 = newDocs?.GetEnumerator();
            var enum2 = archiveDocs?.GetEnumerator();

            while (enum1.MoveNext() | enum2.MoveNext())
            {
                docs.Add(new
                {
                    NewDoc = enum1?.Current,
                    ArchiveDoc = enum2?.Current,
                });
            }

            Console.WriteLine(_delimiter);
            Console.Write($"{leftHeaderText}");
            ExtendTitleToTableCenter(leftHeaderText, center);

            var delimeterStartPosition = Console.GetCursorPosition().Left;
            Console.Write($"|\t\t");
            var rightColumnStartPosition = Console.GetCursorPosition().Left;
            Console.Write($"{rightHeaderText}");
            Console.WriteLine($"{Environment.NewLine}{_delimiter}");

            foreach (var doc in docs)
            {
                var newDocLines = doc?.NewDoc?.ToString().Split(Environment.NewLine);
                var archiveDocLines = doc?.ArchiveDoc?.ToString().Split(Environment.NewLine);

                for (int i = 0;
                     i < Math.Max(newDocLines?.Length, archiveDocLines?.Length);
                     i++)
                {
                    Console.Write($"{(i < newDocLines?.Length ? newDocLines[i] : "")}");
                    Console.SetCursorPosition(delimeterStartPosition, Console.GetCursorPosition().Top);
                    Console.Write(@"|");
                    Console.SetCursorPosition(rightColumnStartPosition, Console.GetCursorPosition().Top);
                    Console.WriteLine($"{(i < archiveDocLines?.Length ? archiveDocLines[i] : "")}");
                }

                Console.WriteLine(_delimiter);
            }
        }

        /// <summary>
        /// Створити порожну множину документів
        /// </summary>
        private static void CreateEmpty()
        {
            var docs = Enumerable.Empty<IDocument>();

            Console.WriteLine($"Кількість документів:\t{docs?.Count()}");

        }
        /// <summary>
        /// Відфільтрувати документи тільки з певних джерел
        /// </summary>
        private static void GetDocsWithDeadlines()
        {
            // Максимальний час зберігання документа за його типом
            var documentStoringDeadline = new[]
            {
                new {
		            // Архівний документ
		            Type = ArchivedDocument.ArchivedTypeTitle,
		            // Зберігається 5 років
		            Duration = 5, },
                new {
		            // Внутрішній документ
		            Type = Document.InternalDocumentTypeTitle,
		            // Зберігається 1 рік
		            Duration = 1, },
                new {
		            // Вхідний документ
		            Type = Document.ExternalDocumentTypeTitle,
		            // Зберігається 2 роки
		            Duration = 2, },
            };
            // Зпівставлення типу документа та його часу зберігання
            var documentsStoringDeadline = _newDocuments?.Join(documentStoringDeadline,
                                // ключі повинні співпадати за типом даних
                                newDoc => newDoc?.Type,
                                // ключі повинні співпадати за типом даних
                                docType => docType?.Type,
                                (newDoc, docType) =>
                                {
                                    return new
                                    {
                                        Title = newDoc?.Title,
                                        DocTypeTitle = newDoc?.Type,
                                        StorageDuration = docType?.Duration,
                                    };
                                });

            Console.WriteLine(@"Перелік документів, з нормативним часом їх зберігання:");
            // Вивести інформація про час зберігання документа
            foreach (var doc in documentsStoringDeadline)
            {
                Console.WriteLine($"------------------------");
                Console.WriteLine($"Назва документа:\t{doc?.Title}");
                Console.WriteLine($"Тип документа:\t{doc?.DocTypeTitle}");
                Console.WriteLine($"Час зберігання (в роках):\t{doc?.StorageDuration}");

            }
        }
        /// <summary>
        /// Відфільтрувати документи за новизною (які ще не були в архіві)
        /// </summary>
        private static void GetRecentDocs()
        {
            // Згенерувати єдиний ідентифікатор
            var uniqueId = 3;

            // Задати однаковий ідентифікатор для документів
            // з різних колекцій, щоб можна було отримати різницю
            (_newDocuments?.ElementAt(1) as DocumentBase)?.UpdateId(uniqueId);
            (_newDocuments?.ElementAt(3) as DocumentBase)?.UpdateId(uniqueId);
            (_newDocuments?.ElementAt(5) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(4) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(3) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(5) as DocumentBase)?.UpdateId(uniqueId);

            // Отримати перелік документів, які наявні тільки
            // в переліку нових документів
            // Для роботи Except обов'язково в користувацьких
            // типах даних (класах) повинні бути перевизначені методи:
            // bool Equals(object obj) та int GetHashCode()
            var filteredDocs = _newDocuments?.Except(_archivedDocuments);

            // Відобразити таблицю з документами
            ShowAllDocuments();

            Console.WriteLine(@"Перелік документів, які ще не були архівовані:");
            // Вивести тільки нові документи, які ще ні
            // разу не були в архіві
            Out(filteredDocs);

        }
        /// <summary>
        /// Перелік нових та архівних документів в системі
        /// </summary>
        private static void GetNewestDocs()
        {
            // Згенерувати єдиний ідентифікатор
            var uniqueId = 3;

            // Задати однаковий ідентифікатор для документів
            // з різних колекцій, щоб можна було отримати дублікати
            (_newDocuments?.ElementAt(1) as DocumentBase)?.UpdateId(uniqueId);
            (_newDocuments?.ElementAt(3) as DocumentBase)?.UpdateId(uniqueId);
            (_newDocuments?.ElementAt(5) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(4) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(3) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(5) as DocumentBase)?.UpdateId(uniqueId);

            // Отримати перелік унікальних документів,
            // які наявні в обох переліках документів
            // Для роботи Union обов'язково в користувацьких
            // типах даних (класах) повинні бути перевизначені методи:
            // bool Equals(object obj) та int GetHashCode()
            var filteredDocs = _newDocuments?.Union(_archivedDocuments);

            // Відобразити таблицю з документами
            ShowAllDocuments();

            Console.WriteLine(@"Перелік унікальних документів (нових та архівних):");
            // Вивести всі унікальні документи
            // з обох колекцій
            Out(filteredDocs);
        }
        /// <summary>
        /// Перелік документів, які вже були раніше в системі (наявні в архіві з тими ж самими даними)
        /// </summary>
        private static void GetExsistingDocs()
        {
            // Згенерувати єдиний ідентифікатор
            var uniqueId = 3;

            // Задати однаковий ідентифікатор для документів
            // з різних колекцій, щоб можна було отримати дублікати
            (_newDocuments?.ElementAt(1) as DocumentBase)?.UpdateId(uniqueId);
            (_archivedDocuments?.ElementAt(5) as DocumentBase)?.UpdateId(uniqueId);

            // Отримати перелік однакових документів,
            // які наявні в обох переліках документів
            // Для роботи Intersect обов'язково в користувацьких
            // типах даних (класах) повинні бути перевизначені методи:
            // bool Equals(object obj) та int GetHashCode()
            var filteredDocs = _newDocuments?.Intersect(_archivedDocuments);

            // Відобразити таблицю з документами
            ShowAllDocuments();

            Console.WriteLine(@"Перелік документів, які наявні в переліку нових та архівних:");
            // Вивести всі однакових документи
            // з обох колекцій
            Out(filteredDocs);

        }
        static void Menu()
        {
            Console.WriteLine($"Оберіть тип фільтрації:");
            Console.WriteLine($"1.) Створити порожній список документів;");
            Console.WriteLine($"2.) Фільтр документів за часом зберігання (скільки зберігається);");
            Console.WriteLine($"3.) Фільтр документів за новизною (які жодного разу не були в архіві);");
            Console.WriteLine($"4.) Отримати перелік нових та архівних документів в системі;");
            Console.WriteLine($"5.) Фільтр документів, які вже були раніше в системі (наявні в архіві з тими ж самими даними);");
            Console.WriteLine($"6.) Завершити роботу;");

            Byte menuOption;

            while (!Byte.TryParse(Console.ReadLine(), out menuOption))
            {
                Console.WriteLine($"Введено неправильні дані, перевірте та спробуйте знову");
            }

            MenuOptions selectedOption = (MenuOptions)menuOption;

            switch (selectedOption)
            {
                case MenuOptions.EmptyDocsList:
                    CreateEmpty();
                    break;
                case MenuOptions.DeadlinesFilter:
                    GetDocsWithDeadlines();
                    break;
                case MenuOptions.RecentDocsFIlter:
                    GetRecentDocs();
                    break;
                case MenuOptions.NewestDocsFIlter:
                    GetNewestDocs();
                    break;
                case MenuOptions.ExistingDocsFIlter:
                    GetExsistingDocs();
                    break;
                case MenuOptions.Exit:
                    break;
                default:
                    break;
            }

            Console.ReadLine();
        }
        public static void Run()
        {
            Menu();
        }
    }

}
