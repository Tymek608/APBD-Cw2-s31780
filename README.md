


## Instrukcja uruchomienia

1. Wymagane środowisko: .NET 8.0 lub nowsze.
2. Klonowanie repozytorium:
   `git clone https://github.com/Tymek608/APBD-Cw2-s31780.git`
3. Otwórz projekt w środowisku IDE
4. Uruchom aplikację przyciskiem Run lub komendą `dotnet run` w terminalu wewnątrz folderu projektu.

## Architektura i decyzje projektowe

Aplikacja została podzielona na warstwy w celu zachowania porządku w kodzie i ułatwienia ewentualnej rozbudowy:
* **Domain**: Zawiera klasy modeli (np. User, Equipment, Rental), które przechowują jedynie dane.
* **Repositories**: Odpowiada za dostęp do danych. Wykorzystanie interfejsów (np. IUserRepository)
 pozwala na działanie na listach w pamięci (InMemory), umożliwiając łatwe podpięcie bazy danych w przyszłości.
* **Services**: Warstwa logiki biznesowej (np. RentalService), która zarządza procesami wypożyczeń i zwrotów.

## Zasady projektowe

### 1. Kohezja (Cohesion)
Zastosowano zasadę pojedynczej odpowiedzialności (SRP). Każda klasa ma jedno określone zadanie.
 Na przykład RentalService zajmuje się tylko procesem wypożyczania, a nie wyświetlaniem danych 
czy ich trwałym zapisem.

### 2. Sprzężenie (Coupling)
W celu uzyskania niskiego sprzężenia wykorzystano wstrzykiwanie zależności przez konstruktory.
 Klasy nie tworzą instancji swoich zależności (np. repozytoriów) samodzielnie, lecz otrzymują
 je jako interfejsy. Dzięki temu poszczególne moduły są od siebie niezależne.

### 3. Odpowiedzialności klas
* **Modele**: Definiują strukturę danych.
* **Repozytoria**: Zarządzają kolekcjami obiektów.
* **Serwisy**: Realizują reguły biznesowe i walidację (np. sprawdzanie dostępności sprzętu).
* **Program.cs**: Składa wszystkie komponenty i uruchamia logikę testową.

## Podział plików
Podział na foldery Domain, Services oraz Repositories został wprowadzony, aby oddzielić
 definicje danych od logiki ich przetwarzania. Taka struktura jest czytelniejsza niż trzymanie
 wszystkich klas w jednym folderze czy pliku.