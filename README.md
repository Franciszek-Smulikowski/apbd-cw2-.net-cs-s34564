# APBD Ćwiczenie 2 - Uczelniana Wypożyczalnia Sprzętu

## Opis projektu
Projekt stanowi aplikację konsolową w języku C# służącą do zarządzania uczelnianą wypożyczalnią sprzętu. Został zrealizowany ze szczególnym naciskiem na czytelność kodu, zasady programowania obiektowego oraz odpowiednią separację odpowiedzialności.

## Główne funkcjonalności
- Rejestracja użytkowników (z podziałem na studentów i pracowników).
- Ewidencja sprzętu uczelnianego (m.in. laptopy, rzutniki, aparaty).
- Obsługa procesu wypożyczania z zachowaniem walidacji dostępności oraz rygorystycznych limitów przydzielonych dla konkretnych grup użytkowników.
- Obsługa zwrotów z wykorzystaniem modułu naliczającego kary finansowe za opóźnienia.
- Generowanie raportów tekstowych podsumowujących stan inwentarza oraz wykazujących bieżące statystyki wypożyczeń.

## Uruchomienie projektu
Aby skompilować aplikację i uruchomić przygotowany scenariusz demonstracyjny, należy wykonać w terminalu poniższe polecenia:

```bash
dotnet build Cw2App/Cw2App.csproj
dotnet run --project Cw2App/Cw2App.csproj
```

## Struktura projektu
- `Models/` – klasy domenowe, np. `Equipment`, `User`, `Rental` oraz ich typy pochodne.
- `Services/` – logika biznesowa, np. `RentalService`, `ReportService`, `LoanPolicy`, `PenaltyCalculator`.
- `Demo/` – scenariusz demonstracyjny uruchamiany z poziomu aplikacji.
- `Program.cs` – punkt wejścia, który tylko uruchamia scenariusz demonstracyjny.

Taki podział został wybrany w celu oddzielenia modelu domenowego od logiki operacyjnej i warstwy konsolowej. Dzięki temu kod jest bardziej czytelny, klasy mają bardziej jednoznaczne odpowiedzialności, a ewentualne zmiany w logice biznesowej nie wymagają modyfikacji wielu niepowiązanych elementów projektu.

## Architektura i decyzje projektowe
W celu zwiększenia elastyczności systemu zdecydowano się na zastosowanie podziału na warstwy:

- **Izolacja modelu domenowego:** Klasy takie jak `Equipment`, `User` oraz `Rental` odpowiadają wyłącznie za odzwierciedlenie systemowych encji i relacji między nimi. Pozbawiono je złożonej logiki biznesowej, aby stanowiły czyste struktury danych.
- **Logika w warstwie usług (Services):** Za prawidłowy przebieg procesów odpowiada w głównej mierze klasa `RentalService`. Utrzymanie pliku wejściowego `Program.cs` w jak najbardziej surowej formie (odpowiada on jedynie za wywołanie klasy dema) pozwala na prostą, bezinwazyjną migrację rozwiązania w przyszłości np. wymieniając interfejs konsolowy na Web API.
- **Wysoka spójność (Cohesion) i niski poziom powiązań (Coupling):** Aplikacja dąży do hermetyzacji zadań w wyspecjalizowanych komponentach. Reguły nakładania limitów i modelowania opłat wyodrębniono do odpowiednich klas pomocniczych: `LoanPolicy` i `PenaltyCalculator`. Zwiększa to czytelność kodu głównego serwisu oraz chroni go przed szkodliwym wpływem zmian w mechanizmach samych zasad biznesowych. Do formatowania i prezentacji danych o stanie aplikacji dedykowano natomiast oddzielną klasę – `ReportService`.
