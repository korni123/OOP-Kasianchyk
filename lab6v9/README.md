# Лабораторна робота №6

**Тема:** Наслідування. Ключові слова base, override, virtual. Приховування членів (new).

**Варіант:** 9 — ієрархія `Weapon → Sword → Bow`

## Опис ієрархії

- **Weapon (базовий клас):** поля `Name`, `Damage`; метод `virtual void Attack()`; метод `GetWeaponType()`
- **Sword (похідний):** додаткове поле `Material`; `override void Attack()`; власний метод `Parry()`; конструктор викликає `base(...)`
- **Bow (похідний):** додаткове поле `ArrowType`; `override void Attack()`; власний метод `Aim()`; конструктор викликає `base(...)`
- **Демонстрація `new`:** у `Sword` створено `new string GetWeaponType()`, що приховує (а не перевизначає) однойменний метод з `Weapon`

## Запуск проєкту

\`\`\`bash
cd lab6v9
dotnet run
\`\`\`

## Результат виконання програми

<img width="663" height="292" alt="image" src="https://github.com/user-attachments/assets/acedf847-2801-48b5-be0c-880cae4b0bb2" />
