# A.L. - zadanie rekrutacyjne

# PL

## Opis

Aplikacja służy do zarządzania meldunkami oraz pokojami za pomocą serwisów. Baza danych działa z ORM - Entity Framework. Dla sprawdzenia poprawności działania załączone są Unit Testy oraz Testy Integracyjne.

Flow bizesowe:
W standarodwym przypadku przez Booking Service pokój, który zaczyna jako wolny, zmienia status na zajęty. Możemy podać wtedy dane osoby meldującej się. Jeśli nie istnieje jeszcze w systemie to zostanie dodana. 
Gdy następuje wymeldowanie pokój automatycznie zmieniany jest jako do czyszczenia a powiązany Meldunek jest usuwany.
Dalsza kontrola nad pokojem (np oznaczenie jako wolny i gotowy lub wymagający napraw) jest możliwa przez RoomManagement Service.

Więcej szczegółów w pliku Documentation.md

Na każdym etapie możemy manualnie zmienić status pokoju bez względu na meldunek - np jeśli klient zauważy awarię w łazience to nie chcemy usuwać meldunku.
Możemy to wykonać przez wybór odpowiedniego EndPoint'u i podaniu (w przypadku gdy pokój oznaczamy jako niedostępny*) powodu zmiany.

*Pokój uznajemy jako niedostępny jeśli ma status inny niż wolny - czyli Zajęty, wymagający Czyszczenia, wymagający Napraw, Manualnie Zamknięty

## Jak uruchomić

1. Otwórz i uruchom Rozwiązanie
2. Ustaw Projekt "BasicDataAdder" jako startowy i go uruchom
3. Całość powinna wygenerować* Bazę Danych oraz uzupełnić ją przykładowymi danymi. 
4. Ustaw Projekt "Service_HotelManagement" jako startowy i go uruchom
5. Powinnien uruchomić się swagger w którym mamy dostęp do wszystkich endpoint'ów i możemy wysyłać requesty


*Może służyć też jako reset do ustawień początkowych

## Jak testować automatycznie

Solucja zawiera dwa projekty: 
Services_UnitTests oraz Services_IntegrationTests

By je urchomić należy wybrać w Eksploratorze Testów interesujące nas testy oraz je uruchomić.
Można również kliknąć prawym przyciskiem na projekt i wybrać opcję uruchomienia testów.

## Zawartość Projektu

### Basic Data Adder

Projekt służący do stworzenia bazy danych i wypełnienia jej podstawowymi danymi.

### DataHolder

Projekt z bazą danych oraz modelami dla Entity Framework.

### Service_HotelManagement

Projekt z mikroserwisami odpowiedzialnymi za meldunek oraz zarządzania pokojami

### Services_UnitTests

Projekt z unit test'ami do sprawdzania poprawności logiki

### Services_IntegrationTests

Projekt z testami integracyjnymi dla sprawdzenia poprawności działania controllerów

### Services_UnitTestsShared

Projekt z danymi współdzielonymi przez oba projekty testowe.

# ENG

## Description

The application is used to manage check-ins and rooms via services. The database operates with an ORM - Entity Framework. To check the correctness of the operation, Unit Tests and Integration Tests are included.

Business Flow:
In the standard case, a room, which initially starts as free, is changed to occupied through the Booking Service. At that point, we can provide the details of the person checking in. If they don’t exist in the system yet, they will be added.
When a checkout occurs, the room is automatically marked as requiring cleaning, and the associated check-in record is deleted.
Further control over the room (e.g., marking it as free and ready or requiring repair) can be done via the RoomManagement Service.

You can find more details in Documentation.md

At each stage, we can manually change the room's status regardless of the check-in, for instance, if the client notices a bathroom malfunction and we don’t want to delete the check-in.
This can be done by selecting the appropriate Endpoint and providing a reason for the status change (in the case where we mark the room as unavailable).

A room is considered unavailable if its status is anything other than free — i.e., Occupied, Requiring Cleaning, Requiring Repair, or Manually Closed.

## How to Run

1. Open and run the Solution.
2. Set the "BasicDataAdder" project as the startup project and run it.
3. The entire setup should generate* the Database and populate it with sample data.
4. Set the "Service_HotelManagement" project as the startup project and run it.
5. Swagger should launch, giving access to all endpoints where we can send requests.


*This can also serve as a reset to the initial settings.

## How to Test Automatically

The solution contains two projects:
Services_UnitTests and Services_IntegrationTests

To run the tests, select the tests you are interested in from the Test Explorer and run them.
You can also right-click on the project and select the option to run the tests.

## Project Contents

### BasicDataAdder

A project we use to prepare database and fill it with sample data

### DataHolder

A project containing the database and models for Entity Framework.

### Service_HotelManagement

A project containing the microservices responsible for check-ins and room management.

### Services_UnitTests

A project containing unit tests for checking the correctness of logic.

### Services_IntegrationTests

A project with integration tests for checking the correctness of controller functionality.

### Services_UnitTestsShared

A project with shared data for integration tests.

# Sample

Booking:

{
  "name": "string",
  "description": "string",
  "user": {
    "id": 0
  },
  "room": null,
  "paymentMethod": null,
  "from": "2025-02-17T02:50:33.605Z",
  "to": "2025-02-17T02:50:33.605Z"
}

Room: 

{
    "RoomIdentification":"001",
    "Price":0,
    "RoomSize":
    {
        "Id":1
    },
    "RoomStatusDetailsPair":
    {
        "RoomStatus":
        {
            "Id":1
        },
        "RoomStatusDetailsId":null,
        "RoomStatusDetails":null,
    },
    "FloorNo":0,
    "Name":"TestRoom",
    "Description":null,
}