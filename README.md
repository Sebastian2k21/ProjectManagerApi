# Aplikacja do zarządzania projektami studenckimi - ProjectManagerApi

## Opis

Aplikacja do zarządzania projektami studenckimi umożliwia studentom organizowanie i śledzenie projektów. Umożliwia dodawanie i edycję projektów, zarządzanie zespołami oraz rekrutację członków. System obsługuje projekty otwarte i zamknięte, umożliwiając odpowiednie zarządzanie składem zespołu. Użytkownicy mają możliwość rejestracji, weryfikacji, edycji swojego profilu, a także oceniania swoich umiejętności w języku programowania. Projekt może przechodzić przez różne stany, a zespół składa się z użytkowników o różnych rolach.

## Wymagania systemowe

- Serwer obsługujący .NET Core 7.0 oraz SQL Server
- Przeglądarka internetowa

## Instalacja

1. Sklonuj repozytorium aplikacji do swojego lokalnego środowiska.
2. Zainstaluj .NET 7.0 oraz SQL Server.
3. Zaktualizuj plik konfiguracyjny aplikacji (`appsettings.json`) z odpowiednimi danymi dostępowymi do bazy danych.
4. W katalogu projektu w terminalu wykonaj polecenie `dotnet ef database update`.

## Uruchamianie

1. W katalogu projektu w terminalu użyj polecenia `dotnet run`.
2. Uruchom przeglądarkę internetową i wpisz adres URL aplikacji.


## Wkład

- Projekty otwarte umożliwiają zgłaszanie się do nich wszystkim użytkownikom systemu.
- Projekty zamknięte pozostawiają decyzję o składzie zespołu jedynie leaderowi, który może dodać członków zespołu.
- Użytkownik, który utworzył projekt, staje się jego leaderem, ale może przekazać tę rolę innemu członkowi zespołu projektu.
- Możliwe role członków zespołu: developer, tester, leader, devops.
- Każdy z członków zespołu, który brał co najmniej ¾ czasu trwania opracowania projektu, otrzymuje punkty prestiżu. Punktacja zależy od ustawień projektu, takich jak trudność.


