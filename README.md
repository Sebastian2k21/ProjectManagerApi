# Nazwa projektu: Aplikacja do zarządzania projektami studenckimi - ProjectManagerApi

## Opis

Aplikacja do zarządzania projektami studenckimi umożliwia studentom organizowanie i śledzenie projektów. Umożliwia dodawanie i edycję projektów, zarządzanie zespołami oraz rekrutację członków. System obsługuje projekty otwarte i zamknięte, umożliwiając odpowiednie zarządzanie składem zespołu. Użytkownicy mają możliwość rejestracji, weryfikacji, edycji swojego profilu, a także oceniania swoich umiejętności w języku programowania. Projekt może przechodzić przez różne stany, a zespół składa się z użytkowników o różnych rolach.

## Wymagania systemowe

- Serwer hostingowy z obsługą C# oraz bazą danych SQL Server
- SQL Server Management Studio
- Przeglądarka internetowa

## Instalacja

1. Sklonuj repozytorium aplikacji do swojego lokalnego środowiska.
2. Skonfiguruj serwer hostingowy z obsługą C# oraz bazą danych SQL Server.
3. Zaimportuj dostarczoną strukturę bazy danych do SQL Server Management Studio.
4. Zaktualizuj plik konfiguracyjny aplikacji (`Web.config`) z odpowiednimi danymi dostępowymi do bazy danych.

## Uruchamianie

1. Uruchom SQL Server Management Studio i skonfiguruj połączenie do bazy danych.
2. Skompiluj i uruchom aplikację w swoim środowisku deweloperskim dla C#.
3. Uruchom przeglądarkę internetową i wpisz adres URL aplikacji.

## Dokumentacja

- W folderze "docs" znajduje się dokumentacja projektu, która opisuje szczegółowe instrukcje dotyczące korzystania z aplikacji oraz architekturę systemu.

## Wkład

- Projekty otwarte umożliwiają zgłaszanie się do nich wszystkim użytkownikom systemu.
- Projekty zamknięte pozostawiają decyzję o składzie zespołu jedynie leaderowi, który może dodać członków zespołu.
- Użytkownik, który utworzył projekt, staje się jego leaderem, ale może przekazać tę rolę innemu członkowi zespołu projektu.
- Możliwe role członków zespołu: developer, tester, leader, devops.
- Każdy z członków zespołu, który brał co najmniej ¾ czasu trwania opracowania projektu, otrzymuje punkty prestiżu. Punktacja zależy od ustawień projektu, takich jak trudność.

## Licencja

Ten projekt jest udostępniany na licencji MIT.
