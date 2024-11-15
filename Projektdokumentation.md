# Projekt-Dokumentation

☝️ Alle Text-Stellen, welche mit einem ✍️ beginnen, können Sie löschen, sobald Sie die entsprechende Stellen ausgefüllt haben.

✍️ Ihr Gruppenname und Ihre Nachnamen

| Datum | Version | Zusammenfassung                                              |
| ----- | ------- | ------------------------------------------------------------ |
|       | 0.0.1   | ✍️ Jedes Mal, wenn Sie an dem Projekt arbeiten, fügen Sie hier eine neue Zeile ein und beschreiben in *einem* Satz, was Sie erreicht haben. |
|       | ...     |                                                              |
|       | 1.0.0   |                                                              |

## 1 Informieren

### 1.1 Ihr Projekt

✍️ Beschreiben Sie Ihr Projekt in einem griffigen Satz.

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

![UseCase](https://github.com/user-attachments/assets/3b5b1d62-d6fa-48c3-b837-55000eb945bd)


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

Total: 

✍️ Die Nummer hat das Format `N.m`, wobei `N` die Nummer der User Story ist, auf die sich das Arbeitspaket bezieht, und `m` von `A` an nach oben buchstabiert. Beispiel: Das dritte Arbeitspaket, das die zweite User Story betrifft, hat also die Nummer `2.C`.

✍️ Ein Arbeitspaket sollte etwa 45' für eine Person in Anspruch nehmen. Die totale Anzahl Arbeitspakete sollte etwa Folgendem entsprechen: `Anzahl R-Sitzungen` ╳ `Anzahl Gruppenmitglieder` ╳ `4`. Wenn Sie also zu dritt an einem Projekt arbeiten, für welches zwei R-Sitzungen geplant sind, sollten Sie auf `2` ╳ `3` ╳`4` = `24` Arbeitspakete kommen. Sollten Sie merken, dass Sie hier nicht genügend Arbeitspakte haben, denken Sie sich weitere "Kann"-User Stories für Kapitel 1.2 aus.

## 3 Entscheiden

Wir haben uns entschieden nach Planung (Arbeitspakete) vorzugehen.

## 4 Realisieren

| AP-№ | Datum | Zuständig | geplante Zeit | tatsächliche Zeit |
| ---- | ----- | --------- | ------------- | ----------------- |
| 1.A  |       |           |               |                   |
| ...  |       |           |               |                   |

✍️ Tragen Sie jedes Mal, wenn Sie ein Arbeitspaket abschließen, hier ein, wie lang Sie effektiv dafür hatten.

## 5 Kontrollieren

### 5.1 Testprotokoll

| TC-№ | Datum | Resultat | Tester |
| ---- | ----- | -------- | ------ |
| 1.1  |       |          |        |
| ...  |       |          |        |

✍️ Vergessen Sie nicht, ein Fazit hinzuzufügen, welches das Test-Ergebnis einordnet.


## 6 Auswerten

✍️ Fügen Sie hier eine Verknüpfung zu Ihrem Lern-Bericht ein.
