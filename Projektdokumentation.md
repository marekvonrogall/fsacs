# Projekt-Dokumentation


Marek, Pascal, Manuel, Stefan

| Datum | Version | Zusammenfassung                                              |
| ----- | ------- | ------------------------------------------------------------ |
|08.11.2024| 0.0.1   |Heute haben wir den Projektantrag abgegeben und uns zu potenziellen Möglichkeiten infromiert wie wir das Projekt realisieren können.|
|15.11.2024| 0.6.1 |Heute haben wir mit dem Programieren angefangen. Wir haben mit den Arbeitspaketen 1.A und 2.A angefangen|
|22.11.2024| 0.11.5 |Heute war ein sehr produktiver Tag, wir konnten viele Arbeitspakete erledigen, 1.A, 2.A, 2.B, 3.A, 4.B, 5.A ein grossteil der Funktionalität des Clients wurde implementiert.|
|06.12.2024| 0.12.15 |Heute konnten wir die Arbeitspakete 7.B, 4.D, 5.A erledigen. Wir konnten gut arbeiten.|
|13.12.2024| 1.1.0|Wir haben heute daran gearbeitet den Server und den Client zu verbinden. Dies hat nicht gut funktioniert, wir hatten mehrere Probleme und wir haben uns dazu entschieden, weil wir nichtmehr so viel Zeit hatten einen kleinen Server zu erstellen der die nötige funktionalität abdeckt, die wir für unsere Applikation benötigen.|

## 1 Informieren

### 1.1 Ihr Projekt

In unserem Projekt erstellen wir eine App um Dateien über Peer to Peer zu Senden, das verbinden der Clients wird mit einem Server geamcht.

### 1.2 User Stories

| US-№ | Verbindlichkeit | Typ  | Beschreibung                       |
| ---- | --------------- | ---- | ---------------------------------- |
| 1    |   Muss              | funktional     | Als Benutzer möchte ich, dass ich Dateien mit einem anderen Computer austauschen kann. |
| 2    |    Muss             | funktional     | Als Entwickler möchte ich, dass die IP-Adresse in einer Datenbank gespeichert wird, sobald der Benutzer sich registriert, um diese anschliessend zu verwenden.  |
| 3    |   Muss              | funktional     | Als Entwickler möchte ich, dass die IP-Adresse wieder entfernt wird, sobald der Benutzer das Programm schliesst, damit die Datenbank nicht unnötig gefüllt ist. |
| 4    |   Muss              | funktional     | Als Benutzer möchte ich, dass ich die IP-Adressen über einen Vermittler erhalte, um Dateien empfangen zu können. |
| 5    |   Muss              | funktional     | Als Benutzer möchte ich, dass ich die Datenübertragung annehmen muss, um die Datei zu erhalten. |
| 6    |   Muss              | funktional     | Als Benutzer möchte ich, dass ich die Datenübertragung verweigern kann, um zu verhindern, dass ich ungewollte Dateien erhalte. |
| 7    |   Muss              | funktional     | Als Entwickler möchte ich, dass die Kommunikation zwischen Server und Client über einen Websocket läuft, um die Kommunikation zu ermöglichen. |



### 1.3 Testfälle

| TC-№ | Ausgangslage | Eingabe | Erwartete Ausgabe |
| ---- | ------------ | ------- | ----------------- |
| 1.1  |Zwei Nutzer haben sich mit dem Server verbunden und wollen eine Datei austauschen.| - |Datei wird übertragen.|
| 2.1  |Benutzer loggt sich ein.| - |IP wird in der Datenbank gespeichert.|
| 3.1  |Benutzer loggt sich aus.| - |Benutzer wird aus der Datenbank gelöscht.|
| 4.1  |Benutzer ist eingeloggt.| - |Benutzer wird anderen Benutzern als Online angezeigt.|
| 5.1  |Anderer Benutzer schickt eine Anfrage, um eine Datei zu schicken.| Anfrage wird angenommen.|Datei wird übertragen.|
| 6.1  |Anderer Benutzer schickt eine Anfrage, um eine Datei zu schicken.| Anfrage wird abgelehnt.|Datei wird nicht übertragen.|




### 1.4 Diagramme
#### UseCase

![UseCase](https://github.com/user-attachments/assets/d491114a-9eb1-49f0-bac9-1116f65da5b0)

#### Architecture

![Architecture](https://github.com/user-attachments/assets/19467aaa-b6cb-42d8-a7b1-32a2de122610)



## 2 Planen

| AP-№ | Frist | Zuständig | Beschreibung | geplante Zeit (h) |
| ---- | ----- | --------- | ------------ | ------------- |
| 1.A  |  22/11/24     |      Stefan, Marek     |   Implementierung der Peer-to-Peer-Verbindung in C# für den Dateitransfer zwischen zwei Clients.           |       7        |
| 1.B  |  29/11/24     |      Stefan, Marek     |   GUI Erstellen und Integration der Peer-to-Peer-Verbindung in das GUI.           |       2        |
| 2.A  |  15/11/24     |      Manuel, Pascal    |   Einrichtung der Datenbank für die Speicherung der IP-Adresse beim Registrieren des Clients.           |      4         |
| 2.B  |  29/11/24     |      Manuel, Pascal     |   Entwicklung des Anmelde-Services für die Registrierung und Speicherung der IP.           |      2         |
| 2.C  |  13/12/24     |      Stefan, Marek     |   Anmelden beim Server beim Client implementieren.          |      1         |
| 3.A  |  29/11/24     |      Manuel, Pascal     |   Erweiterung des Anmelde-Services: IP-Adresse des Clients bei Abmeldung oder Verbindungsverlust entfernen.           |      2         |
| 3.B  |  13/12/24     |      Stefan, Marek     |   Abmelden beim Server beim Client implementieren.          |      1         |
| 4.A  |  06/12/24     |      Manuel, Pascal    |   Implementierung des BenutzerAusgabe-Services: Bereitstellung verfügbarer Clients mit IP-Adressen.           |      2         |
| 4.B  |  06/12/24     |      Stefan, Marek     |   Annahme des Adressbuches (Benutzerausgabe-Service) beim Client implementieren.          |      2         |
| 4.C  |  13/12/24     |      Manuel, Pascal    |   Entwicklung des Vermittler-Services: Weiterleitung der IP-Adressen (& Dateiname) zwischen Clients.           |      4         |
| 4.D  |  13/12/24     |      Stefan, Marek     |   "Anklopfen" implementieren (Nachricht an den Vermittler-Service).           |      2         |
| 5.A  |  06/12/24     |      Stefan, Marek     |   Implementierung der Logik zur Annahme von Dateiübertragungen im Client (+ Speicherung der Datei).          |      2         |
| 6.A  |  06/12/24     |      Stefan, Marek     |   Implementierung der Logik zur Ablehnung von Dateiübertragungen im Client.           |      1         |
| 7.A  |  22/11/24     |      Manuel, Pascal    |   Implementierung des API-Gateways und WebSocket-Support für die Kommunikation zwischen Client und Server. |      4         |
| 7.B  |  29/11/24     |      Stefan, Marek     |   Implementierung der WebSocket-Verbindung beim Client für die Kommunikation zwischen Client und Server.           |      2         |

## 3 Entscheiden

Wir haben uns entschieden nach Planung (Arbeitspakete) vorzugehen.

## 4 Realisieren

| AP-№ | Datum | Zuständig | geplante Zeit | tatsächliche Zeit |
| ---- | ----- | --------- | ------------- | ----------------- |
| 1.A  |22.11.2024|Stefan, Marek|7|5|
| 1.B  |22.11.2024|Stefan, Marek|2|1|
| 2.A  |22.11.2024|Pascal, Manuel|4|3|
| 2.B  |22.11.2024|Pascal, Manuel|2|3|
| 3.A  |22.11.2024|Pascal, Manuel|2|1|
| 4.B  |22.11.2024|Stefan, Marek|2|2|
| 5.A  |22.11.2024|Stefan, Marek|2|1|
| 7.B  |06.12.2024|Stefan, Marek|2|2|
| 4.D  |06.12.2024|Stefan, Marek|2|3|
| 5.A  |06.12.2024|Stefan, Marek|2|2|
| 4.C  |13.12.2024|Pascal,Manuel|4|6|


## 5 Kontrollieren

### 5.1 Testprotokoll

| TC-№ | Datum | Resultat | Tester |
| ---- | ----- | -------- | ------ |
| 1.1  |20.12.2024|OK|Stefan Jesenko|
| 2.1  |20.12.2024|NOK|Stefan Jesenko|
| 3.1  |20.12.2024|NOK|Stefan Jesenko|
| 4.1  |20.12.2024|OK|Stefan Jesenko|
| 5.1  |20.12.2024|OK|Stefan Jesenko|
| 6.1  |20.12.2024|OK|Stefan Jesenko|

Die Applikation funktioniert so wie geplant, wir konnten aber den Server nicht so umsetzen wie geplant, wegen Zeitgründen wurde eine kleine Version des Servers erstellt, der die funktionalen Aspekte abdeckt, bei dem werden die Daten nicht in einer Datenbank gespeichert, deswegen sind manche Testfälle NOK.

## 6 Auswerten
[Portfolio von Stefan H. Jesenko](https://portfolio.bbbaden.ch/view/view.php?t=3120b43bcf86994cec78)

[Portfolio von Pascal Oestrich](https://portfolio.bbbaden.ch/view/view.php?t=e52b88f9a5edc422b625)

[Portfolio von Manuel Greub](https://portfolio.bbbaden.ch/view/view.php?t=b4c500fab0a1fd25dca6) 

[Portfolio von Marek von Rogall](https://portfolio.bbbaden.ch/view/view.php?t=31d66b8eacbc0990a598)

