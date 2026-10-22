# Звіт до самостійної роботи №5

**Тема:** Приклади поліморфізму в .NET.

**Проєкт:** `IndependentWork5`

## Сценарій 1. Поліморфізм через `System.Object`

Створено класи `Product`, `Customer`, `Order`, у кожному перевизначено `ToString()`. Для порівняння доданий клас `Note` **без** перевизначення.

```csharp
public override string ToString()
{
    return $"Customer: {FullName} ({City})";
}

List<object> objects = new List<object> { product, customer, order, new Note("..."), 42, "звичайний рядок" };
foreach (object obj in objects)
{
    Console.WriteLine($"{obj.GetType().Name,-10} -> {obj}");
}
```

**Як проявляється поліморфізм:** змінна має тип `object`, але `ToString()` віртуальний, тому під час виконання викликається версія фактичного типу об'єкта (пізнє зв'язування). Для `Product`, `Customer`, `Order` — наші версії; для `Note` — базова `Object.ToString()`, яка повертає просто назву типу (`Note`); `int` і `string` теж мають власні перевизначення.

**Переваги:** `Console.WriteLine`, інтерполяція рядків, відладчик — увесь код .NET вміє виводити будь-який об'єкт, нічого не знаючи про наші класи.

## Сценарій 2. Поліморфізм через `IEnumerable<string>`

```csharp
public class MyCustomCollection : IEnumerable<string>
{
    private List<string> _items = new List<string>();

    public IEnumerator<string> GetEnumerator()
    {
        foreach (string item in _items)
        {
            yield return item;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
```

**Як проявляється поліморфізм:** `foreach` працює з будь-яким типом, у якого є `GetEnumerator()`. Метод `PrintAll(string title, IEnumerable<string> items)` приймає і нашу `MyCustomCollection`, і `List<string>`, і `string[]` — для нього це однаковий тип `IEnumerable<string>`. Також `string.Join` і всі LINQ-методи одразу працюють з нашою колекцією.

Внутрішній `List<string>` закритий, назовні видно тільки перебір — колекцію не можна змінити в обхід методу `Add`.

**Переваги:** один клієнтський код для всіх колекцій; можна змінити внутрішню реалізацію (наприклад, замінити `List` на масив), і код, що перебирає колекцію, не зміниться.

## Сценарій 3. Поліморфізм через абстрактний клас `Stream`

```csharp
static void ProcessStream(Stream stream)
{
    Console.WriteLine($"Тип потоку: {stream.GetType().Name}, довжина: {stream.Length} байт");
    byte[] buffer = new byte[16];
    int read = stream.Read(buffer, 0, buffer.Length);
    ...
}

using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
    ProcessStream(fileStream);

using (MemoryStream memoryStream = new MemoryStream(bytes))
    ProcessStream(memoryStream);
```

Тестовий файл `test.txt` створюється прямо в коді через `File.WriteAllText`, тож проєкт не залежить від зовнішніх файлів.

**Як проявляється поліморфізм:** `Stream` — абстрактний клас з абстрактними `Read`, `Length`, `Position` тощо. `FileStream` реалізує їх через читання з диска, `MemoryStream` — через масив байтів у пам'яті. `ProcessStream` викликає ті самі методи, а виконується реалізація конкретного потоку. `StreamReader` усередині теж приймає просто `Stream`.

**Переваги:** метод обробки написаний один раз і працює з файлами, пам'яттю, мережею (`NetworkStream`), стисненими даними (`GZipStream`). Легко тестувати: замість файлу можна підставити `MemoryStream`.

## Висновки

У всіх трьох випадках один і той самий клієнтський код (`Console.WriteLine`, `foreach`, `ProcessStream`) працює з різними реалізаціями завдяки спільному базовому типу: віртуальним методам `Object`, інтерфейсу `IEnumerable<T>` та абстрактному класу `Stream`. Це дає гнучкість (нові типи підключаються без змін клієнтського коду) і повторне використання коду (бібліотеки .NET працюють з нашими класами).
