# Самостійна робота №4

**Тема:** Розширення ієрархії та наслідки змін базового класу.

**Проєкт:** `IndependentWork4`

## Ієрархія класів

```
LibraryItem
├── Book
│   └── AudioBook
└── Journal
```

- **LibraryItem (базовий):** поля `_uniqueId`, `_title`, `_publicationYear`; властивості `UniqueId` (тільки читання), `Title`, `PublicationYear`; конструктор `LibraryItem(string uniqueId, string title, int publicationYear)`; `virtual void DisplayDetails()` — виводить базову інформацію.
- **Book:** поле `_author`, властивість `Author`; конструктор викликає `base(...)`; `override DisplayDetails()` — спочатку `base.DisplayDetails()`, потім автор.
- **Journal:** поле `_issueNumber`, властивість `IssueNumber`; конструктор викликає `base(...)`; `override DisplayDetails()` додає номер випуску.
- **AudioBook : Book:** поле `_narrator`, властивість `Narrator`; конструктор `AudioBook(uniqueId, title, publicationYear, author, narrator) : base(uniqueId, title, publicationYear, author)`; `override DisplayDetails()` додає оповідача.

## Що демонструє Main

- створюються `Book` і `Journal`, для кожного викликається `DisplayDetails()`;
- створюється `AudioBook`, її `DisplayDetails()` виводить дані з усіх трьох рівнів ієрархії (LibraryItem → Book → AudioBook);
- усі об'єкти кладуться в `List<LibraryItem>` і в циклі викликається `DisplayDetails()` — для кожного спрацьовує своя перевизначена версія (поліморфізм).

## Звіт

### Які зміни довелося внести в похідні класи після модифікації LibraryItem (пункт 4)

Спочатку ієрархія була без ідентифікатора:

```csharp
public LibraryItem(string title, int publicationYear) { ... }

public Book(string title, int publicationYear, string author)
    : base(title, publicationYear) { ... }

public Journal(string title, int publicationYear, int issueNumber)
    : base(title, publicationYear) { ... }
```

Після того, як у `LibraryItem` додали поле `_uniqueId`, властивість `UniqueId` і змінили конструктор на `LibraryItem(string uniqueId, string title, int publicationYear)`, проєкт перестав компілюватися:

```
error CS7036: There is no argument given that corresponds to the required parameter 'publicationYear'
of 'LibraryItem.LibraryItem(string, string, int)'
```

Довелося змінити **обидва** похідні класи:
1. додати параметр `string uniqueId` у конструктори `Book` і `Journal`;
2. передати його далі: `: base(uniqueId, title, publicationYear)`;
3. виправити всі місця в `Main`, де створювалися `Book` і `Journal` (додати ідентифікатор першим аргументом).

`DisplayDetails()` у похідних класах змінювати не довелося — вони викликають `base.DisplayDetails()`, тому новий ідентифікатор з'явився у виводі автоматично. Новий клас `AudioBook` вже писався під нову версію конструктора.

### Чому ці зміни були необхідні

- Похідний клас не може створитися, поки не відпрацює конструктор базового класу. У `LibraryItem` більше немає конструктора без `uniqueId`, тому кожен нащадок **зобов'язаний** його передати через `base(...)`.
- Конструктори не наслідуються, тому зміна сигнатури базового конструктора «розходиться» по всіх прямих нащадках, а через них — і по непрямих (`AudioBook` змушений передавати `uniqueId` у `Book`).
- Поле `_uniqueId` приватне, тому нащадки не можуть встановити його самі — тільки через конструктор базового класу. Це правильно з точки зору інкапсуляції, але саме тому зміна ламає похідні класи.

### Переваги та недоліки наслідування в контексті розширення ієрархій

**Переваги:**
- новий тип додається легко, без змін у старому коді: `AudioBook` успадкувала всю логіку `Book` і `LibraryItem` і лише дописала оповідача (принцип Open/Closed);
- спільний код (`Title`, `PublicationYear`, `UniqueId`, базовий вивід) написано один раз;
- поліморфізм: код, що працює з `List<LibraryItem>`, підтримує `AudioBook` без жодних змін.

**Недоліки:**
- **крихкий базовий клас** — навіть невелика зміна в `LibraryItem` (новий параметр конструктора) змушує правити всіх нащадків; у великій ієрархії це десятки файлів;
- сильна зв'язаність: нащадки залежать від деталей реалізації базового класу (порядок параметрів, що робить `base.DisplayDetails()`);
- ієрархія жорстка: якщо з'явиться «аудіожурнал», його не можна успадкувати і від `Journal`, і від `AudioBook` одночасно.

**Порівняння з композицією.** Аудіо-частину можна було б винести в окремий клас, наприклад `AudioInfo { Narrator, Duration }`, і додати поле `AudioInfo? Audio` в `LibraryItem` або в окремі класи. Тоді «аудіо» могли б мати і книги, і журнали, а зміни в `AudioInfo` не зачіпали б конструктори всієї ієрархії.

**Коли наслідування доцільне:** коли є чітке відношення «є» (`AudioBook` **є** `Book`), базовий клас стабільний і рідко змінюється, а нащадки дійсно розширюють поведінку. **Коли призводить до проблем:** коли базовий клас часто змінюється, ієрархія глибока, або потрібно комбінувати кілька незалежних ознак — тоді краще композиція та інтерфейси.

## Запуск

```
cd Самостійні/Самостійна_04/IndependentWork4
dotnet run
```

## Приклад виводу

