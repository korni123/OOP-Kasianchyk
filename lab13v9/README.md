# Лабораторна робота №13

**Тема:** Власні винятки та патерн Result.

**Варіант:** 9 — Обробка HTTP-запитів

Проєкт: `lab13v9`.

## Класи

### ApiRequestFailedException

Власний виняток, успадкований від `Exception`.

- Властивості: `StatusCode` (int), `ResponseContent` (string)
- Конструктори: без параметрів, з `message`, з `message` та `innerException`, а також з `message`, `statusCode` і `responseContent`

### Result&lt;T&gt;

Узагальнений клас результату.

- Властивості: `IsSuccess` (bool), `Value` (T), `Error` (string)
- Приватний конструктор: `Result(T? value, bool isSuccess, string error)`
- Фабричні методи: `Success(T value)`, `Failure(string error)`, `Failure(Exception ex)`

### ApiService

- `string MakeRequest(string url)` — імітує HTTP-запит. Якщо статус відповіді 4xx або 5xx, кидає `ApiRequestFailedException` зі статусом і текстом відповіді. Статус залежить від адреси: `/forbidden` → 403, `/missing` → 404, `/crash` → 500, інакше 200.
- `Result<bool> PostData(string url, string data)` — імітує POST. Повертає `Success`, а `Failure`, якщо адреса не починається з `http`, дані порожні або не є правильним JSON.

## Що показує Main

- `MakeRequest` для трьох адрес: успішна і дві з помилками. Помилки обробляються через `try-catch`, виводяться `StatusCode` і `ResponseContent`.
- `PostData` для чотирьох наборів даних: успіх і три різні `Failure`. Результат обробляється через `IsSuccess` та `Error`.

## Висновок: виняток чи Result

`MakeRequest` кидає виняток, бо відповідь 4xx/5xx означає, що метод не зміг виконати свою роботу і повернути дані. Таку помилку не можна непомітно проігнорувати, а виняток несе з собою статус і текст відповіді для обробки вище за стеком. `PostData` повертає `Result<bool>`, бо невалідні дані для POST є очікуваною ситуацією, яка трапляється часто. Для неї не потрібен стек викликів, достатньо знати, успішно чи ні, і прочитати текст помилки, а `Result` це показує прямо в сигнатурі методу і працює швидше за виняток.

## Запуск

```
cd OOP-Kasianchyk
dotnet run --project lab13v9
```

## Приклад виводу

(скріншот)
