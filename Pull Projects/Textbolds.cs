using System.Xml.Serialization;

namespace Pull_Projects
{
    [XmlRoot("Textbolds")]
    public class Textbolds
    {
        [XmlElement("Textbold")]
        public List<Textbold> TextboldList { get; set; }
    }

    public class Textbold
    {
        public string Width { get; set; }
    }
}

/*
https://www.sonarsource.com/learn/sonarqube-readme-badges/

📛 Dostępne odznaki w SonarQube
Poniżej znajduje się lista dostępnych odznak wraz z ich opisami:

Quality Gate Status (alert_status)

Pokazuje, czy projekt przeszedł zdefiniowane progi jakości (zielony: passed, czerwony: failed).

Code Coverage (coverage)

Procent pokrycia kodu testami jednostkowymi.

Duplicated Lines Density (duplicated_lines_density)

Procent zduplikowanych linii kodu w projekcie.

Lines of Code (ncloc)

Liczba linii kodu (nie uwzględnia komentarzy i pustych linii).

Bugs (bugs)

Liczba błędów wpływających na niezawodność aplikacji.

Vulnerabilities (vulnerabilities)

Liczba luk bezpieczeństwa w kodzie.

Code Smells (code_smells)

Liczba "code smells", czyli fragmentów kodu utrudniających jego utrzymanie.
community.sonarsource.com

Security Rating (security_rating)

Ocena bezpieczeństwa kodu w skali od A (najlepsza) do E (najgorsza).
sonarsource.com

Reliability Rating (reliability_rating)

Ocena niezawodności kodu w skali od A do E.

Maintainability Rating (sqale_rating)

Ocena łatwości utrzymania kodu w skali od A do E.

Technical Debt (sqale_index)

Szacowany czas potrzebny na usunięcie problemów z utrzymaniem kodu (w minutach lub dniach).

Security Hotspots

Liczba miejsc w kodzie wymagających przeglądu pod kątem bezpieczeństwa.

AI Code Assurance

Status analizy kodu wygenerowanego przez AI: Off, Pass lub Fail.

Badge (metric)	Opis (PL, skrócony)
alert_status	Status Quality Gate (OK / ERROR)
coverage	Pokrycie kodu testami (%)
duplicated_lines_density	Zduplikowane linie (%)
ncloc	Liczba linii kodu (bez komentarzy)
bugs	Liczba błędów (niezawodność)
vulnerabilities	Liczba luk bezpieczeństwa
code_smells	Liczba problematycznych fragmentów kodu
security_rating	Ocena bezpieczeństwa (A–E)
reliability_rating	Ocena niezawodności (A–E)
sqale_rating	Ocena łatwości utrzymania (A–E)
sqale_index	Dług techniczny (czas naprawy)
security_hotspots	Miejsca wymagające przeglądu bezpieczeństwa
ai_code_assistance_status	Status analizy kodu AI (Off / Pass / Fail)

 ||Serwis||alert_status||coverage||duplicated_lines_density||ncloc||bugs||vulnerabilities||code_smells||
|UserService|!https://sonar.example.com/api/project_badges/measure?project=project_userservice&metric=alert_status&token=xxx|height=20!|...|
...

 */

/*

 // 1. Mock for the scoped service (IDocumentsService)
    var documentsServiceMock = new Mock<IDocumentsService>();
    documentsServiceMock
        .Setup(x => x.GetDocumentLink())
        .ReturnsAsync("polska");

    // 2. Mock for IServiceProvider
    var serviceProviderMock = new Mock<IServiceProvider>();
    serviceProviderMock
        .Setup(sp => sp.GetService(typeof(IDocumentsService)))
        .Returns(documentsServiceMock.Object);

    // 3. Mock for IServiceScope
    var serviceScopeMock = new Mock<IServiceScope>();
    serviceScopeMock
        .Setup(s => s.ServiceProvider)
        .Returns(serviceProviderMock.Object);

    // 4. Mock for IServiceScopeFactory
    var scopeFactoryMock = new Mock<IServiceScopeFactory>();
    scopeFactoryMock
        .Setup(f => f.CreateScope())
        .Returns(serviceScopeMock.Object);

    // 5. Teraz wstrzykujesz scopeFactoryMock do klasy testowanej
    var sut = new DocumentLinkProvider(scopeFactoryMock.Object);

 */