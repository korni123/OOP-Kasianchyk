# Звіт з аналізу інкапсуляції в Open-Source проєкті

## 1. Обраний проєкт

- **Назва:** Newtonsoft.Json — бібліотека для роботи з JSON у .NET
- **Посилання на GitHub:** https://github.com/JamesNK/Newtonsoft.Json
- **Проаналізовані файли:**
  - [JsonTextWriter.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonTextWriter.cs)
  - [JsonSerializerSettings.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/JsonSerializerSettings.cs)
  - [Linq/JObject.cs](https://github.com/JamesNK/Newtonsoft.Json/blob/52fa3aef1f2cadcd3a3f874251eddc98d3efbbaa/Src/Newtonsoft.Json/Linq/JObject.cs)

## 2. Аналіз інкапсуляції

### Клас: `JsonTextWriter`

- **Опис класу:** записує JSON у текстовий потік (файл, рядок).
- **Поля:** усі приватні, частина `readonly` — ззовні їх не змінити.
  ```csharp
  private readonly TextWriter _writer;
  private char _indentChar;
  private int _indentation;
  private char _quoteChar;
  ```
- **Властивості:** read-write з логікою в `set`, наприклад `Indentation`, `QuoteChar`, `IndentChar` (див. розділ 3).
- **Індексатори/Оператори:** немає.

### Клас: `JsonSerializerSettings`

- **Опис класу:** налаштування серіалізації (форматування, глибина вкладеності тощо).
- **Поля:** `internal` — видно тільки всередині бібліотеки.
  ```csharp
  internal Formatting? _formatting;
  internal int? _maxDepth;
  ```
- **Властивості:** є звичайні властивості з логікою та автоматичні властивості:
  ```csharp
  public Formatting Formatting
  {
      get => _formatting ?? DefaultFormatting;
      set => _formatting = value;
  }

  public IList<JsonConverter> Converters { get; set; }
  ```
- **Індексатори/Оператори:** немає.

### Клас: `JObject`

- **Опис класу:** JSON-об'єкт у вигляді дерева.
- **Поля:** властивості об'єкта зберігаються в приватній колекції `private readonly JPropertyKeyedCollection _properties`.
- **Властивості:** у базовому класі `JToken` властивість `Parent` має публічний `get` і `internal set` — змінювати її може тільки сама бібліотека.
- **Індексатори/Оператори:** є індексатор, який дозволяє писати `obj["name"] = "Ivan"`:
  ```csharp
  public JToken? this[string propertyName]
  {
      get
      {
          ValidationUtils.ArgumentNotNull(propertyName, nameof(propertyName));
          JProperty? property = Property(propertyName, StringComparison.Ordinal);
          return property?.Value;
      }
      set { ... }
  }
  ```
  У `JToken` також є перевантажені оператори перетворення, наприклад `(int)token`, який кидає `ArgumentException`, якщо значення не число.

## 3. Практики валідації у властивостях

- **Приклад 1:** відступ не може бути від'ємним (`JsonTextWriter.Indentation`)
  ```csharp
  set
  {
      if (value < 0)
      {
          throw new ArgumentException("Indentation value must be greater than 0.");
      }
      _indentation = value;
  }
  ```
- **Приклад 2:** лапки можуть бути тільки `"` або `'` (`JsonTextWriter.QuoteChar`)
  ```csharp
  set
  {
      if (value != '"' && value != '\'')
      {
          throw new ArgumentException(@"Invalid JavaScript string quote character. Valid quote characters are ' and "".");
      }
      _quoteChar = value;
      UpdateCharEscapeFlags();
  }
  ```
- **Приклад 3:** максимальна глибина тільки додатна (`JsonSerializerSettings.MaxDepth`)
  ```csharp
  set
  {
      if (value <= 0)
      {
          throw new ArgumentException("Value must be positive.", nameof(value));
      }
      _maxDepth = value;
      _maxDepthSet = true;
  }
  ```
- **Висновки щодо валідації:** використовуються прості перевірки `if` і виняток `ArgumentException`. Помилка виявляється одразу, коли задали неправильне значення, а не пізніше під час запису JSON. Атрибути `DataAnnotations` не використовуються — вони більше підходять для моделей в ASP.NET, а не для бібліотеки.

## 4. Загальні висновки

В Newtonsoft.Json інкапсуляції дотримано: усі поля закриті, доступ до даних тільки через властивості, індексатор і методи. У властивостях є валідація, тому об'єкт не може опинитися в неправильному стані. Автоматичні властивості використовуються там, де перевірки не потрібні, а `internal set` захищає дані, які не можна змінювати ззовні. Такий самий підхід варто використовувати і у своїх класах.
